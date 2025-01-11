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
    public partial class MainForm : Form
    {
        private Account loginAccount;
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(); }
        }

        public MainForm(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
        }

        void ChangeAccount()
        {
            btnProfile.Text = loginAccount.Name;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AdjustUIBasedOnRole();
            OpenChildForm(new TableFoodForm());
        }

        private Form currentFormChild;
        public void OpenChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnBody.Controls.Add(childForm);
            pnBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTableFood_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TableFoodForm());
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ProductForm());
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            OpenChildForm(new StatisticalForm());
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AccountForm());
        }

        private void AdjustUIBasedOnRole()
        {
            if(User.Role == Roles.staff)
            {
                btnStatistic.Visible = false;
                btnAccount.Visible = false;
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm(loginAccount);
            profileForm.UpdateAccount += profileForm_UpdateAccount;
            profileForm.Show();
        }

        private void profileForm_UpdateAccount(object sender, AccountEvent e)
        {
            btnProfile.Text = e.Acc.Name;
        }

        private void btnTableFood_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Gray;
        }

        private void btnTableFood_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnMenu_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Gray;
        }

        private void btnMenu_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnStatistic_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Gray;
        }

        private void btnStatistic_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnAccount_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Gray;
        }

        private void btnAccount_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void btnLogout_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.Gray;
        }

        private void btnLogout_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackColor = Color.FromArgb(64, 64, 64);
        }
    }
}
