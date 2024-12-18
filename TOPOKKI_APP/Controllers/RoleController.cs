using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Controllers
{
    internal class RoleController
    {
        private static RoleController instance;

        public static RoleController Instance
        {
            get { if (instance == null) instance = new RoleController(); return instance; }
            set { instance = value; }
        }

        private RoleController() { }

        public List<Role> GetRole()
        {
            using (var context = new TopokkiEntities())
            {
                return context.Roles.ToList();
            }
        }

        public Role GetRoleByID(int id)
        {
            using (var context = new TopokkiEntities())
            {
                var role = context.Roles.FirstOrDefault(r => r.ID == id);
                return role;
            }
        }
    }
}
