using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    public class ItemComplejo
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Codigo { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Descripcion { get; private set; }

        public ItemComplejo(int id, string codigo, string descripcion)
        {
            this.Id = id;
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }
    }
}
