using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities.Biblia;

namespace IBVD.Digital.Logic.Entities
{
    public class Pasaje : ItemReunion
    {
        public int Id { get; private set; }
        public Biblia.Biblia Biblia { get; private set; }
        public Libro Libro { get; private set; }
        public Capitulo Capitulo { get; private set; }
        public string PasajeTexto { get; set; }
        public IList<Versiculo> Versiculos {get;private set;}  

        public Pasaje()
        {

        }

        public Pasaje(Biblia.Biblia biblia, Libro libro, Capitulo capitulo, IList<Versiculo> versiculos)
        {
            this.Id = -1;
            this.Biblia = biblia;
            this.Libro = libro;
            this.Capitulo = capitulo;
            this.Versiculos = versiculos;
        }

        public Pasaje(int id, Biblia.Biblia biblia, Libro libro, Capitulo capitulo, IList<Versiculo> versiculos)
        {
            this.Id = id;
            this.Biblia = biblia;
            this.Libro = libro;
            this.Capitulo = capitulo;
            this.Versiculos = versiculos;
        }

        public override int GetId()
        {
            return this.Id;
        }

        public override TipoItemReunion GetTipo()
        {
            return TipoItemReunion.PASAJE;
        }

        protected override void SetCurrentId(int id)
        {
            this.Id = id;
        }

        public override string GetText()
        {
            return string.Format("{0} {1} ({2})",Libro.Descripcion, PasajeTexto, Biblia.Codigo);
        }

        public override string GetDetails()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var item in this.Versiculos)
            {
                builder.AppendFormat("&nbsp;<sub class='NroVersiculo' style='font-weight:bold;'>{0}</sub>&nbsp;<span class='Texto'>{1}</span>. ", item.Numero, item.Texto);
            }

            return builder.ToString();
        }
    }
}
