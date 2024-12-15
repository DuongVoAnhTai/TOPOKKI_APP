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
    public partial class StatisticalForm : Form
    {
        public StatisticalForm()
        {
            InitializeComponent();
            LoadDateTimePicker();
            LoadListOrderByDate(dateTimePicker1.Value, dateTimePicker2.Value);
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

        void LoadListOrderByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvOrder.DataSource = OrderController.Instance.GetOrderListByDate(checkIn, checkOut);
        }

        private void btnStatistical_Click(object sender, EventArgs e)
        {
            LoadListOrderByDate(dateTimePicker1.Value, dateTimePicker2.Value);
        }
    }
}
