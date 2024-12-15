using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class ProductController
    {
        private static ProductController instance;

        public static ProductController Instance
        {
            get { if (instance == null) instance = new ProductController(); return instance; }
            set { instance = value; }
        }

        private ProductController() { }

        public List<Product> GetFoodByCategoryID(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var productList = context.Products
                    .Where(p => p.CategoryID == id)
                    .ToList();
                return productList;
            }
        }

        public void GetListProduct(DataGridView gridView)
        {
            using (var context = new TopokkiEntities())
            {
                var product = context.Products.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    CategoryID = p.CategoryID
                }).ToList();
                gridView.DataSource = product;
            };
        }
    }
}
