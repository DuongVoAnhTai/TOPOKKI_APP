using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class CategoryController
    {
        private static CategoryController instance;

        public static CategoryController Instance
        {
            get { if (instance == null) instance = new CategoryController(); return instance; }
            set { instance = value; }
        }

        private CategoryController() { }

        public List<Category> GetListCategory()
        {
            using (var context = new TopokkiEntities())
            {
                return context.Categories.ToList();
            }
        }
    }
}
