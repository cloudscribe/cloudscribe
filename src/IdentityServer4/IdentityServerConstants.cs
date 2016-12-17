// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


namespace IdentityServer4
{
    public static class IdentityServerConstants
    {
        public const string LocalIdentityProvider = "local";
        public const string DefaultCookieAuthenticationScheme = "idsrv";
        public const string SignoutScheme = "idsrv";
        public const string ExternalCookieAuthenticationScheme = "external";

        public static class ProtocolTypes
        {
            public const string OpenIdConnect = "oidc";
            public const string WsFederation = "wsfed";
            public const string Saml2p = "saml2p";
        }

        public static class ClaimValueTypes
        {
            public const string Json = "json";
        }

        public static class ParsedSecretTypes
        {
            public const string NoSecret = "NoSecret";
            public const string SharedSecret = "SharedSecret";
            public const string X509Certificate = "X509Certificate";
            public const string JwtBearer = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        }

        public static class SecretTypes
        {
            public const string SharedSecret = "SharedSecret";
            public const string X509CertificateThumbprint = "X509Thumbprint";
            public const string X509CertificateName = "X509Name";
            public const string X509CertificateBase64 = "X509CertificateBase64";
        }

        public static class ProfileDataCallers
        {
            public const string UserInfoEndpoint = "UserInfoEndpoint";
            public const string ClaimsProviderIdentityToken = "ClaimsProviderIdentityToken";
            public const string ClaimsProviderAccessToken = "ClaimsProviderAccessToken";
        }

        public static class ProfileIsActiveCallers
        {
            public const string AuthorizeEndpoint = "AuthorizeEndpoint";
            public const string IdentityTokenValidation = "IdentityTokenValidation";
            public const string AccessTokenValidation = "AccessTokenValidation";
            public const string ResourceOwnerValidation = "ResourceOwnerValidation";
            public const string RefreshTokenValidation = "RefreshTokenValidation";
            public const string AuthorizationCodeValidation = "AuthorizationCodeValidation";
        }

        public static class StandardScopes
        {
            /// <summary>REQUIRED. Informs the Authorization Server that the Client is making an OpenID Connect request. If the <c>openid</c> scope value is not present, the behavior is entirely unspecified.</summary>
            public const string OpenId = "openid";
            /// <summary>OPTIONAL. This scope value requests access to the End-User's default profile Claims, which are: <c>name</c>, <c>family_name</c>, <c>given_name</c>, <c>middle_name</c>, <c>nickname</c>, <c>preferred_username</c>, <c>profile</c>, <c>picture</c>, <c>website</c>, <c>gender</c>, <c>birthdate</c>, <c>zoneinfo</c>, <c>locale</c>, and <c>updated_at</c>.</summary>
            public const string Profile = "profile";
            /// <summary>OPTIONAL. This scope value requests access to the <c>email</c> and <c>email_verified</c> Claims.</summary>
            public const string Email = "email";
            /// <summary>OPTIONAL. This scope value requests access to the <c>address</c> Claim.</summary>
            public const string Address = "address";
            /// <summary>OPTIONAL. This scope value requests access to the <c>phone_number</c> and <c>phone_number_verified</c> Claims.</summary>
            public const string Phone = "phone";
            /// <summary>This scope value MUST NOT be used with the OpenID Connect Implicit Client Implementer's Guide 1.0. See the OpenID Connect Basic Client Implementer's Guide 1.0 (http://openid.net/specs/openid-connect-implicit-1_0.html#OpenID.Basic) for its usage in that subset of OpenID Connect.</summary>
            public const string OfflineAccess = "offline_access";
        }

    }
}