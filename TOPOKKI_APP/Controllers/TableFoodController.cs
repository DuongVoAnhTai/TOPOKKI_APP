using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
