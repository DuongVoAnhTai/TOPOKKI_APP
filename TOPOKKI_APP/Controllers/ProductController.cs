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

        public void GetListProduct(BindingSource binding)
        {
            using (var context = new TopokkiEntities())
            {
                var product = context.Products.Select(p => new
                {
                    ID = p.ID,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryID = p.CategoryID
                }).ToList();
                binding.DataSource = product;
            };
        }

        public void InsertProduct(string name, int categoryID, decimal price)
        {
            using (var context = new TopokkiEntities())
            {
                var product = new Product
                {
                    CategoryID = categoryID,
                    Name = name,
                    Price = price,
                };
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public void UpdateProduct(int productID, string name, int categoryID, decimal price)
        {
            using (var context = new TopokkiEntities())
            {
                var product = context.Products.FirstOrDefault(p => p.ID == productID);
                if(product != null)
                {
                    product.Name = name;
                    product.Price = price;
                    product.CategoryID = categoryID;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteProduct(int productID)
        {
            using (var context = new TopokkiEntities())
            {
                OrderController.Instance.DeleteOrderDetailByProductID(productID);
                var product = context.Products.FirstOrDefault(p => p.ID == productID);
                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
            }
        }

        public void SearchProductByName(string name, BindingSource binding)
        {
            using (var context = new TopokkiEntities())
            {
                var product = context.Products.Where(p => p.Name.Contains(name.ToLower()))
                    .Select(p => new
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Price = p.Price,
                        CategoryID = p.CategoryID
                    }).ToList();
                binding.DataSource = product;
            }
        } 
    }
}
