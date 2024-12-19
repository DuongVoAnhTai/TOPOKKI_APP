using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Controllers;

namespace TOPOKKI_APP
{
    public partial class StatisticalForm : Form
    {
        private int CurrentPage = 1; // Trang hien tai
        private int PageSize = 10; // So luong dong cua moi trang
        private int TotalPages = 0; // Tong so trang
        public StatisticalForm()
        {
            InitializeComponent();
            LoadDateTimePicker();
            LoadData();
        }

        void LoadData()
        {
            TotalPages = OrderController.Instance.CaculateTotalPages(dateTimePicker1.Value, dateTimePicker2.Value, PageSize);
            LoadListOrderByDate(dateTimePicker1.Value, dateTimePicker2.Value, CurrentPage, PageSize);
            txtPage.Text = $"{CurrentPage}/{TotalPages}";
        }
        void LoadDateTimePicker()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd 'tháng' MM 'năm' yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd 'tháng' MM 'năm' yyyy";

            DateTime today = DateTime.Now;
            dateTimePicker1.Value = new DateTime(today.Year, today.Month, 1);
            dateTimePicker2.Value = dateTimePicker1.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListOrderByDate(DateTime checkIn, DateTime checkOut, int currentPage, int pageSize)
        {
            dgvOrder.DataSource = OrderController.Instance.GetOrderListByDate(checkIn, checkOut, currentPage, pageSize);
        }

        private void btnStatistical_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            LoadData();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            LoadData();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = TotalPages;
            LoadData();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Lấy tháng và năm từ DateTimePicker
            int month = dateTimePicker1.Value.Month;
            int year = dateTimePicker1.Value.Year;

            // Lấy dữ liệu từ OrderController
            DataTable orders = OrderController.Instance.GetOrdersByMonth(month, year);

            // Kiểm tra dữ liệu có hay không
            if (orders.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu đơn hàng cho tháng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Đường dẫn thư mục gốc
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;

            // Tạo thư mục OutputFile
            string outputFolder = Path.Combine(projectDirectory, "OutputFile");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Đường dẫn file Excel
            string fileName = $"BaoCao_Thang_{month}_{year}.xlsx";
            string filePath = Path.Combine(outputFolder, fileName);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add($"Order_{month}_{year}");

                // Thêm tiêu đề cột
                for (int col = 0; col < orders.Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = orders.Columns[col].ColumnName;
                    worksheet.Cells[1, col + 1].Style.Font.Bold = true; // In đậm tiêu đề
                }

                // Thêm dữ liệu vào file Excel
                for (int row = 0; row < orders.Rows.Count; row++)
                {
                    for (int col = 0; col < orders.Columns.Count; col++)
                    {
                        object value = orders.Rows[row][col];

                        // Kiểm tra và format ngày tháng
                        if (value is DateTime)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = ((DateTime)value).ToString("dd/MM/yyyy HH:mm");
                            worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                        }
                        else
                        {
                            worksheet.Cells[row + 2, col + 1].Value = value;
                        }
                    }
                }

                // Tự động điều chỉnh kích thước cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Lưu file Excel
                package.SaveAs(new FileInfo(filePath));
                MessageBox.Show($"Báo cáo đã được xuất ra file {filePath}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
