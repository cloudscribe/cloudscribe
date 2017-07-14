// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-06-30
// Last Modified:			2017-07-14
// 

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Core.Web.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Web.ExtensionPoints
{
    /// <summary>
    /// you can inject your implementation of IHandleCustomRegistration as scoped, and it will be invoked during the user registration process
    /// allowing you to handle custom form fields.
    /// </summary>
    public interface IHandleCustomRegistration
    {
        /// <summary>
        /// This makes is possible for you to use a custom view name or even different views per tenant.
        /// There is also an empty partial view named RegisterMiddle.cshtml that is invoked by the default Register
        /// so you could override just that partial view to add some additional fields in the middle of the form 
        /// and use the default view name here. But if you need to control the entire view then return
        /// a custom name. Note that it is also possible to have per theme views so you can override a view that way as well.
        /// So there are many possible solutions for custom register views, here we are just trying to make it even more flexible,
        /// </summary>
        /// <param name="site"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<string> GetRegisterViewName(ISiteContext site, HttpContext httpContext);


        /// <summary>
        /// a method that will be invoked fromm the AccountController Register GET action method.
        /// you could add custom field data to the ViewDataDictionary and use that in a custom view to populate additional custom fields
        /// </summary>
        /// <param name="site"></param>
        /// <param name="viewModel"></param>
        /// <param name="httpContext"></param>
        /// <param name="viewData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleRegisterGet(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        /// A method that will be invoked fromm the AccountController Register POST action method as part of model validation.
        /// You could process additional form parameters and if needed add custom errors to the modelState.
        /// return true if everything is valid and return false if you added any errors to modelState.
        /// You can also if needed add data to viewData.
        /// You should not persist custom data here because this is called as part of model validation and there could be other validation errors from the main viewmodel.
        /// Use HandleRegisterPostSuccess to persist any addtional data.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="viewModel"></param>
        /// <param name="httpContext"></param>
        /// <param name="viewData"></param>
        /// <param name="modelState"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> HandleRegisterValidation(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        /// this method is called just before the attempt to create the user by usermanager
        /// if you need to update native user properties this is the time to do it.
        /// I added this method because I have found that with EFCore since the dbcontext is scoped per request
        /// it is only expected that SaveChanges will be called once during the request lifetime
        /// so updating user properties and saving again in HandleRegisterPostSuccess doesn't always work.
        /// Strangely it works for some properties but not others, ie I found that I it failed to update AuthorBio
        /// though it did update firstname,lastname, and dob. Updating the user model here make sure the properties are set before the
        /// call to create the user so it allows participating in the one SaveChanges that happens when the user is created and 
        /// this works as expected for all properties of SiteUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task ProcessUserBeforeCreate(ISiteUser user, HttpContext httpContext);


        /// <summary>
        /// A method that will be invoked fromm the AccountController Register POST action method on successful registration.
        /// This is the right place to persist any custom data related to the newly registered user.
        /// The User property of the passed in loginResult should have an IUserContext object but you should do a null check.
        /// You can get the userid to tag your custon data as well as the siteid from ISiteContext.
        /// Success doesn't always mean the user is authenticated at this point, there can be other site rule that need to be met before the user is allowed to login,
        /// such as email verification, or account approval needed.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="viewModel"></param>
        /// <param name="httpContext"></param>
        /// <param name="loginResult"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleRegisterPostSuccess(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            UserLoginResult loginResult,
            CancellationToken cancellationToken = default(CancellationToken)
            );

    }

    /// <summary>
    /// we inject the NoRegistrationCustomization by default which doesn't do anything.
    /// If you inject your own implementation as scoped it will replace this one.
    /// </summary>
    public class NoRegistrationCustomization : IHandleCustomRegistration
    {
        public Task<string> GetRegisterViewName(ISiteContext site, HttpContext httpContext)
        {
            return Task.FromResult("Register"); // this is just returning the default view name.
        }

        public Task HandleRegisterGet(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(0);
        }

        public Task<bool> HandleRegisterValidation(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            ViewDataDictionary viewData,
            ModelStateDictionary modelState,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(true);
        }

        public Task ProcessUserBeforeCreate(ISiteUser user, HttpContext httpContext)
        {
            return Task.FromResult(0);
        }

        public Task HandleRegisterPostSuccess(
            ISiteContext site,
            RegisterViewModel viewModel,
            HttpContext httpContext,
            UserLoginResult loginResult,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            return Task.FromResult(0);
        }

    }
}
