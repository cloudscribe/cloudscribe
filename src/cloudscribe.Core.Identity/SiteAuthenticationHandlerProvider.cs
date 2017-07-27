//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace cloudscribe.Core.Identity
//{
//    public class SiteAuthenticationHandlerProvider : IAuthenticationHandlerProvider
//    {
//        public SiteAuthenticationHandlerProvider(
//            IAuthenticationSchemeProvider schemes
//            )
//        {
//            Schemes = schemes;
//        }

//        /// <summary>
//        /// The <see cref="IAuthenticationHandlerProvider"/>.
//        /// </summary>
//        public IAuthenticationSchemeProvider Schemes { get; }

//        // handler instance cache, need to initialize once per request
//        private Dictionary<string, IAuthenticationHandler> _handlerMap = new Dictionary<string, IAuthenticationHandler>(StringComparer.Ordinal);

//        /// <summary>
//        /// Returns the handler instance that will be used.
//        /// </summary>
//        /// <param name="context">The context.</param>
//        /// <param name="authenticationScheme">The name of the authentication scheme being handled.</param>
//        /// <returns>The handler instance.</returns>
//        public async Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
//        {
//            if (_handlerMap.ContainsKey(authenticationScheme))
//            {
//                return _handlerMap[authenticationScheme];
//            }

//            var scheme = await Schemes.GetSchemeAsync(authenticationScheme);
//            if (scheme == null)
//            {
//                return null;
//            }
//            var handler = (context.RequestServices.GetService(scheme.HandlerType) ??
//                ActivatorUtilities.CreateInstance(context.RequestServices, scheme.HandlerType))
//                as IAuthenticationHandler;
//            if (handler != null)
//            {
//                await handler.InitializeAsync(scheme, context);
//                _handlerMap[authenticationScheme] = handler;
//            }
//            return handler;
//        }
//    }
//}
