using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Fault;
using IBVD.Digital.Logic.Entities.Biblia;
using IBVD.Digital.Logic.Component.Mapper;
using System.IO;
using Ionic.Zip;
using System.Web.Hosting;
using IBVD.Digital.IBVD.Cache;
using IBVD.Digital.Logic.Helper;
using System.Transactions;
using IBVD.Digital.Common.Entities;

namespace IBVD.Digital.Logic.Component
{
    public static class BibliasComponent
    {

        public static Biblia GetBibliaDefault()
        {
            throw new NotImplementedException();
        }

        public static IList<Biblia> GetBiblias()
        {
            IList<Biblia> biblias = BibliaMapper.GetBiblias();

            return biblias;
        }

        public static IList<Libro> GetLibros()
        {
            IList<Libro> libros = BibliaMapper.GetLibrosData();

            return libros;
        }


        public static IList<Pasaje> BuscarPasaje(int capituloNro, int libroNro, IList<string> codigoBiblias, IList<int> versiculosList)
        {
            try
            {
                IList<Pasaje> pasajes = BibliaMapper.BuscarPasaje(capituloNro, libroNro, codigoBiblias, versiculosList);

                foreach (var pasaje in pasajes)
                {
                    pasaje.PasajeTexto = string.Format("{0}{1}",capituloNro, pasaje.Versiculos.Count > 0 ? ":" + GenerarCitaVersiculos(pasaje.Versiculos.Select(m=> m.Numero).ToList()) : string.Empty);
                }
                return pasajes;
            }
            catch (Exception ex)
            {
                throw new HandleException("El pasaje es inválido o no fué encontrado",ex);
            }
        }

        public static Libro GetLibro(int nro)
        {
            var libros = BibliaMapper.GetLibrosData();
            var libro = libros.FirstOrDefault(m => m.Numero == nro);

            return libro;
        }

        public static Biblia GetBiblia(string codigoBiblia)
        {
            var biblia = GetBiblias().FirstOrDefault(m => m.Codigo.Equals(codigoBiblia));

            return biblia;
        }

        public static void GenerarPasajeData(string pasaje, out int capitulo, IList<int> versiculosList)
        {
            string[] data = pasaje.Split(':');
            capitulo = int.Parse(data[0]);

            if (data.Count() == 2)
            {
                var listaVersiculos = new List<int>();
                string versiculos = data[1];
                string[] especificos = versiculos.Split(',');

                foreach (var item in especificos)
                {
                    var items = item.Split('-');
                    int min = int.Parse(items[0]);
                    int max = int.Parse(items[0]);

                    if (items.Count() == 2)
                    {
                        max = int.Parse(items[1]);
                    }

                    for (int i = min; i <= max; i++)
                    {
                        if (!versiculosList.Contains(i))
                        versiculosList.Add(i);
                    }
                }
            }

        }

        private static string GenerarCitaVersiculos(IList<int> versiculosList)
        {
            string cita = string.Empty;
            List<int> parcial = new List<int>();
            for (int i = 0; i < versiculosList.Count; i++)
            {
                var numero = versiculosList[i];
                parcial.Add(numero);
                bool addComa = i == versiculosList.Count - 1 || numero != versiculosList[i + 1] - 1;
                int first = parcial.First();
                int last = parcial.Last();

                if (addComa)
                {
                    if (first == last)
                        cita += first.ToString();
                    else
                        cita += string.Format("{0}-{1}", first, last);
                    parcial.Clear();

                    if (addComa && i < versiculosList.Count - 1)
                        cita += ",";
                }


            }

            return cita;

        }
        public static void ImportarBiblia(CacheItem cacheItem)
        {
            string tempFolder = IOHelper.GetDiskLocation(ConfigurationHelper.DirectorioBiblias);
            IOHelper.CheckAndCreateFolder(tempFolder);

            MemoryStream stream = new MemoryStream(cacheItem.Content);

            ZipFile file = ZipFile.Read(stream);

            string fileName = IOHelper.ConbinePath(tempFolder, file.Entries.First().FileName);

            LoggingComponent.LogInfo("Extrayendo biblia en directorio " + tempFolder);
            file.Extract(file.Entries.First().FileName, tempFolder, true);

            LoggingComponent.LogInfo("Comenzando importacion a la base");
            BibliaMapper.ImportarBiblia(fileName);
   
        }


        public static ListCollection<Versiculo> GetVersiculos(Query query)
        {
            ListCollection<Versiculo> versiculos = new ListCollection<Versiculo>();

            IList<Versiculo> result = BibliaMapper.GetVersiculos(query);
            int total = BibliaMapper.GetTotalVersiculos(query);

            versiculos.AddRange(result);
            versiculos.Total = total;

            return versiculos;
        }

        public static IDictionary<string, string> GetBibliasBasico()
        {
            IDictionary<string, string> biblias = BibliaMapper.GetBibliasBasico();

            return biblias;
        }

        public static IDictionary<string, string> GetLibrosBasico()
        {
            IDictionary<string, string> libros = BibliaMapper.GetLibrosBasico();

            return libros;
        }
    }
}
