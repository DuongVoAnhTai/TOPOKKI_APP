using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Controllers;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP
{
    public partial class ProfileForm : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public ProfileForm(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
        }

        void ChangeAccount(Account account)
        {
            txtUserName.Text = loginAccount.UserName;
            txtDisplayName.Text = loginAccount.Name;
        }

        void UpdateAccountInfo()
        {
            string displayName = txtDisplayName.Text;
            string password = txtPassword.Text;
            string newPassword = txtNewPass.Text;
            string confirm = txtConfirm.Text;
            string userName = txtUserName.Text;

            if (!newPassword.Equals(confirm))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu");
            }
            else
            {
                if(AccountController.Instance.UpdateAccount(userName, displayName, password, newPassword))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if(updateAccount != null)
                    {
                        updateAccount(this, new AccountEvent(AccountController.Instance.GetAccountByUserName(userName)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;
        public Account Acc
        {
            get { return acc; }
            set { acc = value; }
        }
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
