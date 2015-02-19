using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using System.Web.Mvc;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using IBVD.Digital.UI.Areas.Adoracion.Configuration.ItemsConfiguration;
using IBVD.Digital.UI.Areas.Adoracion.Configuration.Model;
using IBVD.Digital.Logic.Helper;
using System.IO;
using IBVD.Digital.Logic.Component;


namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class UserViewData
    {
        public string UserFotoURL
        {
            get
            {
                return User.UserFotoURL;
            }
        }
        public Dictionary<string, string> Data { get; set; }
        private IList<KeyValueItem> sexos;
        public bool ShowCrearButton { get; set; }
        /// <summary>
        /// Lista de Sexos
        /// </summary>
        public SelectList Sexos 
        {
            get 
            {
                if (User != null)
                {
                    return new SelectList(sexos, "Value", "Key", User.Sexo);
                }

                return new SelectList(sexos, "Value", "Key", sexos.Single(m => m.Default).Key);
            }
        }

        /// <summary>
        /// Usuario
        /// </summary>
        public Usuario User { get; set; }
        
        /// <summary>
        /// Lista de usuarios
        /// </summary>
        public IList<Usuario> ListUsers { get; set; }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public UserViewData()
        {
            sexos = UIConfigurationHelper.GetSelectListConfigurationByName(ItemsConfigurationEnum.SEXOS);
            User = new Usuario();
            ListUsers = new List<Usuario>();
            Roles = new List<Role>();
        }

        /// <summary>
        /// Listado de Roles
        /// </summary>
        public IList<Role> Roles { get; set; }

        public IList<Operacion> Operaciones { get; set; }

        public List<ItemNode> GetTreeItems()
        {
            return UIHelper.GetTreeItems(Operaciones, User.Individuales);
        }

    }
}
