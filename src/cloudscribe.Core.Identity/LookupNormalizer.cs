// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:				    2014-07-18
// Last Modified:		    2019-09-01
// 

using Microsoft.AspNetCore.Identity;

namespace cloudscribe.Core.Identity
{
    /// <summary>
    /// the EF implementation of stores and ojects such as user or role have a separate field for NormalizedUserName or NormalizaedName
    /// etc which they use for lookup and store as upperinvariant
    /// actually Identity namespace for some reason has the interface and implementation to upper case things
    /// we have loweredemail field on users, not sure we want to add more fields and change how things are looked up
    /// since it has been working fine
    /// so implemented these to use when we don't want to change the value for lookup purposes
    /// or if we want to lookup by our existing loweredemail
    /// </summary>
    public class UseOriginalLookupNormalizer :ILookupNormalizer
    {

        public string Normalize(string key)
        {
            return key;
        }

        public string NormalizeEmail(string email)
        {
            return email;
        }

        public string NormalizeName(string name)
        {
            return name;
        }
    }

    public class LowerInvariantLookupNormalizer : ILookupNormalizer
    {
        /// <summary>
        /// Returns a normalized representation of the specified <paramref name="key"/>
        /// by converting keys to their lower cased invariant culture representation.
        /// </summary>
        /// <param name="key">The key to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key"/>.</returns>
        public virtual string Normalize(string key)
        {
            if (key == null)
            {
                return null;
            }
            //return key.Normalize().ToLowerInvariant();
            return key.ToLowerInvariant();
        }

        public string NormalizeEmail(string email)
        {
            if (email == null) return email;

            return email.ToLowerInvariant();
        }

        public string NormalizeName(string name)
        {
            if (name == null) return name;

            return name.ToLowerInvariant();
        }
    }
}
