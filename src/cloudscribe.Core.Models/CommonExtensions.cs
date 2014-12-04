using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models
{
    public static class CommonExtensions
    {
        public static string ToInvariantString(this int i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static string ToInvariantString(this float i)
        {
            return i.ToString(CultureInfo.InvariantCulture);

        }

        public static List<string> SplitOnChar(this string s, char c)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(s)) { return list; }

            string[] a = s.Split(c);
            foreach (string item in a)
            {
                if (!string.IsNullOrEmpty(item)) { list.Add(item); }
            }


            return list;
        }

        public static bool IsDeletable(this ISiteRole role, string undeletableRolesSemiColonSeparated)
        {
            List<string> rolesThatCannotBeDelete = undeletableRolesSemiColonSeparated.SplitOnChar(';');
            return role.IsDeletable(rolesThatCannotBeDelete);
        }

        public static bool IsDeletable(this ISiteRole role, List<string> rolesThatCannotBeDeleted)
        {
            if (role.RoleName == "Admins") { return false; }
            if (role.RoleName == "Content Administrators") { return false; }
            if (role.RoleName == "Authenticated Users") { return false; }
            if (role.RoleName == "Role Admins") { return false; }

            if (rolesThatCannotBeDeleted != null)
            {
                foreach (string roleName in rolesThatCannotBeDeleted)
                {
                    if (role.RoleName == roleName) { return false; }
                    if (role.DisplayName == roleName) { return false; }
                }
            }

            return true;
        }
    }
}
