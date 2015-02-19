using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemLibre : ItemReunion
    {
        public int Index { get; private set; }
        public int IdReunion { get; private set; }
        public string Titulo { get; private set; }
        public string Texto { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="idReunion"></param>
        /// <param name="titulo"></param>
        /// <param name="texto"></param>
        public ItemLibre(int index, int idReunion, string titulo, string texto)
        {
            this.Index = index;
            this.IdReunion = IdReunion;
            this.Titulo = titulo;
            this.Texto = texto;
        }

        public override int GetId()
        {
            return Index;
        }

        public override TipoItemReunion GetTipo()
        {
            return TipoItemReunion.ITEM_LIBRE;
        }

        protected override void SetCurrentId(int id)
        {
            this.Index = id;
        }


        public override string GetText()
        {
            return this.Titulo;
        }

        public override string GetDetails()
        {
            return this.Texto;
        }
    }
}
