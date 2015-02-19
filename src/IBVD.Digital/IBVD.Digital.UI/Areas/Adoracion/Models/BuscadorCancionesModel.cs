using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class BuscadorCancionesModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string IdBuscador { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string FuncionEncontrada { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<Cancion> Canciones { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BuscadorCancionesModel()
        {
            Canciones = new List<Cancion>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canciones"></param>
        public BuscadorCancionesModel(IList<Cancion> canciones, string idBuscador, string funcionEncontrada)
        {
            Canciones = canciones;
            IdBuscador = idBuscador;
            FuncionEncontrada = funcionEncontrada;
        }

    }
}
