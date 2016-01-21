
ALTER PROCEDURE [dbo].[mp_Users_ConfirmRegistration] 

@EmptyGuid					uniqueidentifier,
@RegisterConfirmGuid		uniqueidentifier

AS
UPDATE	mp_Users
SET		IsLockedOut = 0,
        EmailConfirmed = 1,
		RegisterConfirmGuid = @EmptyGuid
		

WHERE	RegisterConfirmGuid = @RegisterConfirmGuid

GO

