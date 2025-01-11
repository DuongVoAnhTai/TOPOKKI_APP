using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TOPOKKI_APP.Controllers;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP
{
    public partial class AccountForm : Form
    {
        BindingSource accountList = new BindingSource();
        string currentName = "", currentPhone = "";

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
            currentName = txtUserName.Text;
            currentPhone = txtPhoneAccount.Text;
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

            if (IsValid())
            {
                AddAccount(username, displayName, phone, roleID);
            }
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

            if(IsValid(currentName, currentPhone))
            {
                EditAccount(username, displayName, phone, roleID);
                currentName = username; // Cập nhật giá trị hiện tại
                currentPhone = phone;
            }
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

        private void dgvAccount_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAccount.SelectedCells.Count > 0)
            {
                currentName = txtUserName.Text;
                currentPhone = txtPhoneAccount.Text;
            }
        }

        private bool IsValid(string currentName = "", string currentPhone = "")
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                isValid = false;
                errorProvider1.SetError(txtUserName, "Tên đăng nhập không được để trống.");
            }
            else if (txtUserName.Text.Contains(' '))
            {
                isValid = false;
                errorProvider1.SetError(txtUserName, "Tên đăng nhập không được có khoảng trống.");
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtUserName.Text, "^[a-zA-Z0-9]+$"))
            {
                isValid = false;
                errorProvider1.SetError(txtUserName, "Tên đăng nhập chỉ được chứa các ký tự từ a-z và A-Z và số 0-9, không có dấu.");
            }
            else
            {
                // check duplicate username
                string userNameTest = "";

                userNameTest = txtUserName.Text;

                bool test = AccountController.Instance.CheckAccount(userNameTest, currentName);
                if (test)
                {
                    isValid = false;
                    errorProvider1.SetError(txtUserName, "Tên đăng nhập đã tồn tại.");
                }

            }


            if (string.IsNullOrWhiteSpace(txtNameAccount.Text))
            {
                isValid = false;
                errorProvider1.SetError(txtNameAccount, "Tên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(txtPhoneAccount.Text))
            {
                errorProvider1.SetError(txtPhoneAccount, "Vui lòng nhập số điện thoại.");
                isValid = false;
            }
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhoneAccount.Text, @"^\d{10}$"))
            {
                errorProvider1.SetError(txtPhoneAccount, "Số điện thoại phải là 10 chữ số.");
                isValid = false;
            }
            else
            {

                string phoneTest = "";
                phoneTest = txtPhoneAccount.Text;
                bool test = AccountController.Instance.CheckPhone(phoneTest, currentPhone);
                if (test)
                {
                    isValid = false;
                    errorProvider1.SetError(txtPhoneAccount, "Số điện thoại đã tồn tại.");
                }
            }
            return isValid;
        }
    }
}
