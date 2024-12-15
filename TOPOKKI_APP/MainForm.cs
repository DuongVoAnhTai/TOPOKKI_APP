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

namespace TOPOKKI_APP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AdjustUIBasedOnRole();
            openChildForm(new TableFoodForm());
        }

        private Form currentFormChild;
        public void openChildForm(Form childForm)
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
            openChildForm(new TableFoodForm());
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            openChildForm(new StatisticalForm());
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            openChildForm(new AccountForm());
        }

        private void AdjustUIBasedOnRole()
        {
            if(User.Role == Roles.staff)
            {
                btnStatistic.Visible = false;
                btnAccount.Visible = false;
            }
        }
    }
}
