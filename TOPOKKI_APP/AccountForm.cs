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
            LoadRole();
        }

        void LoadAccount()
        {
            AccountController.Instance.GetListAccount(accountList);
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Username", true, DataSourceUpdateMode.Never)); // ko cho du lieuj tu txb chuyen lai dgv
            txtNameAccount.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtPhoneAccount.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "Phone", true, DataSourceUpdateMode.Never));
        }

        void LoadRole()
        {
            cbRoleAccount.DataSource = RoleController.Instance.GetRole();
            cbRoleAccount.DisplayMember = "Name";
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)dgvAccount.SelectedCells[0].OwningRow.Cells["RoleID"].Value;
                Role role = RoleController.Instance.GetRoleByID(id);
                cbRoleAccount.SelectedItem = role;

                int index = -1;
                int i = 0;
                foreach (Role item in cbRoleAccount.Items)
                {
                    if (item.ID == role.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbRoleAccount.SelectedIndex = index;
            }
            catch
            {

            }
        }

        void AddAccount(string username, string displayName, string phone, int roleID)
        {
            AccountController.Instance.InsertAccount(username, displayName, phone, roleID);
            MessageBox.Show("Thêm tài khoản thành công");
            LoadAccount();
        }

        void EditAccount(string username, string displayName, string phone, int roleID)
        {
            AccountController.Instance.UpdateAccount(username, displayName, phone, roleID);
            MessageBox.Show("Cập nhật tài khoản thành công");
            LoadAccount();
        }

        void DeleteAccount(string username)
        {
            AccountController.Instance.DeleteAccount(username);
            MessageBox.Show("Xóa tài khoản thành công");
            LoadAccount();
        }

        void ResetPassword(string username)
        {
            AccountController.Instance.ResetPassword(username);
            MessageBox.Show("Đặt lai mật khẩu thành công");
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string displayName = txtNameAccount.Text;
            string phone = txtPhoneAccount.Text;
            int roleID = (cbRoleAccount.SelectedItem as Role).ID;

            AddAccount(username, displayName, phone, roleID );
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;

            DeleteAccount(username);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string displayName = txtNameAccount.Text;
            string phone = txtPhoneAccount.Text;
            int roleID = (cbRoleAccount.SelectedItem as Role).ID;

            EditAccount(username, displayName, phone, roleID );
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnPassAccount_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;

            ResetPassword(username);
        }
    }
}
