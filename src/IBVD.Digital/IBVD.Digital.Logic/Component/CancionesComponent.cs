using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities;

using IBVD.Digital.Common.Fault;
using IBVD.Digital.Common.Entities;
using IBVD.Digital.Logic.Component.Mapper;
using IBVD.Digital.IBVD.Cache;
using System.Xml;
using IBVD.Digital.Logic.Helper;
using System.IO;
using System.Xml.XPath;
using System.Web.Mvc;
using IBVD.Digital.Common.Helpers;

namespace IBVD.Digital.Logic.Component
{
    public static class CancionesComponent
    {
        public static ListCollection<Cancion> GetCanciones(Query query)
        {
            ListCollection<Cancion> canciones = new ListCollection<Cancion>();

            IList<Cancion> result = CancionesMapper.GetCanciones(query);
            int total = CancionesMapper.GetTotalCanciones(query);

            canciones.AddRange(result);
            canciones.Total = total;

            return canciones;
        }

        public static Cancion GetCancion(int idCancion)
        {
            Cancion cancion = CancionesMapper.GetCancion(idCancion);

            return cancion;
        }

        public static void SaveCancion(Cancion cancion)
        {
            CancionesMapper.Save(cancion);
        }

        public static int CreateCancion(Cancion cancion)
        {
            return CancionesMapper.Create(cancion);
        }


        public static int ImportarCanciones(CacheItem documento, bool reemplazarExistentes)
        {
            
            MemoryStream file = new MemoryStream(documento.Content, false);
            var settings = new XmlReaderSettings();
            var xmlReader = XmlTextReader.Create(file, settings);
            XPathDocument xPath = new XPathDocument(xmlReader);
            var navigator = xPath.CreateNavigator();
            
            XmlDocument xmldocument = new XmlDocument();
            xmldocument.InnerXml = navigator.OuterXml;

            List<Cancion> canciones =  XMLGenerator.ImportarCancionesDesdeXml(xmldocument);

            foreach (var item in canciones)
            {
                int idCancion = CancionesComponent.GetIdCancion(item.Titulo);
                if (idCancion > 0 && reemplazarExistentes)
                {
                    Cancion cancion = new Cancion(idCancion, item.Titulo, item.Tono, item.Compas, item.Letra, true,item.DuracionEstimada, string.Empty);

                    CancionesComponent.SaveCancion(cancion);
                }
                else
                {
                    CancionesComponent.CreateCancion(item);
                }
            }
            
            return canciones.Count;

        }

        private static int GetIdCancion(string titulo)
        {
            int idCancion = CancionesMapper.GetIdCancion(titulo);

            return idCancion;
        }

        public static IList<Cancion> GetCanciones()
        {
            IList<Cancion> canciones = (IList<Cancion>)CacheHelper.GetFromCacheHelper("CancionesBusqueda");

            if(canciones != null)
                return canciones;

            Query query = new Query("Id", "asc",string.Empty);
            query.Paginate = false;
            var list = GetCanciones(query);
            canciones = list.ToList();

            CacheHelper.AddToCacheHelper(canciones, 24 * 30 * 60, "CancionesBusqueda");
            return canciones;
        }

        public static System.Web.Mvc.FileContentResult GenerarCancionesXML(IList<Cancion> canciones)
        {
            MemoryStream fileXmlCanciones = new MemoryStream();
            var xmlDocument = XMLGenerator.GenerarXmlCancionesExport(canciones);
            xmlDocument.Save(fileXmlCanciones);

            FileContentResult file = new FileContentResult(fileXmlCanciones.ToArray(), "application/xml");
            file.FileDownloadName = string.Format("IBVD_ExportCanciones_{0}.xml", DateTime.Now.ToString("dd-MM-yyyy hh:mm"));
            return file;
        }

        /// <summary>
        /// Busca las canciones por letra, o titulo
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static IList<Cancion> Buscar(string texto, int max)
        {
            if (max == 0)
                return GetCanciones().Where(m => m.Match(texto)).ToList();
            else
                return GetCanciones().Where(m => m.Match(texto)).Take(max).ToList();
        }
    }
}
