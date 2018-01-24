// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2017-07-10
// Last Modified:			2017-07-10
//

using cloudscribe.Core.Identity;
using cloudscribe.Core.Models;
using cloudscribe.Kvp.Models;
using cloudscribe.UserProperties.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.UserProperties.Kvp
{
    public class UserPropertyService : IUserPropertyService
    {
        public UserPropertyService(
            SiteUserManager<SiteUser> userManager,
            IKvpStorageService kvpStorage
            )
        {
            _userManager = userManager;
            _kvpStorage = kvpStorage;
        }

        private SiteUserManager<SiteUser> _userManager;
        private IKvpStorageService _kvpStorage;

        public async Task<SiteUser> GetUser(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public bool IsNativeUserProperty(string key)
        {
            switch(key)
            {
                case "FirstName":
                case "LastName":
                case "DateOfBirth":
                case "AuthorBio":
                case "Signature":
                case "Gender":
                case "AvatarUrl":
                case "WebSiteUrl":

                    return true;
            }
            return false;
        }

        public bool HasAnyNativeProps(List<UserPropertyDefinition> props)
        {
            foreach (var p in props)
            {
                if (IsNativeUserProperty(p.Key)) { return true; }
            }

            return false;
        }

        public string GetNativeUserProperty(ISiteUser siteUser, string key)
        {
            switch (key)
            {
                case "FirstName":
                    return siteUser.FirstName;
                   
                case "LastName":
                    return siteUser.LastName;
                case "DateOfBirth":
                    if(siteUser.DateOfBirth.HasValue)
                    {
                        return siteUser.DateOfBirth.Value.ToString("d");
                    }
                    return null;
                     
                case "AuthorBio":
                    return siteUser.AuthorBio;
                case "Signature":
                    return siteUser.Signature;
                case "Gender":
                    return siteUser.Gender;
                case "AvatarUrl":
                    return siteUser.AvatarUrl;
                case "WebSiteUrl":
                    return siteUser.WebSiteUrl;


            }
            return null;
        }

        public void UpdateNativeUserProperty(ISiteUser siteUser, string key, string value)
        {
            switch (key)
            {
                case "FirstName":
                    siteUser.FirstName = value;
                    break;
                case "LastName":
                    siteUser.LastName = value;
                    break;
                case "DateOfBirth":
                    DateTime dob;
                    var dobParsed = DateTime.TryParse(value, out dob);
                    if (!dobParsed)
                    {
                        siteUser.DateOfBirth = dob.Date;
                    }

                    break;
                case "AuthorBio":
                    siteUser.AuthorBio = value;
                    break;
                case "Signature":
                    siteUser.Signature = value;
                    break;
                case "Gender":
                    siteUser.Gender = value;
                    break;
                case "AvatarUrl":
                    siteUser.AvatarUrl = value;
                    break;
                case "WebSiteUrl":
                    siteUser.WebSiteUrl = value;
                    break;


            }
            
        }

        //public async Task SaveUser(SiteUser siteUser)
        //{
        //    var result = await _userManager.UpdateAsync(siteUser);
        //    if (!result.Succeeded)
        //    {
        //        //TODO: log it
        //    }
        //}

        public async Task<List<UserProperty>> FetchByUser(string siteId, string userId)
        {
            var result = new List<UserProperty>();

            var kvpList = await _kvpStorage.FetchById(
                siteId, //projectId
                "*", // featureId
                siteId, //setId
                userId // subSetId
                ).ConfigureAwait(false);
            
            foreach (var kvp in kvpList)
            {
                var prop = new UserProperty();
                prop.CreatedUtc = kvp.CreatedUtc;
                prop.Key = kvp.Key;
                prop.ModifiedUtc = kvp.ModifiedUtc;
                prop.SiteId = kvp.SetId;
                prop.UserId = kvp.SubSetId;
                prop.Value = kvp.Value;

                result.Add(prop);
            }
            
            return result;
        }

        public async Task CreateOrUpdate(string siteId, string userId, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(siteId)) throw new ArgumentException("siteid must be provided");
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("userId must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");
            //if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value must be provided");

            var kvpList = await _kvpStorage.FetchById(
                siteId, //projectId
                "*", // featureId
                siteId, //setId
                userId // subSetId
                ).ConfigureAwait(false);

            var foundKvp = kvpList.Where(x => x.Key == key).FirstOrDefault();
            if(foundKvp != null)
            {
                foundKvp.Value = value;
                foundKvp.ModifiedUtc = DateTime.UtcNow;
                await _kvpStorage.Update(
                    siteId,
                    foundKvp).ConfigureAwait(false);
            }
            else
            {
                var kvp = new KvpItem();
                kvp.Key = key;
                kvp.Value = value;
                kvp.SetId = siteId;
                kvp.SubSetId = userId;

                await _kvpStorage.Create(
                    siteId,
                    kvp).ConfigureAwait(false);
            }
        }

        //public async Task<UserProperty> FetchByKey(string siteId, string userId, string key)
        //{
        //    var kvp = await _kvpStorage.FetchByKey(
        //        siteId, //projectid
        //        key,
        //        "*", // featureId
        //        siteId, // setId
        //        userId //subSetId
        //        ).ConfigureAwait(false);

        //    if (kvp == null) return null;

        //    var prop = new UserProperty();
        //    prop.CreatedUtc = kvp.CreatedUtc;
        //    prop.Key = kvp.Key;
        //    prop.ModifiedUtc = kvp.ModifiedUtc;
        //    prop.SiteId = kvp.SetId;
        //    prop.UserId = kvp.SubSetId;
        //    prop.Value = kvp.Value;

        //    return prop;
        //}
    }
}
