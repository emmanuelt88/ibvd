using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities.Biblia
{
    public class Versiculo
    {
        public int Numero { get; private set; }
        public string Texto { get; private set; }
        public int Capitulo { get; set; }
        public int Libro { get; set; }
        public string LibroTexto { get; set; }
        public string CodigoBiblia { get; set; }

        public Versiculo(int numero, string texto)
        {
            Numero = numero;
            Texto = texto;
        }

        public Versiculo(string biblia, int idLibro, string libroTexto,int capitulo, int versiculo, string texto)
        {
            this.CodigoBiblia = biblia;
            this.Libro = idLibro;
            this.LibroTexto = libroTexto;
            this.Numero = versiculo;
            this.Texto = texto;
            this.Capitulo = capitulo;
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Numero);
            builder.Append(Capitulo);
            builder.Append(Libro);
            builder.Append(CodigoBiblia);

            return builder.GetHashCode();
        }
    }
}
