// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-12-03
// Last Modified:			2016-08-28
// 

using cloudscribe.Core.Models.Geography;
using System;
using System.Collections.Generic;

namespace cloudscribe.Core.Models
{
    public static class InitialData
    {
        public static SiteSettings BuildInitialSite()
        {
            var newSite = new SiteSettings();
            newSite.SiteName = "Sample Site";
            newSite.AliasId = "s1";
            newSite.IsServerAdminSite = true;
            newSite.Theme = "";
            newSite.AllowNewRegistration = true;
            newSite.AutoCreateLdapUserOnFirstLogin = true;
            newSite.LdapPort = 389;
            newSite.LdapRootDN = string.Empty;
            newSite.LdapServer = string.Empty;
            newSite.UseEmailForLogin = true;
            newSite.UseLdapAuth = false;
            newSite.RequireConfirmedEmail = false;
            newSite.RequiresQuestionAndAnswer = false;
            newSite.MaxInvalidPasswordAttempts = 10;
            newSite.MinRequiredPasswordLength = 7;

            return newSite;
        }

        public static SiteUser BuildInitialAdmin()
        {
            var adminUser = new SiteUser();
            adminUser.Email = "admin@admin.com";
            adminUser.NormalizedEmail = adminUser.Email.ToUpperInvariant();
            adminUser.DisplayName = "Admin";
            adminUser.UserName = "admin";
            adminUser.NormalizedUserName = adminUser.UserName.ToUpperInvariant();

            adminUser.EmailConfirmed = true;
            adminUser.AccountApproved = true;

            // clear text password will be hashed upon login
            // this format allows migrating from mojoportal
            adminUser.PasswordHash = "admin||0"; //pwd/salt/format 

            return adminUser;
        }

        public static SiteRole BuildAdminRole()
        {
            var role = new SiteRole();
            role.RoleName = "Administrators";
            role.NormalizedRoleName = role.RoleName.ToUpperInvariant();

            return role;
        }

        public static SiteRole BuildRoleAdminRole()
        {
            var role = new SiteRole();
            role.RoleName = "Role Administrators";
            role.NormalizedRoleName = role.RoleName.ToUpperInvariant();

            return role;
        }

        public static SiteRole BuildAuthenticatedRole()
        {
            var role = new SiteRole();
            role.RoleName = "Authenticated Users";
            role.NormalizedRoleName = role.RoleName.ToUpperInvariant();

            return role;
        }

        public static SiteRole BuildContentAdminsRole()
        {
            var role = new SiteRole();
            role.RoleName = "Content Administrators";
            role.NormalizedRoleName = role.RoleName.ToUpperInvariant();

            return role;
        }
        

        public static List<GeoCountry> BuildCountryList()
        {
            List<GeoCountry> list = new List<GeoCountry>();

            list.Add(new GeoCountry { Id = new Guid("844686BA-57C3-4C91-8B33-C1E1889A44C0"), Name = "Albania", ISOCode2 = "AL", ISOCode3 = "ALB" });
            list.Add(new GeoCountry { Id = new Guid("BEC3AF5B-D2D4-4DFB-ACA5-CF87059469D4"), Name = "Algerian", ISOCode2 = "DZ", ISOCode3 = "DZA" });
            list.Add(new GeoCountry { Id = new Guid("1D925A47-3902-462A-BA2E-C58E5CB24F2F"), Name = "American Samoa", ISOCode2 = "AS", ISOCode3 = "ASM" });
            list.Add(new GeoCountry { Id = new Guid("574E1B06-4332-4A1C-9B30-5DAF2CCE6B10"), Name = "Andorra", ISOCode2 = "AD", ISOCode3 = "AND" });
            list.Add(new GeoCountry { Id = new Guid("AAE223C8-6330-4641-B12B-F231866DE4C6"), Name = "Anguilla", ISOCode2 = "AI", ISOCode3 = "AIA" });
            list.Add(new GeoCountry { Id = new Guid("5DC77E2B-DF39-475B-99DA-C9756CABB5B6"), Name = "Anla", ISOCode2 = "AO", ISOCode3 = "A  " });
            list.Add(new GeoCountry { Id = new Guid("579FBEE3-0BE0-4884-B7C5-658C23C4E7D3"), Name = "Antarctica", ISOCode2 = "AQ", ISOCode3 = "ATA" });
            list.Add(new GeoCountry { Id = new Guid("67497E93-C793-4134-915E-E04F5ADAE5D0"), Name = "Antigua and Barbuda", ISOCode2 = "AG", ISOCode3 = "ATG" });
            list.Add(new GeoCountry { Id = new Guid("B5133B5B-1687-447A-B88A-EF21F7599EDA"), Name = "Argentina", ISOCode2 = "AR", ISOCode3 = "ARG" });
            list.Add(new GeoCountry { Id = new Guid("392616F8-1B24-489F-8600-BAE22EF478CC"), Name = "Armenia", ISOCode2 = "AM", ISOCode3 = "ARM" });
            list.Add(new GeoCountry { Id = new Guid("E1AA65E1-D524-48BA-91EF-39570B9984D7"), Name = "Aruba", ISOCode2 = "AW", ISOCode3 = "ABW" });
            list.Add(new GeoCountry { Id = new Guid("2EBCE3A9-660A-4C1D-AC8F-0E899B34A987"), Name = "Australia", ISOCode2 = "AU", ISOCode3 = "AUS" });
            list.Add(new GeoCountry { Id = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Austria", ISOCode2 = "AT", ISOCode3 = "AUT" });
            list.Add(new GeoCountry { Id = new Guid("78A78ABB-31D9-4E2A-AEA5-6744F27A6519"), Name = "Azerbaijan", ISOCode2 = "AZ", ISOCode3 = "AZE" });
            list.Add(new GeoCountry { Id = new Guid("B4A3405B-1293-4E98-9B11-777F666B25D4"), Name = "Bahamas", ISOCode2 = "BS", ISOCode3 = "BHS" });
            list.Add(new GeoCountry { Id = new Guid("C10D2E3A-AF21-4BAD-9B18-FBF3FB659EAE"), Name = "Bahrain", ISOCode2 = "BH", ISOCode3 = "BHR" });
            list.Add(new GeoCountry { Id = new Guid("5AAC5AA6-8BC0-4BE5-A4DE-76A5917DD2B2"), Name = "Bangladesh", ISOCode2 = "BD", ISOCode3 = "BGD" });
            list.Add(new GeoCountry { Id = new Guid("3664546F-14F2-4561-9B77-67E8BE6A9B1F"), Name = "Barbados", ISOCode2 = "BB", ISOCode3 = "BRB" });
            list.Add(new GeoCountry { Id = new Guid("D61F7A82-85C5-45E1-A23C-60EDAE497459"), Name = "Belarus", ISOCode2 = "BY", ISOCode3 = "BLR" });
            list.Add(new GeoCountry { Id = new Guid("72BBBB80-EA6C-43C9-8CCD-99D26290F560"), Name = "Belgium", ISOCode2 = "BE", ISOCode3 = "BEL" });
            list.Add(new GeoCountry { Id = new Guid("F5548AC2-958F-4B3D-8669-38B58735C517"), Name = "Belize", ISOCode2 = "BZ", ISOCode3 = "BLZ" });
            list.Add(new GeoCountry { Id = new Guid("2391213F-FCBF-479A-9AB9-AF1D6DEB9E11"), Name = "Benin", ISOCode2 = "BJ", ISOCode3 = "BEN" });
            list.Add(new GeoCountry { Id = new Guid("E82E9DC1-7D00-47C0-9476-10EAF259967D"), Name = "Bermuda", ISOCode2 = "BM", ISOCode3 = "BMU" });
            list.Add(new GeoCountry { Id = new Guid("C1EC594F-4B56-436D-AA28-CE3004DE2803"), Name = "Bhutan", ISOCode2 = "BT", ISOCode3 = "BTN" });
            list.Add(new GeoCountry { Id = new Guid("1E64910A-BCE3-402C-9035-9CB1F820B195"), Name = "Bolivia", ISOCode2 = "BO", ISOCode3 = "BOL" });
            list.Add(new GeoCountry { Id = new Guid("1A07C0B8-EB6D-4153-8CB1-BE6E31FEB566"), Name = "Bosnia and Herzewina", ISOCode2 = "BA", ISOCode3 = "BIH" });
            list.Add(new GeoCountry { Id = new Guid("1B8FBDE0-E709-4F7B-838D-B09DEF73DE8F"), Name = "Botswana", ISOCode2 = "BW", ISOCode3 = "BWA" });
            list.Add(new GeoCountry { Id = new Guid("C63D51D8-B319-4A48-A6F1-81671B28EF07"), Name = "Bouvet Island", ISOCode2 = "BV", ISOCode3 = "BVT" });
            list.Add(new GeoCountry { Id = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Brazil", ISOCode2 = "BR", ISOCode3 = "BRA" });
            list.Add(new GeoCountry { Id = new Guid("3E57398A-0006-4E48-8CB4-F9F143DFCF22"), Name = "British Indian Ocean Territory", ISOCode2 = "IO", ISOCode3 = "IOT" });
            list.Add(new GeoCountry { Id = new Guid("E04ED9C1-FACE-4EE6-BADE-7E522C0D210E"), Name = "Brunei Darussalam", ISOCode2 = "BN", ISOCode3 = "BRN" });
            list.Add(new GeoCountry { Id = new Guid("FD70FE71-1429-4C6E-B399-90318ED9DDCB"), Name = "Bulgaria", ISOCode2 = "BG", ISOCode3 = "BGR" });
            list.Add(new GeoCountry { Id = new Guid("3220B426-8251-4F95-85C8-3F7821ECC932"), Name = "Burkina Faso", ISOCode2 = "BF", ISOCode3 = "BFA" });
            list.Add(new GeoCountry { Id = new Guid("D7A96DD1-66F4-49B4-9085-53A12FACAC98"), Name = "Burundi", ISOCode2 = "BI", ISOCode3 = "BDI" });
            list.Add(new GeoCountry { Id = new Guid("EAFEB25D-265A-4899-BE24-BB0F4BF64480"), Name = "Cambodia", ISOCode2 = "KH", ISOCode3 = "KHM" });
            list.Add(new GeoCountry { Id = new Guid("32EB5D85-1283-4586-BB16-B2B978B6537F"), Name = "Cameroon", ISOCode2 = "CM", ISOCode3 = "CMR" });
            list.Add(new GeoCountry { Id = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Canada", ISOCode2 = "CA", ISOCode3 = "CAN" });
            list.Add(new GeoCountry { Id = new Guid("7E83BA7D-1C8F-465C-87D3-9BD86256031A"), Name = "Cape Verde", ISOCode2 = "CV", ISOCode3 = "CPV" });
            list.Add(new GeoCountry { Id = new Guid("1D0DAE21-CD07-4022-B86A-7780C5EA0264"), Name = "Cayman Islands", ISOCode2 = "KY", ISOCode3 = "CYM" });
            list.Add(new GeoCountry { Id = new Guid("23BA8DCE-C784-4712-A6A0-0271F175D4E5"), Name = "Central African Republic", ISOCode2 = "CF", ISOCode3 = "CAF" });
            list.Add(new GeoCountry { Id = new Guid("2DD32741-D7E9-49C9-B3D3-B58C4A913E60"), Name = "Chad", ISOCode2 = "TD", ISOCode3 = "TCD" });
            list.Add(new GeoCountry { Id = new Guid("70E9EF51-B838-461B-A1D8-2B32EE49855B"), Name = "Chile", ISOCode2 = "CL", ISOCode3 = "CHL" });
            list.Add(new GeoCountry { Id = new Guid("C23969D4-E195-4E53-BF7E-D3D041184325"), Name = "China", ISOCode2 = "CN", ISOCode3 = "CHN" });
            list.Add(new GeoCountry { Id = new Guid("F9C72583-E1F8-4F13-BFB5-DDF68BCD656A"), Name = "Christmas Island", ISOCode2 = "CX", ISOCode3 = "CXR" });
            list.Add(new GeoCountry { Id = new Guid("4CE3DF16-4D00-4F4D-A5D6-675020FA117D"), Name = "Cocos (Keeling) Islands", ISOCode2 = "CC", ISOCode3 = "CCK" });
            list.Add(new GeoCountry { Id = new Guid("C7C9F73A-F4BE-4C59-9278-524D6069D9DC"), Name = "Colombia", ISOCode2 = "CO", ISOCode3 = "COL" });
            list.Add(new GeoCountry { Id = new Guid("386812D8-E983-4D3A-B7F0-1FA0BBE5919F"), Name = "Comoros", ISOCode2 = "KM", ISOCode3 = "COM" });
            list.Add(new GeoCountry { Id = new Guid("0B182EE0-0CC0-4844-9CF0-BA15F47682E8"), Name = "Con", ISOCode2 = "CG", ISOCode3 = "COG" });
            list.Add(new GeoCountry { Id = new Guid("3F677556-1C9C-4315-9CFC-210A54F1F41D"), Name = "Cook Islands", ISOCode2 = "CK", ISOCode3 = "COK" });
            list.Add(new GeoCountry { Id = new Guid("E691AC69-A14D-4CCA-86ED-82978614283E"), Name = "Costa Rica", ISOCode2 = "CR", ISOCode3 = "CRI" });
            list.Add(new GeoCountry { Id = new Guid("333ED823-0E19-4BCC-A74E-C6C66FE76834"), Name = "Cote D Ivoire", ISOCode2 = "CI", ISOCode3 = "CIV" });
            list.Add(new GeoCountry { Id = new Guid("F3B7F86F-3165-4430-B263-87E1222B5BB1"), Name = "Croatia", ISOCode2 = "HR", ISOCode3 = "HRV" });
            list.Add(new GeoCountry { Id = new Guid("F909C4C1-5FA9-4188-B848-ECD37E3DBF64"), Name = "Cuba", ISOCode2 = "CU", ISOCode3 = "CUB" });
            list.Add(new GeoCountry { Id = new Guid("9C035E40-A5DC-406B-A83A-559F940EB355"), Name = "Cyprus", ISOCode2 = "CY", ISOCode3 = "CYP" });
            list.Add(new GeoCountry { Id = new Guid("19F2DA98-FEFD-4B45-A260-8D9392C35A24"), Name = "Czech Republic", ISOCode2 = "CZ", ISOCode3 = "CZE" });
            list.Add(new GeoCountry { Id = new Guid("83C5561E-E4BE-40B0-AE56-28A371680AF8"), Name = "Denmark", ISOCode2 = "DK", ISOCode3 = "DNK" });
            list.Add(new GeoCountry { Id = new Guid("AA393972-1604-47D2-A533-81B41199CCF0"), Name = "Djibouti", ISOCode2 = "DJ", ISOCode3 = "DJI" });
            list.Add(new GeoCountry { Id = new Guid("DD3D7458-318B-4C6B-891C-766A6D7AC265"), Name = "Dominica", ISOCode2 = "DM", ISOCode3 = "DMA" });
            list.Add(new GeoCountry { Id = new Guid("66C2BFB0-11C9-4191-8E91-1A0314726CC6"), Name = "Dominican Republic", ISOCode2 = "DO", ISOCode3 = "DOM" });
            list.Add(new GeoCountry { Id = new Guid("468DCA85-484A-4529-8753-B26DBC316A71"), Name = "East Timor", ISOCode2 = "TP", ISOCode3 = "TMP" });
            list.Add(new GeoCountry { Id = new Guid("7B3B0B11-B3CF-4E69-B4C2-C414BB7BD78D"), Name = "Ecuador", ISOCode2 = "EC", ISOCode3 = "ECU" });
            list.Add(new GeoCountry { Id = new Guid("66D1D01B-A1A5-4634-9C15-4CD382A44147"), Name = "Egypt", ISOCode2 = "EG", ISOCode3 = "EGY" });
            list.Add(new GeoCountry { Id = new Guid("5EDC9DDF-242C-4533-9C38-CBF41709EF60"), Name = "El Salvador", ISOCode2 = "SV", ISOCode3 = "SLV" });
            list.Add(new GeoCountry { Id = new Guid("05B98DDC-F36B-4DAF-9459-0717FDE9B38E"), Name = "Equatorial Guinea", ISOCode2 = "GQ", ISOCode3 = "GNQ" });
            list.Add(new GeoCountry { Id = new Guid("10D4D58E-D0C2-4A4E-8FDD-B99D68C0BD22"), Name = "Eritrea", ISOCode2 = "ER", ISOCode3 = "ERI" });
            list.Add(new GeoCountry { Id = new Guid("167838F1-3FDD-4FB6-9268-4BEAFEECEA4B"), Name = "Estonia", ISOCode2 = "EE", ISOCode3 = "EST" });
            list.Add(new GeoCountry { Id = new Guid("4CC52CE2-0A6C-4564-8FE6-2EEB347A9429"), Name = "Ethiopia", ISOCode2 = "ET", ISOCode3 = "ETH" });
            list.Add(new GeoCountry { Id = new Guid("63AECD7A-9B3F-4732-BF8C-1702AD3A49DC"), Name = "Falkland Islands (Malvinas)", ISOCode2 = "FK", ISOCode3 = "FLK" });
            list.Add(new GeoCountry { Id = new Guid("EADABF25-0FA0-4E8E-AA1E-26D02EB70653"), Name = "Faroe Islands", ISOCode2 = "FO", ISOCode3 = "FRO" });
            list.Add(new GeoCountry { Id = new Guid("9151AAF1-A75B-4A2C-BF2B-C823E2586DB2"), Name = "Fiji", ISOCode2 = "FJ", ISOCode3 = "FJI" });
            list.Add(new GeoCountry { Id = new Guid("EC0D252B-7BA6-4AC4-AD41-6158A10E9CCF"), Name = "Finland", ISOCode2 = "FI", ISOCode3 = "FIN" });
            list.Add(new GeoCountry { Id = new Guid("4F660961-0AFF-4539-9C0B-3BB2662B7A99"), Name = "France", ISOCode2 = "FR", ISOCode3 = "FRA" });
            list.Add(new GeoCountry { Id = new Guid("9CA410F0-EB75-4105-90A1-09FC8D2873B8"), Name = "France, Metropolitan", ISOCode2 = "FX", ISOCode3 = "FXX" });
            list.Add(new GeoCountry { Id = new Guid("9D2C4779-1608-4D2A-B157-F5C4BB334EED"), Name = "French Guiana", ISOCode2 = "GF", ISOCode3 = "GUF" });
            list.Add(new GeoCountry { Id = new Guid("77BBFB67-9D1D-41F9-8626-B327AA90A584"), Name = "French Polynesia", ISOCode2 = "PF", ISOCode3 = "PYF" });
            list.Add(new GeoCountry { Id = new Guid("E399424A-A86A-4C61-B92B-450106831B4C"), Name = "French Southern Territories", ISOCode2 = "TF", ISOCode3 = "ATF" });
            list.Add(new GeoCountry { Id = new Guid("3A733002-9223-4BD7-B2A9-62FA359C4CBD"), Name = "Gabon", ISOCode2 = "GA", ISOCode3 = "GAB" });
            list.Add(new GeoCountry { Id = new Guid("8F5124FA-CB2A-4CC9-87BB-BC155DC9791A"), Name = "Gambia", ISOCode2 = "GM", ISOCode3 = "GMB" });
            list.Add(new GeoCountry { Id = new Guid("CD85035D-3901-4D07-A254-90750CD57C90"), Name = "Georgia", ISOCode2 = "GE", ISOCode3 = "GEO" });
            list.Add(new GeoCountry { Id = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Germany", ISOCode2 = "DE", ISOCode3 = "DEU" });
            list.Add(new GeoCountry { Id = new Guid("FDC8539A-82A7-4D29-BD5C-67FB9769A5AC"), Name = "Ghana", ISOCode2 = "GH", ISOCode3 = "GHA" });
            list.Add(new GeoCountry { Id = new Guid("B50F640F-0AE9-4D63-ACB2-2ABD94B6271B"), Name = "Gibraltar", ISOCode2 = "GI", ISOCode3 = "GIB" });
            list.Add(new GeoCountry { Id = new Guid("B47E2EEC-62A0-440C-9F20-AF9C5C75D57B"), Name = "Greece", ISOCode2 = "GR", ISOCode3 = "GRC" });
            list.Add(new GeoCountry { Id = new Guid("4DCD6ECF-AF6C-4C76-95DB-A0EFAC63F3DE"), Name = "Greenland", ISOCode2 = "GL", ISOCode3 = "GRL" });
            list.Add(new GeoCountry { Id = new Guid("1A6A2DB1-D162-4FEA-B660-B88FC25F558E"), Name = "Grenada", ISOCode2 = "GD", ISOCode3 = "GRD" });
            list.Add(new GeoCountry { Id = new Guid("F63CE832-2C8D-4C43-A4D8-134FC4311098"), Name = "Guadeloupe", ISOCode2 = "GP", ISOCode3 = "GLP" });
            list.Add(new GeoCountry { Id = new Guid("48CD745A-4C47-4282-B60A-CB4B4639C6EE"), Name = "Guam", ISOCode2 = "GU", ISOCode3 = "GUM" });
            list.Add(new GeoCountry { Id = new Guid("44577B6A-6918-4508-ADE4-B6C2ADB25000"), Name = "Guatemala", ISOCode2 = "GT", ISOCode3 = "GTM" });
            list.Add(new GeoCountry { Id = new Guid("4448E7B7-4E4D-4F19-B64D-E649D0F76CC1"), Name = "Guinea", ISOCode2 = "GN", ISOCode3 = "GIN" });
            list.Add(new GeoCountry { Id = new Guid("880F29A2-E51C-4016-AB18-CA09275673C3"), Name = "Guinea-bissau", ISOCode2 = "GW", ISOCode3 = "GNB" });
            list.Add(new GeoCountry { Id = new Guid("92A52065-32B0-42C6-A0AA-E8B8A341F79C"), Name = "Guyana", ISOCode2 = "GY", ISOCode3 = "GUY" });
            list.Add(new GeoCountry { Id = new Guid("CE737F29-05A4-4A9A-B5DC-F1876F409334"), Name = "Haiti", ISOCode2 = "HT", ISOCode3 = "HTI" });
            list.Add(new GeoCountry { Id = new Guid("BDB52E20-8F5C-4A6C-A8D5-2B4DC060CC13"), Name = "Heard and Mc Donald Islands", ISOCode2 = "HM", ISOCode3 = "HMD" });
            list.Add(new GeoCountry { Id = new Guid("6599493D-EAD6-41CE-AE9C-2A47EA74C1A8"), Name = "Honduras", ISOCode2 = "HN", ISOCode3 = "HND" });
            list.Add(new GeoCountry { Id = new Guid("B5946EA8-B8A8-45B9-827D-86FA13E034CD"), Name = "Hong Kong", ISOCode2 = "HK", ISOCode3 = "HKG" });
            list.Add(new GeoCountry { Id = new Guid("61EF876B-9508-48E9-AFBF-2D4386C38127"), Name = "Hungary", ISOCode2 = "HU", ISOCode3 = "HUN" });
            list.Add(new GeoCountry { Id = new Guid("01CA292D-86CA-4FA5-9205-2B0A37E7353B"), Name = "Iceland", ISOCode2 = "IS", ISOCode3 = "ISL" });
            list.Add(new GeoCountry { Id = new Guid("C43B2A01-933B-4021-896F-FCD27F3820DA"), Name = "India", ISOCode2 = "IN", ISOCode3 = "IND" });
            list.Add(new GeoCountry { Id = new Guid("000DA5AD-296A-4698-A21B-7D9C23FEEA14"), Name = "Indonesia", ISOCode2 = "ID", ISOCode3 = "IDN" });
            list.Add(new GeoCountry { Id = new Guid("A566AC8D-4A81-4A11-9CFB-979517440CE2"), Name = "Iran (Islamic Republic of)", ISOCode2 = "IR", ISOCode3 = "IRN" });
            list.Add(new GeoCountry { Id = new Guid("EC4D278F-0D96-478F-B023-0FDC7520C56C"), Name = "Iraq", ISOCode2 = "IQ", ISOCode3 = "IRQ" });
            list.Add(new GeoCountry { Id = new Guid("7E11E0DC-0A4E-4DB9-9673-84600C8035C4"), Name = "Ireland", ISOCode2 = "IE", ISOCode3 = "IRL" });
            list.Add(new GeoCountry { Id = new Guid("FE0E585E-FC54-4FA2-80C0-6FBFE5397E8C"), Name = "Israel", ISOCode2 = "IL", ISOCode3 = "ISR" });
            list.Add(new GeoCountry { Id = new Guid("A642097B-CC0A-430D-9425-9F8385FC6AA4"), Name = "Italy", ISOCode2 = "IT", ISOCode3 = "ITA" });
            list.Add(new GeoCountry { Id = new Guid("9AB1EE28-B81F-4B89-AE6B-3C6E5322E269"), Name = "Jamaica", ISOCode2 = "JM", ISOCode3 = "JAM" });
            list.Add(new GeoCountry { Id = new Guid("F74A81FA-3D6A-415C-88FD-5458ED8C45C2"), Name = "Japan", ISOCode2 = "JP", ISOCode3 = "JPN" });
            list.Add(new GeoCountry { Id = new Guid("25ED463D-21F5-412C-9BDB-6D76073EA790"), Name = "Jordan", ISOCode2 = "JO", ISOCode3 = "JOR" });
            list.Add(new GeoCountry { Id = new Guid("931EE133-2B60-4B82-8889-7C9855CA030A"), Name = "Kazakhstan", ISOCode2 = "KZ", ISOCode3 = "KAZ" });
            list.Add(new GeoCountry { Id = new Guid("C47BF5EA-DFE4-4C9F-8BBC-067BD15FA6D2"), Name = "Kenya", ISOCode2 = "KE", ISOCode3 = "KEN" });
            list.Add(new GeoCountry { Id = new Guid("C1F503A3-C6B4-4EEE-9FEA-1F656F3B0825"), Name = "Kiribati", ISOCode2 = "KI", ISOCode3 = "KIR" });
            list.Add(new GeoCountry { Id = new Guid("48E5E925-6D98-4039-AF6E-36D676059B85"), Name = "Korea, Democratic Peoples Republic of", ISOCode2 = "KP", ISOCode3 = "PRK" });
            list.Add(new GeoCountry { Id = new Guid("267865B1-E8DA-432D-BE45-63933F18A40F"), Name = "Korea, Republic of", ISOCode2 = "KR", ISOCode3 = "KOR" });
            list.Add(new GeoCountry { Id = new Guid("C03527D6-1936-4FDB-AB72-93AE7CB571ED"), Name = "Kuwait", ISOCode2 = "KW", ISOCode3 = "KWT" });
            list.Add(new GeoCountry { Id = new Guid("0416E2FC-C902-4452-8DE9-29A2B453E685"), Name = "Kyrgyzstan", ISOCode2 = "KG", ISOCode3 = "KGZ" });
            list.Add(new GeoCountry { Id = new Guid("4E6D9507-9FB0-4290-80AF-E98AABACCEDB"), Name = "Lao Peoples Democratic Republic", ISOCode2 = "LA", ISOCode3 = "LAO" });
            list.Add(new GeoCountry { Id = new Guid("7B534A1E-E06D-4A2C-8EA6-85C128201834"), Name = "Latvia", ISOCode2 = "LV", ISOCode3 = "LVA" });
            list.Add(new GeoCountry { Id = new Guid("88592F8B-1D15-4AA0-9115-4A28B67E1753"), Name = "Lebanon", ISOCode2 = "LB", ISOCode3 = "LBN" });
            list.Add(new GeoCountry { Id = new Guid("776102B6-3D75-4570-8215-484367EA2A80"), Name = "Lesotho", ISOCode2 = "LS", ISOCode3 = "LSO" });
            list.Add(new GeoCountry { Id = new Guid("58C5C312-85D2-47A3-87A7-1549EC0CCD44"), Name = "Liberia", ISOCode2 = "LR", ISOCode3 = "LBR" });
            list.Add(new GeoCountry { Id = new Guid("28218639-6094-4AA2-AE88-9206630BB930"), Name = "Libyan Arab Jamahiriya", ISOCode2 = "LY", ISOCode3 = "LBY" });
            list.Add(new GeoCountry { Id = new Guid("B10C1EFC-5341-4EC4-BE12-A70DBB1C41CC"), Name = "Liechtenstein", ISOCode2 = "LI", ISOCode3 = "LIE" });
            list.Add(new GeoCountry { Id = new Guid("99C347F1-1427-4D41-BC12-945D38F92A94"), Name = "Lithuania", ISOCode2 = "LT", ISOCode3 = "LTU" });
            list.Add(new GeoCountry { Id = new Guid("A4F1D01A-EBFC-4BD3-9521-BE6D73F79FAC"), Name = "Luxembourg", ISOCode2 = "LU", ISOCode3 = "LUX" });
            list.Add(new GeoCountry { Id = new Guid("5F6DF4FF-EF4B-43D9-98F5-D66EF9D27C67"), Name = "Macau", ISOCode2 = "MO", ISOCode3 = "MAC" });
            list.Add(new GeoCountry { Id = new Guid("3C864692-824C-4593-A739-D1309D4CD75E"), Name = "Macedonia, The Former Yuslav Republic of", ISOCode2 = "MK", ISOCode3 = "MKD" });
            list.Add(new GeoCountry { Id = new Guid("90255D75-AF44-4B5D-BCFD-77CD27DCE782"), Name = "Madagascar", ISOCode2 = "MG", ISOCode3 = "MDG" });
            list.Add(new GeoCountry { Id = new Guid("045A6098-A4A5-457A-AEF0-6CC57CC4A813"), Name = "Malawi", ISOCode2 = "MW", ISOCode3 = "MWI" });
            list.Add(new GeoCountry { Id = new Guid("04724868-0448-48EF-840B-7D5DA12495EC"), Name = "Malaysia", ISOCode2 = "MY", ISOCode3 = "MYS" });
            list.Add(new GeoCountry { Id = new Guid("0055471A-7993-42A1-897C-E5DAF92E7C0E"), Name = "Maldives", ISOCode2 = "MV", ISOCode3 = "MDV" });
            list.Add(new GeoCountry { Id = new Guid("FBFF9784-D58C-4C86-A7F2-2F8CE68D10E7"), Name = "Mali", ISOCode2 = "ML", ISOCode3 = "MLI" });
            list.Add(new GeoCountry { Id = new Guid("90684E6E-2B34-4F18-BBD1-F610F76179B7"), Name = "Malta", ISOCode2 = "MT", ISOCode3 = "MLT" });
            list.Add(new GeoCountry { Id = new Guid("056F6ED6-8F6D-4366-A755-2D6B8FB2B7AD"), Name = "Marshall Islands", ISOCode2 = "MH", ISOCode3 = "MHL" });
            list.Add(new GeoCountry { Id = new Guid("DAC6366F-295F-4DDC-B08C-5A521C70774D"), Name = "Martinique", ISOCode2 = "MQ", ISOCode3 = "MTQ" });
            list.Add(new GeoCountry { Id = new Guid("E8F03EAA-DDD2-4FF2-8B66-DA69FF074CCD"), Name = "Mauritania", ISOCode2 = "MR", ISOCode3 = "MRT" });
            list.Add(new GeoCountry { Id = new Guid("66D7E3D5-F89C-42C5-82D5-9E6869AB9775"), Name = "Mauritius", ISOCode2 = "MU", ISOCode3 = "MUS" });
            list.Add(new GeoCountry { Id = new Guid("65223343-756C-4083-A20F-CF3CF98EFBDC"), Name = "Mayotte", ISOCode2 = "YT", ISOCode3 = "MYT" });
            list.Add(new GeoCountry { Id = new Guid("10FDC2BB-F3A6-4A9D-A6E9-F4C781E8DBFF"), Name = "Mexico", ISOCode2 = "MX", ISOCode3 = "MEX" });
            list.Add(new GeoCountry { Id = new Guid("F321B513-8164-4882-BAE0-F3657A1A98FB"), Name = "Micronesia, Federated States of", ISOCode2 = "FM", ISOCode3 = "FSM" });
            list.Add(new GeoCountry { Id = new Guid("14962ADD-4536-4854-BEA3-A5A904932E1C"), Name = "Moldova, Republic of", ISOCode2 = "MD", ISOCode3 = "MDA" });
            list.Add(new GeoCountry { Id = new Guid("AE094B3E-A8B8-4E29-9853-3BD464EFD247"), Name = "Monaco", ISOCode2 = "MC", ISOCode3 = "MCO" });
            list.Add(new GeoCountry { Id = new Guid("F3418C04-E3A8-4826-A41F-DCDBB5E4613E"), Name = "Monlia", ISOCode2 = "MN", ISOCode3 = "MNG" });
            list.Add(new GeoCountry { Id = new Guid("18160966-4EEB-4C6B-A526-5022042FE1E4"), Name = "Montserrat", ISOCode2 = "MS", ISOCode3 = "MSR" });
            list.Add(new GeoCountry { Id = new Guid("1C7FF578-F079-4B5B-9993-2E0253B8CC14"), Name = "Morocco", ISOCode2 = "MA", ISOCode3 = "MAR" });
            list.Add(new GeoCountry { Id = new Guid("B32A6FE3-F534-4C42-BD2D-8E2307476BA2"), Name = "Mozambique", ISOCode2 = "MZ", ISOCode3 = "MOZ" });
            list.Add(new GeoCountry { Id = new Guid("1583045C-5A80-4850-AC32-F177956FBD6A"), Name = "Myanmar", ISOCode2 = "MM", ISOCode3 = "MMR" });
            list.Add(new GeoCountry { Id = new Guid("70A106CA-3A82-4E37-AEA3-4A0BF8D50AFA"), Name = "Namibia", ISOCode2 = "NA", ISOCode3 = "NAM" });
            list.Add(new GeoCountry { Id = new Guid("B85AA3D6-D923-438C-AAD7-2063F6BFBD3C"), Name = "Nauru", ISOCode2 = "NR", ISOCode3 = "NRU" });
            list.Add(new GeoCountry { Id = new Guid("63404C30-266D-47B6-BEDA-FD252283E4E5"), Name = "Nepal", ISOCode2 = "NP", ISOCode3 = "NPL" });
            list.Add(new GeoCountry { Id = new Guid("07E1DE2F-B11E-4F3B-A342-964F72D24371"), Name = "Netherlands", ISOCode2 = "NL", ISOCode3 = "NLD" });
            list.Add(new GeoCountry { Id = new Guid("8C9D27F2-FE77-4653-9696-B046D6536BFA"), Name = "Netherlands Antilles", ISOCode2 = "AN", ISOCode3 = "ANT" });
            list.Add(new GeoCountry { Id = new Guid("74DFB95B-515D-4561-938D-169AC3782280"), Name = "New Caledonia", ISOCode2 = "NC", ISOCode3 = "NCL" });
            list.Add(new GeoCountry { Id = new Guid("7376C282-B5A3-4898-A342-C45F1C18B609"), Name = "New Zealand", ISOCode2 = "NZ", ISOCode3 = "NZL" });
            list.Add(new GeoCountry { Id = new Guid("5C3D7F0E-1900-4D73-ACF6-69459D70D616"), Name = "Nicaragua", ISOCode2 = "NI", ISOCode3 = "NIC" });
            list.Add(new GeoCountry { Id = new Guid("EB692475-F7AF-402F-BB0D-CD420F670B88"), Name = "Niger", ISOCode2 = "NE", ISOCode3 = "NER" });
            list.Add(new GeoCountry { Id = new Guid("52316192-6328-4E45-A39C-37FC96CAD138"), Name = "Nigeria", ISOCode2 = "NG", ISOCode3 = "NGA" });
            list.Add(new GeoCountry { Id = new Guid("96DBB697-3D7E-49BF-AC9B-0EA5CC014A6F"), Name = "Niue", ISOCode2 = "NU", ISOCode3 = "NIU" });
            list.Add(new GeoCountry { Id = new Guid("4EB5BCBE-13AA-45F0-AFDF-77B379347509"), Name = "Norfolk Island", ISOCode2 = "NF", ISOCode3 = "NFK" });
            list.Add(new GeoCountry { Id = new Guid("B225D445-6884-4232-97E4-B33499982104"), Name = "Northern Mariana Islands", ISOCode2 = "MP", ISOCode3 = "MNP" });
            list.Add(new GeoCountry { Id = new Guid("E55C6A3A-A5E9-4575-B24F-6DA0FD4115CD"), Name = "Norway", ISOCode2 = "NO", ISOCode3 = "NOR" });
            list.Add(new GeoCountry { Id = new Guid("B0FC7899-9C6F-4B80-838F-692A7A0AA83B"), Name = "Oman", ISOCode2 = "OM", ISOCode3 = "OMN" });
            list.Add(new GeoCountry { Id = new Guid("C046CA0B-6DD9-459C-BF76-BD024363AAAC"), Name = "Pakistan", ISOCode2 = "PK", ISOCode3 = "PAK" });
            list.Add(new GeoCountry { Id = new Guid("356D4B6E-9CCB-4DC6-9C82-837433178275"), Name = "Palau", ISOCode2 = "PW", ISOCode3 = "PLW" });
            list.Add(new GeoCountry { Id = new Guid("2AFE5A06-2692-4B96-A385-F299E469D196"), Name = "Panama", ISOCode2 = "PA", ISOCode3 = "PAN" });
            list.Add(new GeoCountry { Id = new Guid("66F06C44-26FF-4015-B0CE-D241A39DEF8B"), Name = "Papua New Guinea", ISOCode2 = "PG", ISOCode3 = "PNG" });
            list.Add(new GeoCountry { Id = new Guid("7C2C1E29-9E58-45EB-B512-5894496CD4DD"), Name = "Paraguay", ISOCode2 = "PY", ISOCode3 = "PRY" });
            list.Add(new GeoCountry { Id = new Guid("085D9357-416B-48D6-8C9E-EC3E9E2582D0"), Name = "Peru", ISOCode2 = "PE", ISOCode3 = "PER" });
            list.Add(new GeoCountry { Id = new Guid("9DCF0A16-DB7F-4B63-BAD7-30F80BCD9901"), Name = "Philippines", ISOCode2 = "PH", ISOCode3 = "PHL" });
            list.Add(new GeoCountry { Id = new Guid("216D38D9-5EEB-42B7-8D2D-0757409DC5FB"), Name = "Pitcairn", ISOCode2 = "PN", ISOCode3 = "PCN" });
            list.Add(new GeoCountry { Id = new Guid("54D227B4-1F3E-4F20-B16C-6428B77F5252"), Name = "Poland", ISOCode2 = "PL", ISOCode3 = "POL" });
            list.Add(new GeoCountry { Id = new Guid("FA26AE74-5404-4AAF-BD54-9B78266CCF03"), Name = "Portugal", ISOCode2 = "PT", ISOCode3 = "PRT" });
            list.Add(new GeoCountry { Id = new Guid("6717BE36-81C1-4DF3-A6F8-0F5EEF45CEC9"), Name = "Puerto Rico", ISOCode2 = "PR", ISOCode3 = "PRI" });
            list.Add(new GeoCountry { Id = new Guid("7756AA70-F22A-4F42-B8F4-E56CA9746064"), Name = "Qatar", ISOCode2 = "QA", ISOCode3 = "QAT" });
            list.Add(new GeoCountry { Id = new Guid("7F2E9D46-F5DB-48BF-8E07-D6D12E77D857"), Name = "Reunion", ISOCode2 = "RE", ISOCode3 = "REU" });
            list.Add(new GeoCountry { Id = new Guid("666699CD-7460-44B1-AFA9-ADC363778FF4"), Name = "Romania", ISOCode2 = "RO", ISOCode3 = "ROM" });
            list.Add(new GeoCountry { Id = new Guid("9F9AC0E3-F689-4E98-B1BB-0F5F01F20FAD"), Name = "Russian Federation", ISOCode2 = "RU", ISOCode3 = "RUS" });
            list.Add(new GeoCountry { Id = new Guid("7C0BA316-C6D9-48DC-919E-76E0EE0CF0FB"), Name = "Rwanda", ISOCode2 = "RW", ISOCode3 = "RWA" });
            list.Add(new GeoCountry { Id = new Guid("BF3B8CD7-679E-4546-81FC-85652653FE8F"), Name = "Saint Kitts and Nevis", ISOCode2 = "KN", ISOCode3 = "KNA" });
            list.Add(new GeoCountry { Id = new Guid("3D3A06A0-0853-4D01-B273-AF7B7CD7002C"), Name = "Saint Lucia", ISOCode2 = "LC", ISOCode3 = "LCA" });
            list.Add(new GeoCountry { Id = new Guid("20A15881-215B-4C4C-9512-80E55ABBB5BA"), Name = "Saint Vincent and the Grenadines", ISOCode2 = "VC", ISOCode3 = "VCT" });
            list.Add(new GeoCountry { Id = new Guid("24045513-0CD8-4FB9-9CF6-78BF717F6A7E"), Name = "Samoa", ISOCode2 = "WS", ISOCode3 = "WSM" });
            list.Add(new GeoCountry { Id = new Guid("972B8208-C88D-47BB-9E79-1574FAB34DFB"), Name = "San Marino", ISOCode2 = "SM", ISOCode3 = "SMR" });
            list.Add(new GeoCountry { Id = new Guid("AEA2F438-77BC-43F5-84FC-C781141A1D47"), Name = "Sao Tome and Principe", ISOCode2 = "ST", ISOCode3 = "STP" });
            list.Add(new GeoCountry { Id = new Guid("B14E1447-0BCA-4DD5-87E1-60C0B5D2988B"), Name = "Saudi Arabia", ISOCode2 = "SA", ISOCode3 = "SAU" });
            list.Add(new GeoCountry { Id = new Guid("6F101294-0433-492B-99F7-D59105A9970B"), Name = "Senegal", ISOCode2 = "SN", ISOCode3 = "SEN" });
            list.Add(new GeoCountry { Id = new Guid("36F89C06-1509-42D2-AEA6-7B4CE3BBC4F5"), Name = "Seychelles", ISOCode2 = "SC", ISOCode3 = "SYC" });
            list.Add(new GeoCountry { Id = new Guid("27A6A985-3A89-4309-AC40-D1F0A94646CE"), Name = "Sierra Leone", ISOCode2 = "SL", ISOCode3 = "SLE" });
            list.Add(new GeoCountry { Id = new Guid("0BD0E1A0-EA93-4883-B0A0-9F3C8668C68C"), Name = "Singapore", ISOCode2 = "SG", ISOCode3 = "SGP" });
            list.Add(new GeoCountry { Id = new Guid("A141AB0D-7E2C-48B1-9963-BA8685BCDFE3"), Name = "Slovakia (Slovak Republic)", ISOCode2 = "SK", ISOCode3 = "SVK" });
            list.Add(new GeoCountry { Id = new Guid("3FBD7371-510A-45B4-813A-88373D19A5A4"), Name = "Slovenia", ISOCode2 = "SI", ISOCode3 = "SVN" });
            list.Add(new GeoCountry { Id = new Guid("8AF11A89-1487-4B21-AABF-6AF57EAD8474"), Name = "Solomon Islands", ISOCode2 = "SB", ISOCode3 = "SLB" });
            list.Add(new GeoCountry { Id = new Guid("4DBE5363-AAD6-4019-B445-472D6E1E49BD"), Name = "Somalia", ISOCode2 = "SO", ISOCode3 = "SOM" });
            list.Add(new GeoCountry { Id = new Guid("73355D89-317A-43A5-8EBB-FA60DD738C5B"), Name = "South Africa", ISOCode2 = "ZA", ISOCode3 = "ZAF" });
            list.Add(new GeoCountry { Id = new Guid("CDA35E7B-29B0-4D34-B925-BF753D16AF7E"), Name = "South Georgia and the South Sandwich Islands", ISOCode2 = "GS", ISOCode3 = "SGS" });
            list.Add(new GeoCountry { Id = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Spain", ISOCode2 = "ES", ISOCode3 = "ESP" });
            list.Add(new GeoCountry { Id = new Guid("278AB63A-9C7E-4CAD-9C99-984F8810D151"), Name = "Sri Lanka", ISOCode2 = "LK", ISOCode3 = "LKA" });
            list.Add(new GeoCountry { Id = new Guid("0589489D-A413-47C6-A90A-600520A8C52D"), Name = "St. Helena", ISOCode2 = "SH", ISOCode3 = "SHN" });
            list.Add(new GeoCountry { Id = new Guid("F6E6E602-468A-4DD7-ACE4-3DA5FEFC165A"), Name = "St. Pierre and Miquelon", ISOCode2 = "PM", ISOCode3 = "SPM" });
            list.Add(new GeoCountry { Id = new Guid("061E11A1-33A2-42F0-8F8D-27E65FC47076"), Name = "Sudan", ISOCode2 = "SD", ISOCode3 = "SDN" });
            list.Add(new GeoCountry { Id = new Guid("F2F258D7-B650-45F9-A0E1-58687C08F4E4"), Name = "Suriname", ISOCode2 = "SR", ISOCode3 = "SUR" });
            list.Add(new GeoCountry { Id = new Guid("73FBC893-331D-4E67-9753-AB988AC005C7"), Name = "Svalbard and Jan Mayen Islands", ISOCode2 = "SJ", ISOCode3 = "SJM" });
            list.Add(new GeoCountry { Id = new Guid("171A3E3E-CC78-4D4A-93EE-ACE870DCB4C4"), Name = "Swaziland", ISOCode2 = "SZ", ISOCode3 = "SWZ" });
            list.Add(new GeoCountry { Id = new Guid("2BAD76B2-20F3-4568-96BB-D60C39CFEC37"), Name = "Sweden", ISOCode2 = "SE", ISOCode3 = "SWE" });
            list.Add(new GeoCountry { Id = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Switzerland", ISOCode2 = "CH", ISOCode3 = "CHE" });
            list.Add(new GeoCountry { Id = new Guid("C87D4CAE-84EE-4336-BC57-69C4EA33A6BC"), Name = "Syrian Arab Republic", ISOCode2 = "SY", ISOCode3 = "SYR" });
            list.Add(new GeoCountry { Id = new Guid("BD2C67C0-26A4-46D5-B58A-F26DCFA8F34B"), Name = "Taiwan", ISOCode2 = "TW", ISOCode3 = "TWN" });
            list.Add(new GeoCountry { Id = new Guid("77DCE560-3D53-4483-963E-37D5F72E219E"), Name = "Tajikistan", ISOCode2 = "TJ", ISOCode3 = "TJK" });
            list.Add(new GeoCountry { Id = new Guid("BAF7D87C-F09B-42CC-BECD-49C2B3426226"), Name = "Tanzania, United Republic of", ISOCode2 = "TZ", ISOCode3 = "TZA" });
            list.Add(new GeoCountry { Id = new Guid("612C5585-4E93-4F4F-9735-EC9AB7F2AAB9"), Name = "Thailand", ISOCode2 = "TH", ISOCode3 = "THA" });
            list.Add(new GeoCountry { Id = new Guid("0789D8A8-59D0-4D2F-8E26-5D917E55550C"), Name = "To", ISOCode2 = "TG", ISOCode3 = "T  " });
            list.Add(new GeoCountry { Id = new Guid("391EBAFD-7689-41E5-A785-DF6A3280528D"), Name = "Tokelau", ISOCode2 = "TK", ISOCode3 = "TKL" });
            list.Add(new GeoCountry { Id = new Guid("BBAAA327-F8CC-43AE-8B0E-FC054EEDA968"), Name = "Tonga", ISOCode2 = "TO", ISOCode3 = "TON" });
            list.Add(new GeoCountry { Id = new Guid("75F88974-01AC-47D7-BCEE-6CE1F0C0D0FC"), Name = "Trinidad and Toba", ISOCode2 = "TT", ISOCode3 = "TTO" });
            list.Add(new GeoCountry { Id = new Guid("68ABEFDB-27F4-4CB8-840C-AFEE8510C249"), Name = "Tunisia", ISOCode2 = "TN", ISOCode3 = "TUN" });
            list.Add(new GeoCountry { Id = new Guid("DA8E07C2-7B3D-46AF-BCC5-FEF0A68B11D1"), Name = "Turkey", ISOCode2 = "TR", ISOCode3 = "TUR" });
            list.Add(new GeoCountry { Id = new Guid("E6471BF0-4692-4B7A-B104-94B12B30A284"), Name = "Turkmenistan", ISOCode2 = "TM", ISOCode3 = "TKM" });
            list.Add(new GeoCountry { Id = new Guid("DA19B4E1-DFEA-43C9-AD8B-19E7036F0DA4"), Name = "Turks and Caicos Islands", ISOCode2 = "TC", ISOCode3 = "TCA" });
            list.Add(new GeoCountry { Id = new Guid("BB176526-F5C6-4871-9E75-CFEEF799AD48"), Name = "Tuvalu", ISOCode2 = "TV", ISOCode3 = "TUV" });
            list.Add(new GeoCountry { Id = new Guid("D9510667-AE8B-4066-811C-08C6834EFADF"), Name = "Uganda", ISOCode2 = "UG", ISOCode3 = "UGA" });
            list.Add(new GeoCountry { Id = new Guid("8FE152E5-B58C-4D3C-B143-358D5C54BA06"), Name = "Ukraine", ISOCode2 = "UA", ISOCode3 = "UKR" });
            list.Add(new GeoCountry { Id = new Guid("7FE147D0-FD91-4119-83AD-4E7EBCCDFD89"), Name = "United Arab Emirates", ISOCode2 = "AE", ISOCode3 = "ARE" });
            list.Add(new GeoCountry { Id = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "United Kingdom", ISOCode2 = "GB", ISOCode3 = "GBR" });
            list.Add(new GeoCountry { Id = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "United States", ISOCode2 = "US", ISOCode3 = "USA" });
            list.Add(new GeoCountry { Id = new Guid("8C982139-3609-48D3-B145-B5CEB484C414"), Name = "United States Minor Outlying Islands", ISOCode2 = "UM", ISOCode3 = "UMI" });
            list.Add(new GeoCountry { Id = new Guid("AEBD8175-FFFE-4EE2-B208-C0BBBD049664"), Name = "Uruguay", ISOCode2 = "UY", ISOCode3 = "URY" });
            list.Add(new GeoCountry { Id = new Guid("B5EE8DA7-5CC3-44F3-BD63-094CB93B4674"), Name = "Uzbekistan", ISOCode2 = "UZ", ISOCode3 = "UZB" });
            list.Add(new GeoCountry { Id = new Guid("B3732BD9-C3D6-4861-8DBE-EB2884557F34"), Name = "Vanuatu", ISOCode2 = "VU", ISOCode3 = "VUT" });
            list.Add(new GeoCountry { Id = new Guid("FBEA6604-4E57-46B6-A3F2-E5DE8514C7B0"), Name = "Vatican City State (Holy See)", ISOCode2 = "VA", ISOCode3 = "VAT" });
            list.Add(new GeoCountry { Id = new Guid("F015E45E-D93A-4D3A-A010-648CA65B47BE"), Name = "Venezuela", ISOCode2 = "VE", ISOCode3 = "VEN" });
            list.Add(new GeoCountry { Id = new Guid("99F791E7-7343-42E8-8C19-3C41068B5F8D"), Name = "Viet Nam", ISOCode2 = "VN", ISOCode3 = "VNM" });
            list.Add(new GeoCountry { Id = new Guid("D42BD5B7-9F7E-4CB2-A295-E37471CDB1C2"), Name = "Virgin Islands (British)", ISOCode2 = "VG", ISOCode3 = "VGB" });
            list.Add(new GeoCountry { Id = new Guid("0758CF79-94EB-4FA3-BD2C-8213034FB66C"), Name = "Virgin Islands (U.S.)", ISOCode2 = "VI", ISOCode3 = "VIR" });
            list.Add(new GeoCountry { Id = new Guid("E0274040-EF54-4B6E-B572-AF65A948D8C4"), Name = "Wallis and Futuna Islands", ISOCode2 = "WF", ISOCode3 = "WLF" });
            list.Add(new GeoCountry { Id = new Guid("3E747B23-543F-4AD0-80A9-5E421651F3B4"), Name = "Western Sahara", ISOCode2 = "EH", ISOCode3 = "ESH" });
            list.Add(new GeoCountry { Id = new Guid("9B5A87F8-F024-4B76-B230-95913E474B57"), Name = "Yemen", ISOCode2 = "YE", ISOCode3 = "YEM" });
            list.Add(new GeoCountry { Id = new Guid("31F9B05E-E21D-41D5-8753-7CDD3BFA917B"), Name = "Yuslavia", ISOCode2 = "YU", ISOCode3 = "YUG" });
            list.Add(new GeoCountry { Id = new Guid("0D074A4F-DF7F-49F3-8375-D35BDC934AE0"), Name = "Zaire", ISOCode2 = "ZR", ISOCode3 = "ZAR" });
            list.Add(new GeoCountry { Id = new Guid("F95A5BB1-59A5-4125-B803-A278B13B3D3B"), Name = "Zambia", ISOCode2 = "ZM", ISOCode3 = "ZMB" });
            list.Add(new GeoCountry { Id = new Guid("01261D1A-74D6-4E02-86C5-BED1A192F67D"), Name = "Zimbabwe", ISOCode2 = "ZW", ISOCode3 = "ZWE" });
            
            return list;

        }

        public static List<GeoZone> BuildStateList()
        {
            List<GeoZone> list = new List<GeoZone>();
            
            list.Add(new GeoZone { Id = new Guid("E314E827-9B3A-4282-954F-B3F46BA92F07"), CountryId = new Guid("844686BA-57C3-4C91-8B33-C1E1889A44C0"), Name = "Albywalbi Wacky Doo", Code = "AW" }); //Albania
            list.Add(new GeoZone { Id = new Guid("155DDC67-1E74-4791-995D-2EDDB0658293"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Burgenland", Code = "BL" }); //Austria
            list.Add(new GeoZone { Id = new Guid("A34DF017-1334-4F1F-AAB8-F650425F937D"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Kärnten", Code = "KN" }); //Austria
            list.Add(new GeoZone { Id = new Guid("BB607ECB-DF31-427B-88BB-4F53959B3E0C"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Niederösterreich", Code = "NO" }); //Austria
            list.Add(new GeoZone { Id = new Guid("F93295D1-7501-487D-93AD-6BD019E82CC2"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Oberösterreich", Code = "OO" }); //Austria
            list.Add(new GeoZone { Id = new Guid("A6ED9918-44C7-4975-B680-95B4ABCFB7AC"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Salzburg", Code = "SB" }); //Austria
            list.Add(new GeoZone { Id = new Guid("CA5C0C52-E8AE-4CCD-9A45-565E352C4E2B"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Steiermark", Code = "ST" }); //Austria
            list.Add(new GeoZone { Id = new Guid("3008A1B3-1188-4F4D-A2EF-B71B4F54233E"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Tirol", Code = "TI" }); //Austria
            list.Add(new GeoZone { Id = new Guid("962D2729-CC0C-4052-ABC9-C696307F3F26"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Voralberg", Code = "VB" }); //Austria
            list.Add(new GeoZone { Id = new Guid("B5FEB85C-2DC0-4776-BA5C-8C2D1B688E89"), CountryId = new Guid("06BB7816-9AD4-47DC-B1CD-6E206AFDFCCA"), Name = "Wien", Code = "WI" }); //Austria


            list.Add(new GeoZone { Id = new Guid("62202FA8-DB98-40F9-9A26-446AEE191CDD"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Acre", Code = "AC" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("D698A1B6-68D7-480E-8137-421C684F251D"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Alagoas", Code = "AL" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("1E1BA070-F44B-4DFB-8FC2-55C541F4943F"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Amapa", Code = "AP" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("91BF4254-F418-404A-8CB2-5449D498991E"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Amazonas", Code = "AM" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("5006FF54-AA63-4E57-8414-30D51598BE60"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Bahia", Code = "BA" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("66CC8A10-4DFB-4E8A-B5F0-B935D22A18F9"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Ceara", Code = "CE" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("9FB374C6-B87C-4096-A43C-D3D9FF2FD04C"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Distrito Federal", Code = "DF" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("294F2E9C-49D1-4094-B558-DD2D4219B0E9"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Espirito Santo", Code = "ES" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("4E0BC53A-62FE-4DFC-9D1D-8B928E40B22E"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Maranhao", Code = "MA" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("FCD4595B-4B67-4B73-84C6-29706A57AF38"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Mato Grosso", Code = "MT" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("2DF783C9-E527-4105-819E-181AF57E7CEC"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Mato Grosso Do Sul", Code = "MS" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("C5D128D8-353A-43DC-BA0A-D0C35E33DE17"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Minas Gerais", Code = "MG" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("E663AEF7-A697-4164-8CE4-141AC5CEF6A9"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Para", Code = "PA" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("FBE69225-8CAD-4E54-B4E5-03D6E404BC3F"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Paraiba", Code = "PB" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("DDB0CA67-8635-4F40-A01D-06CCB266EF56"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Parana", Code = "PR" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("B9F64887-ED6D-4DDC-A142-7EB8898CA47E"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Pernambuco", Code = "PE" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("CBC4121C-D62D-410C-B699-60B08B67711F"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Piaui", Code = "PI" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("D284266A-559D-42F3-A881-0136EA080C12"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Rio De Janeiro", Code = "RJ" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("F2E5FFCE-BF2A-4F21-9696-FD948C07D6AE"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Rio Grande Do Norte", Code = "RN" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("1D049867-DC28-4AE1-B8A6-D44AECB4AA0B"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Rio Grande Do Sul", Code = "RS" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("D256F9B7-8A33-4D04-9E19-95C12C967719"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Rondonia", Code = "RO" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("19B7CD11-15B7-48C0-918D-73FE64EAAE26"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Roraima", Code = "RR" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("79B41943-7A78-4CEC-857D-1FB89D34D301"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Santa Catarina", Code = "SC" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("8FAB7D36-B885-46CD-9DC8-41E40C8683C4"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Sao Paulo", Code = "SP" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("6CC5CF7E-DF8F-4C30-8B75-3C7D7750A4C0"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Sergipe", Code = "SE" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("1D996BA4-1906-44C3-9C51-399FD382D278"), CountryId = new Guid("13FAA99E-18F2-4E6F-B275-1E785B3383F3"), Name = "Tocantins", Code = "TO" }); //Brazil
            list.Add(new GeoZone { Id = new Guid("D85B7129-D009-4747-9748-B116739BA660"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Alberta", Code = "AB" }); //Canada
            list.Add(new GeoZone { Id = new Guid("2A20CF43-8D55-4732-B810-641886F2AED4"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "British Columbia", Code = "BC" }); //Canada
            list.Add(new GeoZone { Id = new Guid("0DF27C73-A612-491F-8B74-C4E384317FB8"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Manitoba", Code = "MB" }); //Canada
            list.Add(new GeoZone { Id = new Guid("B716403C-6B15-488B-9CD0-F60B1AA1BA41"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "New Brunswick", Code = "NB" }); //Canada
            list.Add(new GeoZone { Id = new Guid("4308F7F6-1F1D-4248-8995-3AF588C55976"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Newfoundland", Code = "NF" }); //Canada
            list.Add(new GeoZone { Id = new Guid("74532861-C62D-49D2-A8ED-E99F401EA768"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Northwest Territories", Code = "NT" }); //Canada
            list.Add(new GeoZone { Id = new Guid("29F5CE90-8999-4A8E-91A5-FCF67B4FD8AB"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Nova Scotia", Code = "NS" }); //Canada
            list.Add(new GeoZone { Id = new Guid("D20875CC-8572-453C-B5E0-53B49742DEBB"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Nunavut", Code = "NU" }); //Canada
            list.Add(new GeoZone { Id = new Guid("33CD3650-D80E-4157-B145-5D8D404628E4"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Ontario", Code = "ON" }); //Canada
            list.Add(new GeoZone { Id = new Guid("D4F8133E-5580-4A66-94DD-096D295723A0"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Prince Edward Island", Code = "PE" }); //Canada
            list.Add(new GeoZone { Id = new Guid("E8426499-9214-41C8-9717-44F2A4D6D14E"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Quebec", Code = "QC" }); //Canada
            list.Add(new GeoZone { Id = new Guid("93215E73-4DF8-4609-AC37-9DA1B9BFE1C9"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Saskatchewan", Code = "SK" }); //Canada
            list.Add(new GeoZone { Id = new Guid("56259F37-AF84-4215-AC73-259FA74C7C8D"), CountryId = new Guid("0C356C5A-CA44-4301-8212-1826CCDADC42"), Name = "Yukon Territory", Code = "YT" }); //Canada


            list.Add(new GeoZone { Id = new Guid("2022F303-2481-4B44-BA3D-D261B002C9C1"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Baden-Württemberg", Code = "BAW" }); //Germany
            list.Add(new GeoZone { Id = new Guid("87C1483D-E471-4166-87CB-44F9C4459AA8"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Bayern", Code = "BAY" }); //Germany
            list.Add(new GeoZone { Id = new Guid("2A9B8FFE-91F5-4944-983D-37F52491DDE6"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Berlin", Code = "BER" }); //Germany
            list.Add(new GeoZone { Id = new Guid("B2B175A4-09BA-4E25-919C-9DE52109BF4D"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Brandenburg", Code = "BRG" }); //Germany
            list.Add(new GeoZone { Id = new Guid("7FCCE82B-7828-40C9-A860-A21A787780C2"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Bremen", Code = "BRE" }); //Germany
            list.Add(new GeoZone { Id = new Guid("36F88C25-7A6A-41D4-ABAC-CE05CD5ECFA1"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Hamburg", Code = "HAM" }); //Germany
            list.Add(new GeoZone { Id = new Guid("BA3C2043-CC3E-4225-B28E-BDB18C1A79EF"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Hessen", Code = "HES" }); //Germany
            list.Add(new GeoZone { Id = new Guid("7ACE8E48-A0C5-48EE-B992-AE6EB7142408"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Mecklenburg-Vorpommern", Code = "MEC" }); //Germany
            list.Add(new GeoZone { Id = new Guid("D55B4820-1CCD-44AD-8FBE-60B750ABC2DD"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Niedersachsen", Code = "NDS" }); //Germany
            list.Add(new GeoZone { Id = new Guid("4BC9F931-F1ED-489F-99BC-59F42BD77EEC"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Nordrhein-Westfalen", Code = "NRW" }); //Germany
            list.Add(new GeoZone { Id = new Guid("07C1030F-FA7E-4B1C-BA21-C6ACD092B676"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Rheinland-Pfalz", Code = "RHE" }); //Germany
            list.Add(new GeoZone { Id = new Guid("69A0494D-F8C3-434B-B8D4-C18CA5AF5A4E"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Saarland", Code = "SAR" }); //Germany
            list.Add(new GeoZone { Id = new Guid("05974280-A62D-4FC3-BE15-F16AB9E0F2D1"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Sachsen", Code = "SAS" }); //Germany
            list.Add(new GeoZone { Id = new Guid("02C10C0F-3F09-4D0A-A6EF-AD40AE0A007B"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Sachsen-Anhalt", Code = "SAC" }); //Germany
            list.Add(new GeoZone { Id = new Guid("FB63F22D-2A32-484E-A3E8-41BBAE13891B"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Schleswig-Holstein", Code = "SCN" }); //Germany
            list.Add(new GeoZone { Id = new Guid("AD9E0130-B735-4BE0-9338-99E20BB9410D"), CountryId = new Guid("2D5B53A8-8341-4DA4-A296-E516FE5BB953"), Name = "Thüringen", Code = "THE" }); //Germany


            list.Add(new GeoZone { Id = new Guid("F92A3196-5C67-4FEC-8877-78B28803B8D6"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "A Coruña", Code = "A Coru?a" }); //Spain
            list.Add(new GeoZone { Id = new Guid("D21905C5-6EE9-4072-9618-8447D9C4390E"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Alava", Code = "Alava" }); //Spain
            list.Add(new GeoZone { Id = new Guid("0F115386-3220-49F1-B0F2-EAF6C78A2EDD"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Albacete", Code = "Albacete" }); //Spain
            list.Add(new GeoZone { Id = new Guid("517F1242-FE90-4322-969E-353C5DBFD061"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Alicante", Code = "Alicante" }); //Spain
            list.Add(new GeoZone { Id = new Guid("347629B4-0C74-4E80-84C9-785FB45FB8D7"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Almeria", Code = "Almeria" }); //Spain
            list.Add(new GeoZone { Id = new Guid("A3CB237B-A940-418F-8368-FA6E35263E22"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Asturias", Code = "Asturias" }); //Spain
            list.Add(new GeoZone { Id = new Guid("070DD166-BDC9-4732-8DA0-48BD318D3D9E"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Avila", Code = "Avila" }); //Spain
            list.Add(new GeoZone { Id = new Guid("8BC664A9-B12C-4F48-AF34-A7F68384A76A"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Badajoz", Code = "Badajoz" }); //Spain
            list.Add(new GeoZone { Id = new Guid("4AB74396-FB33-4276-A518-AD05F28375D0"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Baleares", Code = "Baleares" }); //Spain
            list.Add(new GeoZone { Id = new Guid("15B3D139-D927-43EB-8705-84DF9122999F"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Barcelona", Code = "Barcelona" }); //Spain
            list.Add(new GeoZone { Id = new Guid("956B1071-D4C1-4676-BE0C-E8834E47B674"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Burgos", Code = "Burgos" }); //Spain
            list.Add(new GeoZone { Id = new Guid("7783E2F6-DED1-4703-AA2B-9FC844F28018"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Caceres", Code = "Caceres" }); //Spain
            list.Add(new GeoZone { Id = new Guid("5BBD88D1-5023-43DF-91F0-0FDD4F3878EB"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Cadiz", Code = "Cadiz" }); //Spain
            list.Add(new GeoZone { Id = new Guid("F5315BF8-0DC2-49E7-ABEB-0D7348492E6B"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Cantabria", Code = "Cantabria" }); //Spain
            list.Add(new GeoZone { Id = new Guid("3EBF7CEB-8E24-40AF-801C-FECCD6D780EE"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Castellon", Code = "Castellon" }); //Spain
            list.Add(new GeoZone { Id = new Guid("6E0EB9AC-76A2-434D-AE13-18DBE56212BF"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Ceuta", Code = "Ceuta" }); //Spain
            list.Add(new GeoZone { Id = new Guid("640CEF26-1B10-4EAC-A4AE-2F3491C38376"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Ciudad Real", Code = "Ciudad Real" }); //Spain
            list.Add(new GeoZone { Id = new Guid("8A4E0E4C-2727-42CD-86D6-ED27A6A6B74B"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Cordoba", Code = "Cordoba" }); //Spain
            list.Add(new GeoZone { Id = new Guid("CA553819-434A-408F-A2A4-92A7DF9A2618"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Cuenca", Code = "Cuenca" }); //Spain
            list.Add(new GeoZone { Id = new Guid("FEA759DA-4280-46A8-AF3F-EC2CC03B436A"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Girona", Code = "Girona" }); //Spain
            list.Add(new GeoZone { Id = new Guid("1DA58A0A-D0F7-48B1-9D48-102F65819773"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Granada", Code = "Granada" }); //Spain
            list.Add(new GeoZone { Id = new Guid("C2BA8E9E-D370-4639-B168-C51057E2397E"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Guadalajara", Code = "Guadalajara" }); //Spain
            list.Add(new GeoZone { Id = new Guid("21287450-809E-4662-9742-9380159D3C90"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Guipuzcoa", Code = "Guipuzcoa" }); //Spain
            list.Add(new GeoZone { Id = new Guid("CF6E4B72-5F4F-4CC4-ADD3-EB0964892F7B"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Huelva", Code = "Huelva" }); //Spain
            list.Add(new GeoZone { Id = new Guid("CC6B7A8E-4275-4E4E-8D62-34B5480F3995"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Huesca", Code = "Huesca" }); //Spain
            list.Add(new GeoZone { Id = new Guid("CBD2718F-DD60-4151-A24D-437FF37605C6"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Jaen", Code = "Jaen" }); //Spain
            list.Add(new GeoZone { Id = new Guid("3DAB4424-EFA5-409A-B96C-40DAF5EE4B6C"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "La Rioja", Code = "La Rioja" }); //Spain
            list.Add(new GeoZone { Id = new Guid("0DB04A9E-352B-46D6-88BC-B5416B31756D"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Las Palmas", Code = "Las Palmas" }); //Spain
            list.Add(new GeoZone { Id = new Guid("7CE436E6-349D-4F41-9053-5D7666662BB8"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Leon", Code = "Leon" }); //Spain
            list.Add(new GeoZone { Id = new Guid("1BA313DE-0690-42DB-97BB-ECBA89AEC4C7"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Lleida", Code = "Lleida" }); //Spain
            list.Add(new GeoZone { Id = new Guid("CB47CC62-5D26-4B17-B01F-25E5432F913C"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Lugo", Code = "Lugo" }); //Spain
            list.Add(new GeoZone { Id = new Guid("D96D5675-F3E2-42FE-B581-BD2367DC5012"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Madrid", Code = "Madrid" }); //Spain
            list.Add(new GeoZone { Id = new Guid("FE29FFDB-5E1C-44BD-BB9A-2E2E43C1B206"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Malaga", Code = "Malaga" }); //Spain
            list.Add(new GeoZone { Id = new Guid("BB090CE7-E0CA-4D0D-96EB-1B8E044FBCA8"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Melilla", Code = "Melilla" }); //Spain
            list.Add(new GeoZone { Id = new Guid("C26CFB75-5E44-4156-B660-A18A2A487FEC"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Murcia", Code = "Murcia" }); //Spain
            list.Add(new GeoZone { Id = new Guid("B7500C17-30C7-4D87-BB47-BB35D8B1D3A6"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Navarra", Code = "Navarra" }); //Spain
            list.Add(new GeoZone { Id = new Guid("AA492AC6-E3B1-4408-B503-81480B57F008"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Ourense", Code = "Ourense" }); //Spain
            list.Add(new GeoZone { Id = new Guid("D226235D-0EB0-49C5-9E7A-55CC91C57100"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Palencia", Code = "Palencia" }); //Spain
            list.Add(new GeoZone { Id = new Guid("D21E2732-779D-406A-B1B9-CF44FF280DFE"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Pontevedra", Code = "Pontevedra" }); //Spain
            list.Add(new GeoZone { Id = new Guid("3249C886-3B1E-426A-8CD7-EFC3922A964A"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Salamanca", Code = "Salamanca" }); //Spain
            list.Add(new GeoZone { Id = new Guid("D52CEDAC-FCC2-4B9C-8F9E-09DCDA91974C"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Santa Cruz de Tenerife", Code = "Santa Cruz de Tenerife" }); //Spain
            list.Add(new GeoZone { Id = new Guid("4344C1DD-E866-4683-9C90-22C9DB369EAE"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Segovia", Code = "Segovia" }); //Spain
            list.Add(new GeoZone { Id = new Guid("2546D1AB-D4F5-4087-9B78-EA3BADFAFA12"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Sevilla", Code = "Sevilla" }); //Spain
            list.Add(new GeoZone { Id = new Guid("C7A02C1C-3076-43B3-9538-B513BAB8A243"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Soria", Code = "Soria" }); //Spain
            list.Add(new GeoZone { Id = new Guid("2F20005E-7EFC-4186-9144-6996B68EE6E3"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Tarragona", Code = "Tarragona" }); //Spain
            list.Add(new GeoZone { Id = new Guid("7DC834F4-C490-4986-BFBC-10DFC94E235C"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Teruel", Code = "Teruel" }); //Spain
            list.Add(new GeoZone { Id = new Guid("DB9CCCCF-9E20-4224-88A7-067E5238960D"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Toledo", Code = "Toledo" }); //Spain
            list.Add(new GeoZone { Id = new Guid("9C9951D7-68D2-438A-A702-4289CBC1720E"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Valencia", Code = "Valencia" }); //Spain
            list.Add(new GeoZone { Id = new Guid("DAD6586A-C504-4117-B116-4C80A0D1BF52"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Valladolid", Code = "Valladolid" }); //Spain
            list.Add(new GeoZone { Id = new Guid("1C5D3479-59FC-4C77-8D4E-CFC5C33422E7"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Vizcaya", Code = "Vizcaya" }); //Spain
            list.Add(new GeoZone { Id = new Guid("60D9D569-7D0D-448F-B567-B4BB6C518140"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Zamora", Code = "Zamora" }); //Spain
            list.Add(new GeoZone { Id = new Guid("E026BF9D-66A9-49BF-BA77-860B8C60871D"), CountryId = new Guid("38DC01C3-48D8-4FF8-A78A-2C35D4FBFA7B"), Name = "Zaragoza", Code = "Zaragoza" }); //Spain


            list.Add(new GeoZone { Id = new Guid("30FA3416-9FB1-43C1-999D-23A115218324"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Aargau", Code = "AG" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("D892EA50-FCCF-477A-BBDF-418E32DC5B98"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Appenzell Ausserrhoden", Code = "AR" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("AFA207C7-E69D-46F0-8242-2A67A06C42E3"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Appenzell Innerrhoden", Code = "AI" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("25459871-1694-4D08-9E7C-6D06F2EDC7AE"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Basel-Landschaft", Code = "BL" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("58C1E282-CFFA-4B49-B268-5356BA47AA19"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Basel-Stadt", Code = "BS" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("780D9DDB-38A2-47C8-A162-1231BEA2E54D"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Bern", Code = "BE" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("8A6DB145-7FF4-4DFA-AC88-EA161924EA03"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Freiburg", Code = "FR" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("6C342C68-690A-4967-97C6-E6408CA1EA59"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Genf", Code = "GE" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("61D891A3-E620-46D8-AADA-6C9C1944340C"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Glarus", Code = "GL" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("DCC28B9C-8D2F-4569-AD0A-AD5717DA3BB7"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Graubünden", Code = "JU" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("053FAB61-2EFF-446B-A29B-E9BE91E195C9"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Jura", Code = "JU" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("388A4219-A89A-4BF0-960F-F58936288A0A"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Luzern", Code = "LU" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("E885E0CE-A268-4DB0-AFF2-A0205353E7E4"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Neuenburg", Code = "NE" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("9C24162B-10DE-47C1-B55F-0DCAAA24F86E"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Nidwalden", Code = "NW" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("8B3C48FD-9E7E-4653-A711-6DAC6971CB32"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Obwalden", Code = "OW" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("076814FC-7422-40D5-80E0-B6978589CCDC"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Schaffhausen", Code = "SH" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("86BDBE5D-4085-4916-984C-94C191C48C67"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Schwyz", Code = "SZ" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("CCD7968C-7E80-4381-958B-AB72BE0D6C35"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Solothurn", Code = "SO" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("C1983F1D-353A-4042-B097-F0E8237F7FCD"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "St. Gallen", Code = "SG" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("B519AAAF-7E2C-421F-88B8-BF7853A8DE4F"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Tessin", Code = "TI" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("3DEDA5E5-10BB-41CD-87FF-F91688B5B7ED"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Thurgau", Code = "TG" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("67E1633F-7405-451D-A772-EB4119C13B2C"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Uri", Code = "UR" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("B9F911EB-F762-4DA4-A81F-9BC967CD3C4B"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Waadt", Code = "VD" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("440E892D-693C-493B-BA14-81919C3FB091"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Wallis", Code = "VS" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("EC2A6FED-19C2-4364-99CB-A59E8E0929FE"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Zürich", Code = "ZH" }); //Switzerland
            list.Add(new GeoZone { Id = new Guid("4BD4724C-2E5E-4DF4-8B1C-3A679C30398F"), CountryId = new Guid("60CE9AB1-945D-4FEF-ABA8-A1BB640165BE"), Name = "Zug", Code = "ZG" }); //Switzerland


            list.Add(new GeoZone { Id = new Guid("63C099A8-5537-4C80-8654-A6128EE1B203"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Aberdeenshire, Scotland", Code = "ABD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("CA7CE68D-465A-4C6E-AD75-6F9ADE467F1E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Alderney, Channel Islands", Code = "ALD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("37CF9D44-030D-48BE-97F0-5B3DAB24F48F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Anglesey, Wales", Code = "AGY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("28061E80-5D1C-4D47-9E99-A72525B63F85"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Angus, Scotland", Code = "ANS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("584CF595-C117-4D7E-9A0B-6DADD748EDA8"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Argyllshire, Scotland", Code = "ARL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("481F03A2-A3FE-41D9-A938-920720C1F446"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Avon, England", Code = "AVN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("4445EB1E-0888-4B11-BF7B-0BB9A701936D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Ayrshire, Scotland", Code = "AYR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C5B34C76-5BF8-423F-A56F-985CC545ADE8"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Banffshire, Scotland", Code = "BAN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("7190E292-34CF-49F3-8367-A5FAFB749CD3"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Bedfordshire, England", Code = "BDF" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("80717FF0-5218-4119-9128-CEF942826EDC"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Berkshire, England", Code = "BRK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("504D426D-CD02-446D-910C-5E4E36518879"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Berwickshire, Scotland", Code = "BEW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("98F9DB63-A224-462B-8765-240953489CBB"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Borders, Scotland", Code = "BOR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1B4BDC9D-5D38-43C4-97AA-BAD370A18FB4"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Breconshire, Wales", Code = "BRE" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("843C2659-D2BF-4AF0-A0AA-EE8C268BEDE7"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Buckinghamshire, England", Code = "BKM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F894C6BF-2F76-4B42-85F4-B89581CEE97F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Bute, Scotland", Code = "BUT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B9032648-36DC-4903-A5E6-30ABD7519754"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Caernarvonshire, Wales", Code = "CAE" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("7786D5CE-A4F9-4CF0-82FD-25A7EBE39FC5"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Caithness, Scotland", Code = "CAI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3EE49FF8-A56A-451A-A999-067915C8DD75"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cambridgeshire, England", Code = "CAM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C4FA438D-7130-4AE0-9F5D-FB533AFC3139"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cardiganshire, Wales", Code = "CGN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A0984F35-6AF2-493D-BE85-903D453193C2"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Carmarthenshire, Wales", Code = "CMN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("CD1579C0-C471-4095-867A-3E2AF11E1F35"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Central, Scotland", Code = "CEN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("473BBC63-7D8C-4587-945C-32F943091FF4"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Channel Islands", Code = "CHI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("446CB079-FC60-478A-B8CB-D8B7ECE3383D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cheshire, England", Code = "CHS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F6A35C2C-AB10-4531-AFEA-2CDBDF40F5C1"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Clackmannanshire, Scotland", Code = "CLK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2AB44C64-8419-45A1-A78F-83894D679EA9"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cleveland, England", Code = "CLV" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("DF964122-964E-4067-8386-45BCB548E39D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Clwyd, Wales", Code = "CWD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("981B0264-12A2-43BE-B0C6-54E81C960138"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Antrim, Northern Ireland", Code = "ANT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A1B99E38-1A7F-4B2F-927B-FCBDFCCBC198"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Armagh, Northern Ireland", Code = "ARM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("6AC7BEA2-D4BA-4C48-B0D0-F784AF781587"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Carlow, Ireland", Code = "CAR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("27129DC0-1DD2-497F-B724-65D93E0050BE"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Cavan, Ireland", Code = "CAV" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B9720A4E-9CA0-4120-A7CF-F81138B1DB63"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Clare, Ireland", Code = "CLA" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("BBD4704A-100A-4533-9AC9-37C228711DAE"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Cork, Ireland", Code = "COR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("7C68C309-608D-4089-ADBC-F5289D67AA57"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Donegal, Ireland", Code = "DON" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("4182CEBC-5177-48AE-81F7-0C356139494B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Down, Northern Ireland", Code = "DOW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("09D1A93D-64BA-4205-9B93-81BA8EB8FECA"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Dublin, Ireland", Code = "DUB" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("46F46BEF-20B8-4315-8BF1-F816BCB06B8E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Durham, England", Code = "DUR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("997FD24B-E5C6-474A-8C70-DBE6652B9267"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Fermanagh, Northern Ireland", Code = "FER" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D8DE090A-D496-42EA-A1E0-F457DAB41F14"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Galway, Ireland", Code = "GAL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("4FA42D2C-B375-41D4-98EF-4D1442BCCB1A"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Kerry, Ireland", Code = "KER" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("43830082-5772-47F2-8216-8A48C872E337"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Kildare, Ireland", Code = "KID" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9D10E877-AAEC-4913-BC99-B9815AF76BF2"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Kilkenny, Ireland", Code = "KIK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("628F9F88-55EF-4EEF-BCB6-B866EC05838D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Laois, Ireland", Code = "LEX" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C6CF9405-369E-41F4-A5E6-435469596C08"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Leitrim, Ireland", Code = "LET" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("4DA1DAC0-6C99-4A28-9D94-6A3DF5507727"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Limerick, Ireland", Code = "LIM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B009FD08-9CB2-4CD4-85CC-ED7C3E413F59"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Londonderry, Northern Ireland", Code = "LDY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2189B5A8-167F-425D-949C-B3858179003E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Longford, Ireland", Code = "LOG" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3F030F3B-0A0A-464E-B86E-9CD9E7A97B8B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Louth, Ireland", Code = "LOU" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E7B4F7CB-CB36-4795-BFEB-DBD14BF2D520"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Mayo, Ireland", Code = "MAY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B8029F8E-0A19-41A5-A2F0-CF8F0E1A69C6"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Meath, Ireland", Code = "MEA" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("6E54CC16-2A7D-4662-9F2B-1F7808318412"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Monaghan, Ireland", Code = "MOG" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3FCF9EE7-66BC-4CF6-AEF3-D8C70948ECDE"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Offaly, Ireland", Code = "OFF" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9451F5B9-3DBC-4C17-83B9-8966350D26CA"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Roscommon, Ireland", Code = "ROS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("EA553C01-9023-41EF-9068-849A054775F6"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Sligo, Ireland", Code = "SLI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F80A1747-3D1A-4758-B74A-BA2B54844B8B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Tipperary, Ireland", Code = "TIP" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("49025D80-75EE-4367-A06C-88427507642B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Tyrone, Northern Ireland", Code = "TYR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("FD809ED1-732F-4886-9BBC-F96984329B60"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Waterford, Ireland", Code = "WAT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("AE1FAEAF-D3F8-484E-B5A8-4548441AE758"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Westmeath, Ireland", Code = "WEM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3CB3EF7B-B000-41FA-BFD2-405F29BD646F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Wexford, Ireland", Code = "WEX" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("BAF9725F-0D57-4C84-B018-D6D55D00A647"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Co. Wicklow, Ireland", Code = "WIC" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("5469CA74-B57E-41C1-B3E6-5AB725E7F423"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cornwall, England", Code = "CON" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1DDFCA11-3848-4945-848C-AE5CB67E0E8B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cumberland, England", Code = "CUL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("BCEA3DBC-DEBB-483E-85B9-A3E47FF68DBF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Cumbria, England", Code = "CMA" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("EDD15E05-A785-47D0-9936-8489858F1D89"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Denbighshire, Wales", Code = "DEN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F1C315B6-8573-4641-85B1-E9BF76502968"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Derbyshire, England", Code = "DBY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("64D687FC-1908-4323-AB47-991EA4371186"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Devon, England", Code = "DEV" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3F05459F-5453-4AA1-9565-56B05080181D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Dorset, England", Code = "DOR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("540233B4-A7C9-4E61-B54D-186763A2C65D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Dumfries and Galloway, Scotland", Code = "DGY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2D04DE9B-525D-4623-A368-B29DD82DBBD0"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Dumfries-shire, Scotland", Code = "DFS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("34141474-337E-4E28-9180-23620558BA1D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Dunbartonshire, Scotland", Code = "DNB" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("95F80D75-23BC-4710-A106-BB98204059DE"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Dyfed, Wales", Code = "DFD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9013454D-1873-4BE6-8FB1-B503AE1ED652"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "East Lothian, Scotland", Code = "ELN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("5241E330-B255-4FFA-833C-4964F13A0F7B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "East Riding of Yorkshire, England", Code = "ERY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B411F228-7677-4203-8696-DF1A65E1651F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "East Sussex, England", Code = "SXE" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D0C79BD1-3688-4ED3-9B65-0CB24A1E8B43"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "England", Code = "ENG" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("42E55B19-E977-4E00-830C-A1655CF8A072"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Essex, England", Code = "ESS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("EDFB040F-BB1E-47D7-8A16-14AC9D2F2F2F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Fife, Scotland", Code = "FIF" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("04E066BE-2254-44C5-AB1F-41AFE5267CE3"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Flintshire, Wales", Code = "FLN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("67F56BE1-88C8-474D-BEB9-5E75CD3B6062"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Glamorgan, Wales", Code = "GLA" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F44AC5B9-B998-46EF-B335-9FB42F97FE27"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Gloucestershire, England", Code = "GLS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("958CF4DD-04BE-48E3-93E1-3BEA8B80EDCF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Grampian, Scotland", Code = "GMP" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B5EBABD8-7A23-4006-8044-5049FCF8A762"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Greater Manchester, England", Code = "GTM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("DCC2AB57-48E6-4C14-BF14-E8D72C389863"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Guernsey, Channel Islands", Code = "GSY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("BAA695AB-A67A-409F-9F74-189AB212260C"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Gwent, Wales", Code = "GNT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9BC8834B-01CE-40A2-8BE1-8E20C24D1F11"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Gwynedd, Wales", Code = "GWN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A1EABA97-8F4F-4149-B1E3-25CB19B145B7"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Hampshire, England", Code = "HAM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1D4EABB2-D6F1-44AD-BE62-52EF66E5B04B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Hereford and Worcester, England", Code = "HWR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("FD19F293-D0CA-43D2-8274-810EDDF75D21"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Herefordshire, England", Code = "HEF" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E6322365-8CBC-4D69-9515-341C4B038781"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Hertfordshire, England", Code = "HRT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B0720A71-450B-4912-810E-871C8EF518E2"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Highland, Scotland", Code = "HLD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("043A0D4A-F1F5-430D-906D-EBD3D219485F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Humberside, England", Code = "HUM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D2E5BA25-D3D3-4113-B599-7456755DA29E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Huntingdonshire, England", Code = "HUN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A63A1DFF-8F61-4159-9BC5-F005F8BCC19F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Inverness-shire, Scotland", Code = "INV" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("46D3633D-8DB5-4CC8-B42E-76DD3D48458D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Ireland", Code = "IRL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E2A9AE21-97D6-4E4C-AC55-7FE75A298F6C"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Isle of Man", Code = "IOM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("40F760F8-531F-4FEA-9773-A513A7B58AF8"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Isle of Wight, England", Code = "IOW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3E256C55-177F-459C-97CF-A77FB3729494"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Jersey, Channel Islands", Code = "JSY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("8FF34A94-016C-4046-9C39-99F1887C4CB9"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Kent, England", Code = "KEN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("AC353AAC-E2B7-4899-94F9-0BAD7418ACE9"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Kincardineshire, Scotland", Code = "KCD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A67BBF90-32F8-45D0-8C9A-1AF135EA6225"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Kinross-shire, Scotland", Code = "KRS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("49FFFE3B-61F4-433D-B9BC-4044398283CF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Kirkcudbrightshire, Scotland", Code = "KKD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("730F58D1-9129-4F25-A94C-1A0F2F373BCD"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Lanarkshire, Scotland", Code = "LKS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("36B807A9-F496-430D-91BC-E8B1AC738736"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Lancashire, England", Code = "LAN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("CD98173E-507E-4A92-9EEF-2DE1D8E7A61F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Leicestershire, England", Code = "LEI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("59997515-5699-4571-8C69-B91328B65A3F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Lincolnshire, England", Code = "LIN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("356BD975-9775-40D4-9678-FC49098F0A02"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "London, England", Code = "LND" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B299CAD1-F84C-4AA7-B63A-2746B25BAAAA"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Lothian, Scotland", Code = "LTN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E3835A5D-1ECA-4B9C-8C76-F52F4FC17553"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Merionethshire, Wales", Code = "MER" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B7AED2AB-9E68-45D1-B535-FE6C1EC2D892"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Merseyside, England", Code = "MSY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D799B8C1-CAC7-4933-AA6D-29ACF7FD9D91"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Mid Glamorgan, Wales", Code = "MGM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("37A4B316-853A-40B1-8203-B8C27D07917E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Middlesex, England", Code = "MDX" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("72C87C62-5656-4811-84CD-0C5CE4B7D19F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Midlothian, Scotland", Code = "MLN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("87E6C0A9-AC01-457A-B759-0C10FC63605D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Monmouthshire, Wales", Code = "MON" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("30A0E005-A523-4301-B924-8A4651F54E90"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Montgomeryshire, Wales", Code = "MGY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C427A41A-2FD0-4621-A7B1-090EE19F2EBA"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Morayshire, Scotland", Code = "MOR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("87CEB5AF-D6C0-4189-88F2-F0B38E2223A6"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Nairn, Scotland", Code = "NAI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A2EE590A-0B2E-4688-8C87-83011D5D2D3B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Norfolk, England", Code = "NFK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("FCA86C53-D823-41F7-A28D-2146F23E93EF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "North Riding of Yorkshire, England", Code = "NRY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B00A500E-73D6-4229-93CC-99A255266C86"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "North Yorkshire, England", Code = "NYK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("849C05F0-9448-4013-8566-88FF891D1F6E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Northamptonshire, England", Code = "NTH" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D46DC54C-28F5-4EFE-A2BF-F5A814096736"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Northern Ireland", Code = "NIR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1F6D4673-67A2-4313-8837-569B6B671685"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Northumberland, England", Code = "NBL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("01EE03B8-3EA2-4DE4-8656-EC10138F95EA"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Nottinghamshire, England", Code = "NTT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("49EC50BE-2751-443E-ACF8-4C497633267D"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Orkney, Scotland", Code = "OKI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("10ADEE5D-3EC5-4C70-A0D0-9C399A785DD2"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Oxfordshire, England", Code = "OXF" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C7547571-7CDA-427A-A8F2-259E9C587E84"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Peebles-shire, Scotland", Code = "PEE" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D2C0E797-781A-4D5D-A87C-5C16DE2063CB"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Pembrokeshire, Wales", Code = "PEM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B588E3F6-0078-454C-809A-480C575E5200"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Perth, Scotland", Code = "PER" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("56FABA9F-B482-4421-87CC-1EE320DA22CD"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Powys, Wales", Code = "POW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E4DA2345-E459-4B6A-982A-337A8AE84E1B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Radnorshire, Wales", Code = "RAD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2DFE6223-5A2B-4DBC-B77C-9AF80E973A20"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Renfrewshire, Scotland", Code = "RFW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("EF661A83-6355-44D2-AEA4-89F7A7C7BD21"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Ross and Cromarty, Scotland", Code = "ROC" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3933E77A-10F0-47E2-BB60-02DE2AD724DF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Roxburghshire, Scotland", Code = "ROX" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("0B525A39-59CE-40F3-91CA-8676A5404E23"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Rutland, England", Code = "RUT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("DADA271D-2656-4C17-B570-72BB748EB7DC"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Sark, Channel Islands", Code = "SRK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9958D966-7824-4DEA-94A4-725FCF96F0D0"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Scotland", Code = "SCT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("DA7FC721-B375-4B51-AFE1-1CB11328C7A8"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Selkirkshire, Scotland", Code = "SEL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("9D62722C-7FCC-432C-8587-C7953E440E18"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Shetland, Scotland", Code = "SHI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E5F56833-0EA5-40D8-ACC0-0D0F48197A78"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Shropshire, England", Code = "SAL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1D91F9F6-0D62-479A-91A7-62D04FE1FDEF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Somerset, England", Code = "SOM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("BEC44D04-EC8E-45A8-AF09-21468AD9994F"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "South Glamorgan, Wales", Code = "SGM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("968A394F-7764-4DFC-ADB3-BF881644EDDF"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "South Yorkshire, England", Code = "SYK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D1562C4D-3163-4E92-A361-05C2B9541772"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Staffordshire, England", Code = "STS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("FEA24D8C-CA6B-4643-8F8B-D5133AC40B18"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Stirlingshire, Scotland", Code = "STI" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("510C1204-13C5-4F0E-A746-5D1C5F843DFB"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Strathclyde, Scotland", Code = "STD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A91BF8C6-0F2B-4EA8-8B98-3F70CD240BC2"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Suffolk, England", Code = "SFK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2135D11F-4B63-46A5-B4C0-C62E130CE021"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Surrey, England", Code = "SRY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("1AC6BD1B-6B0C-4857-8243-BCA4BA6EEB5E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Sussex, England", Code = "SSX" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("C2C00CAA-3C72-4B5E-A07F-6F605926EF8E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Sutherland, Scotland", Code = "SUT" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("F086728A-8077-4CED-8889-6D9A6E0AB147"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Tayside, Scotland", Code = "TAY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("10185A2B-6AF7-4735-B1E3-8C46AAC842FD"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Tyne and Wear, England", Code = "TWR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("883B8625-3130-4AAC-B239-E57CF79C020B"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Wales", Code = "WLS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2B350062-EAF4-4F05-AB04-A8CCEC353EB5"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Warwickshire, England", Code = "WAR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("E6C0D492-93FC-4798-B6FA-0180F08204F6"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Glamorgan, Wales", Code = "WGM" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("38885962-564A-4686-9099-AA06570E00BD"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Lothian, Scotland", Code = "WLN" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("0971F796-ED73-46D7-9ACF-38CD54EF7F54"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Midlands, England", Code = "WMD" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("7759C9E9-43BC-4570-A9BB-C578564A0951"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Riding of Yorkshire, England", Code = "WRY" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("2B0707FF-EF1A-498F-AF98-FACB2BD9F9C1"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Sussex, England", Code = "SXW" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("B11A9785-C227-47ED-A14E-402A4B5360C7"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "West Yorkshire, England", Code = "WYK" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("D6EBAD0E-4C95-4EC8-8D0E-2543ACC6AD11"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Western Isles, Scotland", Code = "WIS" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("7A2ADE5F-8353-4326-A46D-A42D31370D2C"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Westmorland, England", Code = "WES" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("A540B300-1715-4677-AA6A-9EE79E6FEF2E"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Wigtownshire, Scotland", Code = "WIG" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("05963D51-677F-4F3B-B210-5103A1036506"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Wiltshire, England", Code = "WIL" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("3D11EE42-F2D3-4C0D-81B9-44394A3A5409"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Worcestershire, England", Code = "WOR" }); //United Kingdom
            list.Add(new GeoZone { Id = new Guid("71366F2A-E8B5-469F-A995-A0410DC33FD8"), CountryId = new Guid("1FCC4A89-0E8F-4FA2-B8D0-4FF5EC2277DF"), Name = "Yorkshire, England", Code = "YKS" }); //United Kingdom


            list.Add(new GeoZone { Id = new Guid("B8BF0B26-2F14-49E4-BFDA-2D01EAFA364B"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Alabama", Code = "AL" }); //United States
            list.Add(new GeoZone { Id = new Guid("611023EB-D4F2-4831-812E-C3984A125310"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Alaska", Code = "AK" }); //United States
            list.Add(new GeoZone { Id = new Guid("7566D0A5-7394-4947-B4D7-A76A94746A23"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "American Samoa", Code = "AS" }); //United States
            list.Add(new GeoZone { Id = new Guid("5BD4A551-46BA-465A-B3F9-E15ED70A083F"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Arizona", Code = "AZ" }); //United States
            list.Add(new GeoZone { Id = new Guid("7BF366D4-E9FC-4715-B7F9-1AF37CC97386"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Arkansas", Code = "AR" }); //United States
            list.Add(new GeoZone { Id = new Guid("D318E32E-41B6-4CA6-905D-23714709F38F"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Africa", Code = "AF" }); //United States
            list.Add(new GeoZone { Id = new Guid("A3A183AE-8117-46C0-93B7-3940C7E5694F"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Americas", Code = "AA" }); //United States
            list.Add(new GeoZone { Id = new Guid("993207EC-34A5-4896-88B0-3C43CCD11AB2"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Canada", Code = "AC" }); //United States
            list.Add(new GeoZone { Id = new Guid("93CDD758-CC83-4F5A-94C0-9A3D13C7FA44"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Europe", Code = "AE" }); //United States
            list.Add(new GeoZone { Id = new Guid("3C173B83-5149-4FEC-B000-64A65832C455"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Middle East", Code = "AM" }); //United States
            list.Add(new GeoZone { Id = new Guid("AB47DF32-C57D-412B-B04D-67378C120AE7"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Armed Forces Pacific", Code = "AP" }); //United States
            list.Add(new GeoZone { Id = new Guid("48D12A99-BF3C-4FC7-86C5-C266424973EB"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "California", Code = "CA" }); //United States
            list.Add(new GeoZone { Id = new Guid("3FF66466-E523-492E-80C1-BE19AF171364"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Colorado", Code = "CO" }); //United States
            list.Add(new GeoZone { Id = new Guid("C3E70597-E8DD-4277-B7FC-E9B4206DA073"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Connecticut", Code = "CT" }); //United States
            list.Add(new GeoZone { Id = new Guid("F23BAB33-CAD9-4D9C-9CED-A66B3FF4969F"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Delaware", Code = "DE" }); //United States
            list.Add(new GeoZone { Id = new Guid("E83159F2-ABE3-4F94-80DE-A149BCF83428"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "District of Columbia", Code = "DC" }); //United States
            list.Add(new GeoZone { Id = new Guid("6352D079-20EA-42DA-9377-7A09E6B764AE"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Federated States Of Micronesia", Code = "FM" }); //United States
            list.Add(new GeoZone { Id = new Guid("933CD9EF-C021-48ED-8260-6C013685970F"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Florida", Code = "FL" }); //United States
            list.Add(new GeoZone { Id = new Guid("F1BBC9FC-4B0A-4065-843E-F428F1C20346"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Georgia", Code = "GA" }); //United States
            list.Add(new GeoZone { Id = new Guid("8BD9D2B9-67DB-4FD6-90C7-52D0426E2007"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Guam", Code = "GU" }); //United States
            list.Add(new GeoZone { Id = new Guid("B5812090-E7E1-492B-B9BC-04FEC3EC9492"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Hawaii", Code = "HI" }); //United States
            list.Add(new GeoZone { Id = new Guid("85F3B62E-D3E7-4DEC-B13B-DD494AD7B2CC"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Idaho", Code = "ID" }); //United States
            list.Add(new GeoZone { Id = new Guid("6243F71B-D89B-4FDC-BC01-FCF46AEB1F29"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Illinois", Code = "IL" }); //United States
            list.Add(new GeoZone { Id = new Guid("C7330896-BD61-4282-B3BF-8713A28D3B50"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Indiana", Code = "IN" }); //United States
            list.Add(new GeoZone { Id = new Guid("1026B90D-61BE-4434-AB6D-EBFD92082DFE"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Iowa", Code = "IA" }); //United States
            list.Add(new GeoZone { Id = new Guid("A39F8A9A-6586-41FB-9D5F-F84BD5161333"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Kansas", Code = "KS" }); //United States
            list.Add(new GeoZone { Id = new Guid("84BF6B91-F9FF-4203-BAD1-B5CF01239B77"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Kentucky", Code = "KY" }); //United States
            list.Add(new GeoZone { Id = new Guid("EBC9105F-1F6E-44BE-B4F2-6D23908278D6"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Louisiana", Code = "LA" }); //United States
            list.Add(new GeoZone { Id = new Guid("335C6BA3-37E5-4CCA-B466-6927658EE92E"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Maine", Code = "ME" }); //United States
            list.Add(new GeoZone { Id = new Guid("61952DAD-6B28-4BA8-8580-5012A48ACCDC"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Marshall Islands", Code = "MH" }); //United States
            list.Add(new GeoZone { Id = new Guid("74062D11-8784-40BC-A95D-43B785EF8196"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Maryland", Code = "MD" }); //United States
            list.Add(new GeoZone { Id = new Guid("D2880E75-E454-41A1-A73D-B2CFF71197E2"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Massachusetts", Code = "MA" }); //United States
            list.Add(new GeoZone { Id = new Guid("87268168-CF40-442F-A526-06DDAEB1BEFD"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Michigan", Code = "MI" }); //United States
            list.Add(new GeoZone { Id = new Guid("02BE94A5-3C10-4F83-858B-812796E714AE"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Minnesota", Code = "MN" }); //United States
            list.Add(new GeoZone { Id = new Guid("CF75931A-D86F-43A0-8BD9-3942D5945FF7"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Mississippi", Code = "MS" }); //United States
            list.Add(new GeoZone { Id = new Guid("D907D2A6-4CAA-4687-898A-58BD5F978D03"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Missouri", Code = "MO" }); //United States
            list.Add(new GeoZone { Id = new Guid("71682C43-E9C4-4D96-89E7-B06D47CAA053"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Montana", Code = "MT" }); //United States
            list.Add(new GeoZone { Id = new Guid("6E9D7937-3614-465E-8534-AA9A52F2C69B"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Nebraska", Code = "NE" }); //United States
            list.Add(new GeoZone { Id = new Guid("31265516-54AF-4551-AF1B-A0900FAA3028"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Nevada", Code = "NV" }); //United States
            list.Add(new GeoZone { Id = new Guid("8587E33E-25FC-4C19-B504-0C93C027DD93"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "New Hampshire", Code = "NH" }); //United States
            list.Add(new GeoZone { Id = new Guid("6743C28C-580D-4705-9B01-AA4380D65CE9"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "New Jersey", Code = "NJ" }); //United States
            list.Add(new GeoZone { Id = new Guid("48184D25-0757-405D-934D-74D96F9745DF"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "New Mexico", Code = "NM" }); //United States
            list.Add(new GeoZone { Id = new Guid("EA73C8EB-CAC2-4B28-BB9A-D923F32C17EF"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "New York", Code = "NY" }); //United States
            list.Add(new GeoZone { Id = new Guid("FEC3A4F7-E3B5-44D3-BBDE-62628489B459"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "North Carolina", Code = "NC" }); //United States
            list.Add(new GeoZone { Id = new Guid("5399DF4C-92D4-4C59-9BFB-7DC2A575A3D3"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "North Dakota", Code = "ND" }); //United States
            list.Add(new GeoZone { Id = new Guid("1AA7127A-8C53-4840-A2DA-120F8C6607BD"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Ohio", Code = "OH" }); //United States
            list.Add(new GeoZone { Id = new Guid("4D238397-AF29-4DBC-A349-7F650A5D8D67"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Oklahoma", Code = "OK" }); //United States
            list.Add(new GeoZone { Id = new Guid("DEC30815-883A-45A2-9318-BFB111B383D6"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Oregon", Code = "OR" }); //United States
            list.Add(new GeoZone { Id = new Guid("F6B97ED0-D090-4C68-A590-8FE743EE6D43"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Palau", Code = "PW" }); //United States
            list.Add(new GeoZone { Id = new Guid("8EE2F892-4EE6-44F5-938A-B553E885161A"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Pennsylvania", Code = "PA" }); //United States
            list.Add(new GeoZone { Id = new Guid("152F8DC5-5CAA-44B7-89A8-6469042DC865"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Puerto Rico", Code = "PR" }); //United States
            list.Add(new GeoZone { Id = new Guid("15C350C0-058C-474D-A7C2-E3BD359B7895"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Rhode Island", Code = "RI" }); //United States
            list.Add(new GeoZone { Id = new Guid("BA5A801B-11C6-4408-B097-08AAC22E739E"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "South Carolina", Code = "SC" }); //United States
            list.Add(new GeoZone { Id = new Guid("978ECAAB-C462-4D66-80B6-A65EB83B86A5"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "South Dakota", Code = "SD" }); //United States
            list.Add(new GeoZone { Id = new Guid("41898A0B-A26C-44CE-9568-CFB75F1A2856"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Tennessee", Code = "TN" }); //United States
            list.Add(new GeoZone { Id = new Guid("570FE94C-F226-4701-8C10-13DAB9E59625"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Texas", Code = "TX" }); //United States
            list.Add(new GeoZone { Id = new Guid("CB6D309D-ED20-48D0-8A5D-CD1D7FD1AAD6"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Utah", Code = "UT" }); //United States
            list.Add(new GeoZone { Id = new Guid("8B1FE477-DB16-4DCB-92F0-DCBF2F1DE8CB"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Vermont", Code = "VT" }); //United States
            list.Add(new GeoZone { Id = new Guid("507E831C-8D74-44BF-A251-496B945FAED9"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Virgin Islands", Code = "VI" }); //United States
            list.Add(new GeoZone { Id = new Guid("0B6E3041-4368-4476-A697-A8BAFC77A9E0"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Virginia", Code = "VA" }); //United States
            list.Add(new GeoZone { Id = new Guid("CFA0C0E5-B478-41BD-9029-49BD04C68871"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Washington", Code = "WA" }); //United States
            list.Add(new GeoZone { Id = new Guid("2282DF69-BCF5-49FE-A6EB-C8C9DEC87A52"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "West Virginia", Code = "WV" }); //United States
            list.Add(new GeoZone { Id = new Guid("B9093677-F26A-4B47-AD98-12CAED313044"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Wisconsin", Code = "WI" }); //United States
            list.Add(new GeoZone { Id = new Guid("EB8EFD2D-B9FA-4F99-9C49-9DEF24CCC5B5"), CountryId = new Guid("A71D6727-61E7-4282-9FCB-526D1E7BC24F"), Name = "Wyoming", Code = "WY" }); //United States

            

            return list;

        }

    }
}
