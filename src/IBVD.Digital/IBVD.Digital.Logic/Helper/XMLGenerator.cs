using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Fault;

namespace IBVD.Digital.Logic.Helper
{
    /// <summary>
    /// Generador de XML
    /// </summary>
    public class XMLGenerator
    {
        /// <summary>
        /// Genera un documento xml a partir de una lista de canciones.
        /// </summary>
        /// <param name="canciones">Lista de canciones.</param>
        /// <returns>Documento XML que contiene las canciones.</returns>
        public static XmlDocument GenerarXmlCancionesExport(IList<Cancion> canciones)
        {
            XmlDocument documento = new XmlDocument();
            documento.AppendChild(documento.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlNode easiSlides = documento.CreateElement("EasiSlides");
            documento.AppendChild(easiSlides);
            XmlNode item = null;

            foreach (Cancion cancion in canciones)
            {
                item = documento.CreateElement("Item");

                easiSlides.AppendChild(item);

                XmlNode Title1 = documento.CreateElement("Title1");
                Title1.InnerText = cancion.Titulo;
                XmlNode Title2 = documento.CreateElement("Title2");
                Title2.InnerText = cancion.Titulo;

                XmlNode Folder = documento.CreateElement("Folder");
                Folder.InnerText = "IBVD STARTED";
                XmlNode SongNumber = documento.CreateElement("SongNumber");
                SongNumber.InnerText = cancion.Id.ToString();
                XmlNode Contents = documento.CreateElement("Contents");
                Contents.InnerText = cancion.Letra;
                XmlNode Notations = documento.CreateElement("Notations");
                XmlNode Sequence = documento.CreateElement("Sequence");
                XmlNode Writer = documento.CreateElement("Writer");
                XmlNode Copyright = documento.CreateElement("Copyright");
                XmlNode Category = documento.CreateElement("Category");
                XmlNode Timing = documento.CreateElement("Timing");

                if (!string.IsNullOrEmpty(cancion.Compas))
                {
                    Timing.InnerText = cancion.Compas;
                }
                XmlNode MusicKey = documento.CreateElement("MusicKey");
                if (!string.IsNullOrEmpty(cancion.Tono))
                {
                    MusicKey.InnerText = cancion.Tono;
                }

                XmlNode Capo = documento.CreateElement("Capo");
                XmlNode LicenceAdmin1 = documento.CreateElement("LicenceAdmin1");
                XmlNode LicenceAdmin2 = documento.CreateElement("LicenceAdmin2");
                XmlNode BookReference = documento.CreateElement("BookReference");
                XmlNode UserReference = documento.CreateElement("UserReference");
                XmlNode FormatData = documento.CreateElement("FormatData");

                string root = ConfigurationHelper.DirectorioImagenesRoot;
                string fotoURI = string.IsNullOrEmpty(cancion.FotoURI) ? "Default.jpg" : cancion.FotoURI.Replace(root, string.Empty).Replace("/","\\");

                FormatData.InnerText = cancion.FormatData.Replace("{IMAGEN}", fotoURI);
                XmlNode Settings = documento.CreateElement("Settings");

                item.AppendChild(Title1);
                item.AppendChild(Title2);
                item.AppendChild(Folder);
                item.AppendChild(SongNumber);
                item.AppendChild(Contents);
                item.AppendChild(Notations);
                item.AppendChild(Sequence);
                item.AppendChild(Writer);
                item.AppendChild(Copyright);
                item.AppendChild(Category);
                item.AppendChild(Timing);
                item.AppendChild(MusicKey);
                item.AppendChild(Capo);
                item.AppendChild(LicenceAdmin1);
                item.AppendChild(LicenceAdmin2);
                item.AppendChild(BookReference);
                item.AppendChild(UserReference);
                item.AppendChild(FormatData);
                item.AppendChild(Settings);
            }


            return documento;

        }
        /// <summary>
        /// Genera una lista de canciones a partir de un fichero xml.
        /// </summary>
        /// <param name="documento">Documento xml que contiene las canciones.</param>
        /// <returns>Lista de canciones.</returns>
        public static List<Cancion> ImportarCancionesDesdeXml(XmlDocument documento)
        {
            List<Cancion> canciones = new List<Cancion>();

            foreach (XmlNode nodo in documento.GetElementsByTagName("Item"))
            {
                try
                {
                    Cancion cancion = new Cancion()
                    {
                        Titulo = nodo["Title1"].InnerText,
                        Letra = nodo["Contents"].InnerText,
                        Habilitado = true,
                        Tono = nodo["MusicKey"] == null? string.Empty: nodo["MusicKey"].InnerText,
                        Compas = nodo["MusicKey"] == null ? string.Empty : nodo["Timing"].InnerText,
                        FormatData = ConfigurationHelper.Cancion_FormatData
                    };

                canciones.Add(cancion);

                }
                catch (Exception)
                {
                    throw new HandleException("Los datos de importación son inválidos");
                }
            }

            return canciones;
        }
        /// <summary>
        /// Genera la lista de canciones para importar al Easislides
        /// </summary>
        /// <param name="lista">Lista de canciones para generar el XML</param>
        public static XmlDocument GenerarListaCancionesESW(List<Cancion> canciones)
        {

            XmlDocument documento = new XmlDocument();
            documento.AppendChild(documento.CreateXmlDeclaration("1.0", "utf-8", null));

            XmlNode easiSlides = documento.CreateElement("EasiSlides");
            documento.AppendChild(easiSlides);
            XmlNode item = null;
            XmlNode listItem = null;

            foreach (Cancion cancion in canciones)
            {
                item = documento.CreateElement("Item");
                listItem= documento.CreateElement("ListItem");
                listItem.AppendChild(item);

                easiSlides.AppendChild(listItem);


                XmlNode ItemID = documento.CreateElement("ItemID");
                ItemID.InnerText = cancion.Id.ToString();
                XmlNode Title1 = documento.CreateElement("Title1");
                Title1.InnerText = cancion.Titulo;
                XmlNode Title2 = documento.CreateElement("Title2");
                Title2.InnerText = cancion.Titulo;
                XmlNode FormatData = documento.CreateElement("FormatData");

                item.AppendChild(ItemID);
                item.AppendChild(Title1);
                item.AppendChild(Title2);
                item.AppendChild(FormatData);
            }

            return documento;

        }
    }
}
