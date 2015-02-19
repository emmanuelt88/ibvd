using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities.Biblia
{
    public class Capitulo
    {
        public int Numero { get; private set; }
        public IList<Versiculo> Versiculos { get; private set; }

        public Capitulo(int numero)
        {
            Numero = numero;
            Versiculos = new List<Versiculo>();
        }


    }
}
