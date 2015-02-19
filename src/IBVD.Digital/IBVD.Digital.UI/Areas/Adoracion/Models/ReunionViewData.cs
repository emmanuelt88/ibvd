using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using System.Collections.ObjectModel;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class ReunionViewData
    {
        public bool ShowCrearButton { get; set; }
        public bool ShowEditarButton { get; set; }
        public bool ShowAnularButton { get; set; }

        public Reunion Reunion { get; private set; }
        
        public ReadOnlyCollection<Usuario> Usuarios { get; private set; }

        public ReunionViewData()
        {
            Reunion = new Reunion();
            Usuarios = new ReadOnlyCollection<Usuario>(new List<Usuario>());
        }

        public ReunionViewData(IList<Usuario> usuarios)
        {
            Reunion = new Reunion();
            Usuarios = new ReadOnlyCollection<Usuario>(usuarios);
        }

        public ReunionViewData(Reunion reunion, IList<Usuario> usuarios)
        {
            Reunion = reunion;
            Usuarios = new ReadOnlyCollection<Usuario>(usuarios);
        }

        public ReunionViewData(Reunion reunion)
        {
            Reunion = reunion;
        }

        public string ItemsReunion { get; set; }

        public bool ExportarArchivos { get; set; }
        
    }
}
