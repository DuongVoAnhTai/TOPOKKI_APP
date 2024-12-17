using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class ProductForm : Form
    {
        BindingSource productList = new BindingSource();
        public ProductForm()
        {
            InitializeComponent();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            dgvProduct.DataSource = productList;
            LoadListProduct();
            AddFoodBinding();
            LoadCategory();
        }

        void LoadListProduct() 
        {
            ProductController.Instance.GetListProduct(productList);
        }

        void LoadCategory()
        {
            cbProductCategory.DataSource = CategoryController.Instance.GetListCategory();
            cbProductCategory.DisplayMember = "Name";
        }

        void AddFoodBinding()
        {
            //Từ txtName, hãy cho Binding giá trị là Text và Binding sẽ thay đổi giá trị của Name nằm trong cái dgv
            //Tức là khi ấn qua sản phẩm khác thì giá trị sẽ thay đổi ngay lập tức
            txtProductName.DataBindings.Add(new Binding("Text", dgvProduct.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtProductID.DataBindings.Add(new Binding("Text", dgvProduct.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmProductPrice.DataBindings.Add(new Binding("Value", dgvProduct.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)dgvProduct.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                Category category = CategoryController.Instance.GetCategoryByID(id);
                cbProductCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach (Category item in cbProductCategory.Items)
                {
                    if (item.ID == category.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbProductCategory.SelectedIndex = index;
            }
            catch
            {

            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text;
            int categoryID = (cbProductCategory.SelectedItem as Category).ID;
            decimal price = nmProductPrice.Value;

            ProductController.Instance.InsertProduct(name, categoryID, price);
            MessageBox.Show("Thêm món thành công");
            LoadListProduct();
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text;
            int categoryID = (cbProductCategory.SelectedItem as Category).ID;
            decimal price = nmProductPrice.Value;
            int id = Convert.ToInt32(txtProductID.Text);

            ProductController.Instance.UpdateProduct(id, name, categoryID, price);
            MessageBox.Show("Sửa món thành công");
            LoadListProduct();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            LoadListProduct();
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtProductID.Text);

            ProductController.Instance.DeleteProduct(id);
            MessageBox.Show("Xóa món thành công");
            LoadListProduct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ProductController.Instance.SearchProductByName(txtSearch.Text, productList);
        }
    }
}
