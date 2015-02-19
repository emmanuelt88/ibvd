using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class UsuariosUCViewData
    {
        public IList<Usuario> Usuarios { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public bool Obligatorio { get; set; }

    }
}
