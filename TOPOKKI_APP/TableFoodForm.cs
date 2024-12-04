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

namespace TOPOKKI_APP
{
    public partial class TableFoodForm : Form
    {
        private readonly TableFoodController _controller;
        public TableFoodForm()
        {
            InitializeComponent();
            _controller = new TableFoodController();
        }

        private void TableFoodForm_Load(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void LoadTables()
        {
            // Lấy danh sách bàn từ Controller
            var tables = _controller.GetTableList();

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
                btnName.Click += (s, e) =>
                {
                    MessageBox.Show($"Bạn đã chọn {table.Name}");
                };

                // Thêm panel vào FlowLayoutPanel
                flowLayoutPanel1.Controls.Add(panel);
            }
        }
    }
}
