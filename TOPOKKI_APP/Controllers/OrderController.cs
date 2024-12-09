using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOKKI_APP.Models;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class OrderController
    {
        private static OrderController instance;

        public static OrderController Instance
        {
            get { if(instance == null) instance = new OrderController(); return instance; }
            set { instance = value; }
        }

        private OrderController() { }

        public List<Menu> GetMenuItem(int id)
        {
            using(var context = new TopokkiEntities())
            {
                var order = context.Orders.FirstOrDefault(o => o.ID == id && o.Status == 0);
                if(order == null)
                {
                    return new List<Menu>();
                }
                var orderDetails = context.OrderDetails.Where(od => od.OrderID == order.ID).ToList();
                var menuItems = orderDetails.Select(od => new Menu
                {
                    Name = context.Products.FirstOrDefault(p => p.ID == od.ProductID).Name,
                    Quantity = od.Quantity,
                    Price = context.Products.FirstOrDefault(p => p.ID == od.ProductID).Price
                }).ToList();

                return menuItems;
            }
        }
    }
}
