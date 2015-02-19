using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Helper;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class DirectorioViewData
    {
        public IList<string> Carpetas { get; set; }
        public IList<string> Imagenes { get; set; }
        public string Root { get; set; }
        public bool ModoEdit { get; set; }
        public string CurrentFolder { get; set; }
        public DirectorioViewData()
        {
            Carpetas = new List<string>();
            Imagenes = new List<string>();
        }

        public DirectorioViewData(IList<string> carpetas, IList<string> imagenes, string root)
        {
            Carpetas = carpetas;
            Imagenes = imagenes;
            Root = root.Replace("~", string.Empty);
        }

        public IDictionary<string, IList<string>> ObtenerArchivosPorDirectorio()
        {
            IDictionary<string, IList<string>> data = new Dictionary<string, IList<string>>();
            
            data.Add(ConfigurationHelper.DirectorioImagenesRoot.Replace("~", string.Empty), new List<string>());
            if (!string.IsNullOrEmpty(CurrentFolder))
            {
                data.Add(CurrentFolder, Imagenes.Where(m=> m.StartsWith(CurrentFolder)).ToList());
            }
            else
            {
                foreach (var carpeta in Carpetas)
                {
                    data.Add(carpeta, new List<string>());
                }

                foreach (var imagen in Imagenes)
                {
                    var items = imagen.Split('/');
                    var name = items[items.Length - 1];
                    var carpeta = string.Empty;

                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        carpeta += "/" + items[i];
                    }

                    IList<string> lista = new List<string>();
                    if (data.ContainsKey(carpeta))
                    {
                        lista = data[carpeta];
                    }
                    else
                    {
                        data.Add(carpeta, lista);
                    }

                    lista.Add(name);
                }
            }
          

            return data;
        }
    }
}
