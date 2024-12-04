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
        public List<TableFood> GetTableList()
        {
            using (var context = new TopokkiEntities())
            {
                return context.TableFoods.ToList();
            }
        }
    }
}
