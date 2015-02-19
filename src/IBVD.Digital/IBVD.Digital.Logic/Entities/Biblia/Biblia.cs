using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities.Biblia
{
    public class Biblia
    {
        public string Nombre { get; private set; }
        public IList<Libro> Libros { get; private set; }
        public string Codigo { get; set; }

        public Biblia(string nombre, string codigo)
        {
            Nombre = nombre;
            Libros = new List<Libro>();
            Codigo = codigo;
        }


    }
}
