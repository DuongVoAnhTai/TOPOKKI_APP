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
using TOPOKKI_APP.Helpers;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            string role = AccountController.Instance.CheckLogin(username, password);
            if (role != null)
            {
                Account loginAccount = AccountController.Instance.GetAccountByUserName(username);
                User.SetUser(username, role);
                User.Name = loginAccount.Name;
                MainForm mainForm = new MainForm(loginAccount);
                this.Hide();
                this.Show();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn muón thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
