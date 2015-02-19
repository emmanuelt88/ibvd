using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class FechasReunionesViewData
    {
        public bool ShowCrearButton { get; set; }
        public IList<Usuario> Usuarios { get; set; }

    }
}
