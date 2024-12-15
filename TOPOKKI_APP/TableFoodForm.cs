using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Controllers;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP
{
    public partial class TableFoodForm : Form
    {
        public TableFoodForm()
        {
            InitializeComponent();
        }

        private void TableFoodForm_Load(object sender, EventArgs e)
        {
            LoadTables();
            LoadCategory();
            LoadEmptyTable();
        }

        private void LoadTables()
        {
            flowLayoutPanel1.Controls.Clear();
            // Lấy danh sách bàn từ Controller
            var tables = TableFoodController.Instance.GetTableList();

            foreach (var table in tables)
            {
                // Tạo một Panel đại diện cho mỗi bàn
                Panel panel = new Panel
                {
                    Width = 120,
                    Height = 100,
                    BackColor = table.Status == "Trống" ? Color.Cyan : Color.Pink
                };

                // Thêm nhãn hiển thị tên bàn
                Button btnName = new Button
                {
                    Text = table.Name,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Black,
                    Cursor = Cursors.Hand
                };
                panel.Controls.Add(btnName);

                // Thêm sự kiện click cho panel (nếu cần)
                btnName.Click += btn_Click;
                btnName.Tag = table;

                // Thêm panel vào FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(panel);
            }
        }

        void LoadCategory()
        {
            var category = CategoryController.Instance.GetListCategory();
            cbCategory.DataSource = category;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            var productList = ProductController.Instance.GetFoodByCategoryID(id);
            cbProduct.DataSource = productList;
            cbProduct.DisplayMember = "Name";
        }

        void LoadEmptyTable()
        {
            cbSwitchTable.DataSource = TableFoodController.Instance.GetTableList();
            cbSwitchTable.DisplayMember = "Name";
        }

        void ShowOrder(int id)
        {
            var menuItems = OrderController.Instance.GetMenuItem(id);
            decimal totalPrice = 0;
            lsvOrder.Items.Clear();

            foreach (var item in menuItems)
            {
                ListViewItem lsvItem = new ListViewItem(item.Name.ToString());
                lsvItem.SubItems.Add(item.Quantity.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.Total.ToString());
                totalPrice += item.Total;
                lsvOrder.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            Thread.CurrentThread.CurrentCulture = culture;
            txtTotalPrice.Text = totalPrice.ToString("c");
        }

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as TableFood).ID;
            lsvOrder.Tag = (sender as Button).Tag;
            ShowOrder(tableID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox; // Ép kiểu sender thành ComboBox
            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category; // Lấy item đang chọn và ép kiểu về Category
            id = selected.ID; // Lấy ID từ đối tượng Category

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            // Lay table
            TableFood table = lsvOrder.Tag as TableFood;
            //Lay id cua order dua vao id cua table
            int orderID = OrderController.Instance.GetUncheckOrderByTableID(table.ID);
            int productID = (cbProduct.SelectedItem as Product).ID;
            int quantity = (int)nmFoodCount.Value;

            if(orderID == -1) // Order chua ton tai
            {
                OrderController.Instance.InsertOrder(table.ID);
                OrderController.Instance.InsertOrderDetail(OrderController.Instance.GetMaxOrderID(), productID, quantity);
            }
            else //Order da ton tai
            {
                OrderController.Instance.InsertOrderDetail(orderID, productID, quantity);
            }

            ShowOrder(table.ID);
            LoadTables();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            TableFood table = lsvOrder.Tag as TableFood;

            int orderID = OrderController.Instance.GetUncheckOrderByTableID(table.ID);
            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0]);
            DialogResult result = MessageBox.Show("Bạn có chắc thanh toán hóa đơn cho bàn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel);

            if (orderID != -1)
            {
                if(result == DialogResult.OK)
                {
                    OrderController.Instance.CheckOut(orderID, (decimal)totalPrice);
                    ShowOrder(table.ID);
                    LoadTables();
                } 
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvOrder.Tag as TableFood).ID;
            int id2 = (cbSwitchTable.SelectedItem as TableFood).ID;
            DialogResult result = MessageBox.Show(String.Format("Bạn có chắc muốn chuyển bàn {0} qua bàn {1} không ?", (lsvOrder.Tag as TableFood).Name, (cbSwitchTable.SelectedItem as TableFood).Name ), "Thông báo", MessageBoxButtons.OKCancel);

            if(result == DialogResult.OK)
            {
                TableFoodController.Instance.SwitchTable(id1, id2);
                LoadTables();
            }
        }
    }
}
