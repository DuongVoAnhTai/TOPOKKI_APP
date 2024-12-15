using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP
{
    public partial class AccountForm : Form
    {
        public AccountForm()
        {
            InitializeComponent();
            LoadAccountData();
        }

        private void LoadAccountData()
        {
            try
            {
                using (var context = new TopokkiEntities())
                {
                    var accounts = context.Accounts.ToList();
                    dgvAccount.DataSource = accounts;
                }
            }
            catch
            {

            }
        }
    }
}
