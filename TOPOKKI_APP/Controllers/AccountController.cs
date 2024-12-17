using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public void GetListAccount(BindingSource binding)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.Select(a => new
                    {
                        UserName = a.UserName,
                        Name = a.Name,
                        Phone = a.Phone,
                        Role = a.Role
                    }).ToList();
                binding.DataSource = account;
            }
        }
    }
}
