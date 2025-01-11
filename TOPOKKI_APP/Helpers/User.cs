using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPOKKI_APP.Helpers
{
    public enum Roles
    {
        admin, staff
    }
    internal class User
    {
        private static Roles role;
        private static string username;
        public static string Name { get; set; }

        public static Roles Role { get => role; private set => role = value; }
        public static string UserName { get => username; private set => username = value; }

        public static void SetRoles(string roleName)
        {
            switch(roleName.ToLower())
            {
                case "admin":
                    role = Roles.admin;
                    break;
                case "staff":
                    role = Roles.staff; 
                    break;
                default:
                    throw new ArgumentException("Invalid role provided.");
            }
        }
        public static void SetUser(string name, string roleName)
        {
            UserName = name;
            SetRoles(roleName);
        }
    }
}
