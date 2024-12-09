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
        }

        private void LoadTables()
        {
            // Lấy danh sách bàn từ Controller
            var tables = TableFoodController.Instance.GetTableList();

            foreach (var table in tables)
            {
                // Tạo một Panel đại diện cho mỗi bàn
                Panel panel = new Panel
                {
                    Width = 130,
                    Height = 100,
                    BackColor = table.Status == "Trống" ? Color.Green : Color.Red
                };

                // Thêm nhãn hiển thị tên bàn
                Button btnName = new Button
                {
                    Text = table.Name,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.White,
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

        }

        void LoadFoodListByCategoryID(int id)
        {

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
            ShowOrder(tableID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            LoadFoodListByCategoryID(id);
        }
    }
}
