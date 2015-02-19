using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using System.Collections.ObjectModel;
using IBVD.Digital.Logic.Entities.Biblia;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class BibliaBuscadorModel
    {
        public IList<Biblia> Biblias { get; private set; }
        public IList<Libro> Libros { get; private set; }

        public BibliaBuscadorModel(IList<Biblia> biblias, IList<Libro> libros)
        {
            Biblias = biblias;
            Libros = libros;
        }

    }
}
