using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class TableFoodController
    {
        private static TableFoodController instance;
        public static TableFoodController Instance
        {
            get { if(instance == null) instance = new TableFoodController(); return instance; }
            private set { instance = value; }
        }

        private TableFoodController() { }

        public List<TableFood> GetTableList()
        {
            using (var context = new TopokkiEntities())
            {
                return context.TableFoods.ToList();
            }
        }

        public void SwitchTable(int id1, int id2)
        {
            using (var context = new TopokkiEntities())
            {
                // 1. Kiểm tra bàn mới (id2) có trạng thái "Có người" hay không
                var targetTable = context.TableFoods.FirstOrDefault(t => t.ID == id2);
                if (targetTable == null || targetTable.Status == "Có người")
                {
                    MessageBox.Show("Bàn được chọn đã có người. Vui lòng chọn bàn khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Lấy danh sách các Order chưa thanh toán của bàn cũ (id1)
                var ordersToSwitch = context.Orders
                    .Where(o => o.TableID == id1 && o.Status == 0)
                    .ToList();

                if (ordersToSwitch.Count == 0)
                {
                    MessageBox.Show("Bàn hiện tại không có Order nào để chuyển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 3. Cập nhật TableID cho các Order chuyển sang bàn mới
                foreach (var order in ordersToSwitch)
                {
                    order.TableID = id2;
                }

                // 4. Lưu các thay đổi vào database
                context.SaveChanges();

                // 5. Cập nhật trạng thái bàn
                // Bàn mới -> Có người
                targetTable.Status = "Có người";

                // Kiểm tra lại bàn cũ (id1), nếu không còn Order nào thì chuyển về "Trống"
                var tableOld = context.TableFoods.FirstOrDefault(t => t.ID == id1);
                bool hasOrders = context.Orders.Any(o => o.TableID == id1 && o.Status == 0);
                if (!hasOrders && tableOld != null)
                {
                    tableOld.Status = "Trống";
                }

                // Lưu thay đổi trạng thái bàn
                context.SaveChanges();
            }
        }
    }
}
