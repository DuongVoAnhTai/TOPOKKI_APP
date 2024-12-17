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
    public partial class AccountForm : Form
    {
        BindingSource accountList = new BindingSource();
        public AccountForm()
        {
            InitializeComponent();
        }

        private void AccountForm_Load(object sender, EventArgs e)
        {
            dgvAccount.DataSource = accountList;
            LoadAccount();
            AddAccountBinding();
        }

        void LoadAccount()
        {
            AccountController.Instance.GetListAccount(accountList);
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Username"));
            txtNameAccount.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Name"));
            txtPhoneAccount.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Phone"));
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
    }
}
