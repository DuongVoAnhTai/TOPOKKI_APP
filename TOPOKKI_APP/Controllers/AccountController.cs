using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Helpers;
using TOPOKKI_APP.Models.Entities;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                var account = context.Accounts.Include("Role")
                    .FirstOrDefault(a => a.UserName == username && a.Password == password);
                if (account != null && account.Role != null)
                {
                    return account.Role.Name;
                }
                return null;
            }
        }

        public Account GetAccountByUserName(string userName)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.FirstOrDefault(a => a.UserName == userName);
                return account;
            }
        }

        public bool UpdateAccount(string username, string displayName, string password, string newPassword)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.FirstOrDefault(a => a.UserName == username && a.Password == password);
                if (account != null)
                {
                    if (newPassword == null || newPassword == "")
                    {
                        account.Name = displayName;
                    }
                    else
                    {
                        account.Name = displayName;
                        account.Password = newPassword;
                    }
                    context.SaveChanges();
                    return true;
                }
                return false;
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
                        RoleID = a.RoleID
                    }).ToList();
                binding.DataSource = account;
            }
        }

        public void InsertAccount(string username, string displayName, string phone, int roleID)
        {
            using (var context = new TopokkiEntities())
            {
                var account = new Account
                {
                    UserName = username,
                    Password = "1",
                    Name = displayName,
                    Phone = phone,
                    RoleID = roleID
                };
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public void UpdateAccount(string username, string displayName, string phone, int roleID)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.FirstOrDefault(a => a.UserName == username);
                if (account != null)
                {
                    account.Name = displayName;
                    account.Phone = phone;
                    account.RoleID = roleID;

                    context.SaveChanges();
                }
            }
        }

        public void DeleteAccount(string username)
        {
            using (var context = new TopokkiEntities())
            {
                var acount = context.Accounts.FirstOrDefault(a => a.UserName == username);
                if (acount != null)
                {
                    context.Accounts.Remove(acount);
                    context.SaveChanges();
                }
            }
        }

        public void ResetPassword(string username)
        {
            using (var context = new TopokkiEntities())
            {
                var account = context.Accounts.FirstOrDefault(a => a.UserName == username);
                if (account != null)
                {
                    account.Password = "1";

                    context.SaveChanges();
                }
            }
        }

        public bool CheckAccount(string userName, string currentName)
        {
            using (var context = new TopokkiEntities())
            {
                return context.Accounts.Any(a => a.UserName == userName && userName != currentName);
            }
        }

        public bool CheckPhone(string phone, string currentPhone)
        {
            using (var context = new TopokkiEntities())
            {
                return context.Accounts.Any(a => a.Phone == phone && a.Phone != currentPhone);
            }
        }
    }
}
