using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities.Biblia;

namespace IBVD.Digital.Logic.Entities.Biblia
{
    public class Libro
    {
        public int Numero { get; set; }
        public string Descripcion { get; set; }

        public IList<Capitulo> Capitulos { get; private set; }
        public Libro()
        {
            Capitulos = new List<Capitulo>();
        }

        public Libro(int numero,string descripcion)
        {
            this.Numero = numero;
            this.Descripcion = descripcion;
            Capitulos = new List<Capitulo>();
        }
    }
}
