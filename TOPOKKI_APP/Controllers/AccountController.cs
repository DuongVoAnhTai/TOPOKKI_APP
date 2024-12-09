using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class AccountController
    {
        private static AccountController instance;

        public static AccountController Instance 
        {
            get { if (instance == null) instance = new AccountController(); return instance; }
            set { instance = value; }
        }

        private AccountController() { }

        public string CheckLogin(string username, string password)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.
                    FirstOrDefault(a => a.UserName == username && a.Password == password);
                if (account != null)
                {
                    return account.Role;
                }
                return null;
            }
        }
    }
}
