using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.UI.Areas.Adoracion.Helpers.Session;
using IBVD.Digital.Logic.Component;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    /// <summary>
    /// Representa el set de datos necesarios para el render de la vista
    /// </summary>
    public class RoleViewData
    {
        /// <summary>
        /// Role
        /// </summary>
        public Role Role { get; set; }
        ///// <summary>
        ///// Lista de roles
        ///// </summary>
        //public IList<Role> ListRoles { get; set; }
        /// <summary>
        /// Lista de Operaciones
        /// </summary>
        public IList<Operacion> Operaciones { get; set; }


        public List<ItemNode> GetTreeItems()
        {
            return UIHelper.GetTreeItems(Operaciones, Role.Operaciones);
        }
    }
}
