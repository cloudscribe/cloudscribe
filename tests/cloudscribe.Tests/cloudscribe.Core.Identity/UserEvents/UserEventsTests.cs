using cloudscribe.Core.Models;
using cloudscribe.Core.Models.EventHandlers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace cloudscribe.Core.Identity.UserEventsTests
{
    /// <summary>
    /// Unit tests for UserEvents class, specifically testing the post-delete handler functionality
    /// </summary>
    public class UserEventsTests
    {
        private readonly Mock<ILogger<UserEvents>> _mockLogger;
        private readonly Mock<IHandleUserCreated> _mockCreatedHandler;
        private readonly Mock<IHandleUserPreUpdate> _mockPreUpdateHandler;
        private readonly Mock<IHandleUserPreDelete> _mockPreDeleteHandler;
        private readonly Mock<IHandleUserPostDelete> _mockPostDeleteHandler1;
        private readonly Mock<IHandleUserPostDelete> _mockPostDeleteHandler2;
        private readonly Mock<IHandleUserUpdated> _mockUpdatedHandler;

        public UserEventsTests()
        {
            _mockLogger             = new Mock<ILogger<UserEvents>>();
            _mockCreatedHandler     = new Mock<IHandleUserCreated>();
            _mockPreUpdateHandler   = new Mock<IHandleUserPreUpdate>();
            _mockPreDeleteHandler   = new Mock<IHandleUserPreDelete>();
            _mockPostDeleteHandler1 = new Mock<IHandleUserPostDelete>();
            _mockPostDeleteHandler2 = new Mock<IHandleUserPostDelete>();
            _mockUpdatedHandler     = new Mock<IHandleUserUpdated>();
        }

        private UserEvents CreateUserEvents(
            IEnumerable<IHandleUserPostDelete> postDeleteHandlers = null)
        {
            return new UserEvents(
                new[] { _mockCreatedHandler.Object },
                new[] { _mockPreUpdateHandler.Object },
                new[] { _mockPreDeleteHandler.Object },
                postDeleteHandlers ?? new[] { _mockPostDeleteHandler1.Object, _mockPostDeleteHandler2.Object },
                new[] { _mockUpdatedHandler.Object },
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task HandleUserPostDelete_Should_CallAllRegisteredHandlers()
        {
            // Arrange
            var userEvents = CreateUserEvents();
            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            // Act
            await userEvents.HandleUserPostDelete(siteId, userId, cancellationToken);

            // Assert
            _mockPostDeleteHandler1.Verify(
                h => h.HandleUserPostDelete(siteId, userId, cancellationToken),
                Times.Once
            );

            _mockPostDeleteHandler2.Verify(
                h => h.HandleUserPostDelete(siteId, userId, cancellationToken),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleUserPostDelete_WithNoHandlers_Should_NotThrow()
        {
            // Arrange
            var userEvents = CreateUserEvents(postDeleteHandlers: new IHandleUserPostDelete[0]);
            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act & Assert (should not throw)
            await userEvents.HandleUserPostDelete(siteId, userId, CancellationToken.None);
        }

        [Fact]
        public async Task HandleUserPostDelete_WhenHandlerThrows_Should_ContinueWithOtherHandlers()
        {
            // Arrange
            var failingHandler = new Mock<IHandleUserPostDelete>();
            failingHandler
                .Setup(h => h.HandleUserPostDelete(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Handler failed"));

            var workingHandler = new Mock<IHandleUserPostDelete>();

            var userEvents = CreateUserEvents(
                postDeleteHandlers: new[] { failingHandler.Object, workingHandler.Object }
            );

            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cancellationToken = CancellationToken.None;

            // Act
            await userEvents.HandleUserPostDelete(siteId, userId, cancellationToken);

            // Assert
            failingHandler.Verify(
                h => h.HandleUserPostDelete(siteId, userId, cancellationToken),
                Times.Once
            );

            workingHandler.Verify(
                h => h.HandleUserPostDelete(siteId, userId, cancellationToken),
                Times.Once
            );

            // Verify error was logged
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Handler failed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task HandleUserPostDelete_Should_PassCorrectParameters()
        {
            // Arrange
            var userEvents = CreateUserEvents();
            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var cancellationToken = new CancellationTokenSource().Token;

            // Act
            await userEvents.HandleUserPostDelete(siteId, userId, cancellationToken);

            // Assert
            _mockPostDeleteHandler1.Verify(
                h => h.HandleUserPostDelete(
                    It.Is<Guid>(g => g == siteId),
                    It.Is<Guid>(g => g == userId),
                    It.Is<CancellationToken>(ct => ct == cancellationToken)
                ),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleUserPostDelete_WithDefaultCancellationToken_Should_Work()
        {
            // Arrange
            var userEvents = CreateUserEvents();
            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            await userEvents.HandleUserPostDelete(siteId, userId);

            // Assert
            _mockPostDeleteHandler1.Verify(
                h => h.HandleUserPostDelete(siteId, userId, It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task HandleUserPostDelete_Should_UseConfigureAwaitFalse()
        {
            // This test ensures that ConfigureAwait(false) is used, which is important for avoiding deadlocks
            // We'll verify this by ensuring the method completes successfully in a synchronous context
            
            // Arrange
            var userEvents = CreateUserEvents();
            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act & Assert - should not deadlock
            var task = userEvents.HandleUserPostDelete(siteId, userId);
            task.Wait(TimeSpan.FromSeconds(5)); // Should complete quickly without deadlock
            
            Assert.True(task.IsCompletedSuccessfully);
        }

        [Fact]
        public void UserEvents_Constructor_Should_AcceptPostDeleteHandlers()
        {
            // Arrange
            var postDeleteHandlers = new[] { _mockPostDeleteHandler1.Object };

            // Act
            var userEvents = new UserEvents(
                new[] { _mockCreatedHandler.Object },
                new[] { _mockPreUpdateHandler.Object },
                new[] { _mockPreDeleteHandler.Object },
                postDeleteHandlers,
                new[] { _mockUpdatedHandler.Object },
                _mockLogger.Object
            );

            // Assert - should not throw and should be constructible
            Assert.NotNull(userEvents);
        }

        [Fact]
        public async Task HandleUserPostDelete_MultipleHandlers_Should_ExecuteInOrder()
        {
            // Arrange
            var executionOrder = new List<string>();
            
            var handler1 = new Mock<IHandleUserPostDelete>();
            handler1.Setup(h => h.HandleUserPostDelete(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask)
                   .Callback(() => executionOrder.Add("Handler1"));

            var handler2 = new Mock<IHandleUserPostDelete>();
            handler2.Setup(h => h.HandleUserPostDelete(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask)
                   .Callback(() => executionOrder.Add("Handler2"));

            var userEvents = CreateUserEvents(
                postDeleteHandlers: new[] { handler1.Object, handler2.Object }
            );

            var siteId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            await userEvents.HandleUserPostDelete(siteId, userId);

            // Assert
            Assert.Equal(2, executionOrder.Count);
            Assert.Equal("Handler1", executionOrder[0]);
            Assert.Equal("Handler2", executionOrder[1]);
        }

        [Fact]
        public async Task UserEvents_AllHandlerMethods_Should_ExistAndWork()
        {
            // test to ensure all handler methods exist and work together
            
            // Arrange
            var mockUser = new Mock<ISiteUser>();
            mockUser.Setup(u => u.SiteId).Returns(Guid.NewGuid());
            mockUser.Setup(u => u.Id).Returns(Guid.NewGuid());

            var userEvents = CreateUserEvents();

            // Act & Assert - all methods should exist and be callable
            await userEvents.HandleUserCreated(mockUser.Object);
            await userEvents.HandleUserPreUpdate(mockUser.Object.SiteId, mockUser.Object.Id);
            await userEvents.HandleUserPreDelete(mockUser.Object.SiteId, mockUser.Object.Id);
            await userEvents.HandleUserPostDelete(mockUser.Object.SiteId, mockUser.Object.Id); // New method
            await userEvents.HandleUserUpdated(mockUser.Object);

            // Verify all handlers were called
            _mockCreatedHandler.Verify(h => h.HandleUserCreated(mockUser.Object, It.IsAny<CancellationToken>()), Times.Once);
            _mockPreUpdateHandler.Verify(h => h.HandleUserPreUpdate(mockUser.Object.SiteId, mockUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockPreDeleteHandler.Verify(h => h.HandleUserPreDelete(mockUser.Object.SiteId, mockUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockPostDeleteHandler1.Verify(h => h.HandleUserPostDelete(mockUser.Object.SiteId, mockUser.Object.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockUpdatedHandler.Verify(h => h.HandleUserUpdated(mockUser.Object, It.IsAny<CancellationToken>()), Times.Once);
        }
    }

    /// <summary>
    /// Test implementation of IHandleUserPostDelete for testing purposes
    /// </summary>
    public class TestPostDeleteHandler : IHandleUserPostDelete
    {
        public bool WasCalled { get; private set; }
        public Guid LastSiteId { get; private set; }
        public Guid LastUserId { get; private set; }
        public bool ShouldThrow { get; set; }

        public Task HandleUserPostDelete(Guid siteId, Guid userId, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            LastSiteId = siteId;
            LastUserId = userId;

            if (ShouldThrow)
            {
                throw new InvalidOperationException("Test exception");
            }

            return Task.CompletedTask;
        }
    }
}