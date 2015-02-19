using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities.Biblia;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using IBVD.Digital.Common.Fault;
using IBVD.Digital.Logic.Entities;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using IBVD.Digital.Common.Entities;

namespace IBVD.Digital.Logic.Component.Mapper
{
    internal static class BibliaMapper
    {
        private static Database db = DatabaseFactory.CreateDatabase("DB");


        internal static IList<Biblia> GetBiblias()
        {
            IList<Biblia> biblias = new List<Biblia>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetBiblias");

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    BuildBiblias(reader, biblias);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las biblias en la base de datos: BibliaMapper.GetBiblias", ex);
            }
            
            return biblias;
        }

        private static void BuildBiblias(IDataReader reader, IList<Biblia> biblias)
        {
            while (reader.Read())
            {
                string bibliaNombre = (string)reader["Nombre"];
                int nroLibro = (int)reader["NroLibro"];
                string libroNombre = (string)reader["Libro"];
                int versiculo = (int)reader["Versiculo"];
                string texto = (string)reader["Texto"];
                int capituloNro = (int)reader["Capitulo"];
                string codigo = (string)reader["Codigo"];
                Biblia biblia = biblias.FirstOrDefault(m => m.Codigo == codigo);

                if (biblia == null)
                {
                    biblia = new Biblia(bibliaNombre, codigo);
                    biblias.Add(biblia);
                }

                var libro = biblia.Libros.FirstOrDefault(m => m.Numero == nroLibro);

                if (libro == null)
                {
                    libro = new Libro(nroLibro, libroNombre);
                    biblia.Libros.Add(libro);
                }

                var capitulo = libro.Capitulos.FirstOrDefault(m => m.Numero == capituloNro);

                if (capitulo == null)
                {
                    capitulo = new Capitulo(capituloNro);
                    libro.Capitulos.Add(capitulo);
                }

                capitulo.Versiculos.Add(new Versiculo(versiculo, texto));
            }
        }



        internal static IList<Libro> GetLibrosData()
        {
            IList<Libro> libros = new List<Libro>();

            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetLibrosData");

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        libros.Add(new Libro((int)reader["NroLibro"], (string)reader["Libro"]));
                    }

                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener los datos de los libros en la base de datos: BibliaMapper.GetLibrosData", ex);
            }

            return libros;
        }

        internal static void CrearPasaje(Pasaje pasaje, int idReunion)
        {
            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_CrearPasaje");

                db.AddInParameter(cmd, "Index", DbType.Int32, pasaje.GetId());
                db.AddInParameter(cmd, "CodigoBiblia", DbType.String, pasaje.Biblia.Codigo);
                db.AddInParameter(cmd, "Libro", DbType.Int32, pasaje.Libro.Numero);
                db.AddInParameter(cmd, "Pasaje", DbType.String, pasaje.PasajeTexto);
                db.AddInParameter(cmd, "ReunionId", DbType.Int32, idReunion);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear el pasaje en la base de datos: BibliaMapper.GetLibrosData", ex);
            }

        }

        internal static void ImportarBiblia(string fileName)
        {
            try
            {
                String myConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;"
       + "Data Source={0}";
          
            
            OleDbConnection cnn = new OleDbConnection(string.Format(myConnectionString, fileName));
            string nombre, codigo;
            string cmdNombres = @"select bible.bibletext,bible.verse from bible where bible.book = 0 and bible.chapter = 0 and bible.verse = 0
                                        UNION select bible.bibletext,bible.verse from bible where bible.book = 0 and bible.chapter = 0 and bible.verse = 1
                                        order by bible.verse";

            string cmdVersiculos = @"select bible.book, bible.chapter,bible.verse, bible.bibletext  from bible
                                    where bible.book > 0 
                                    order by bible.book, bible.chapter, bible.verse";
            OleDbCommand cmd = new OleDbCommand(@cmdNombres, cnn);

            cnn.Open();
            LoggingComponent.LogInfo("Iniciando conexion al archivo MDB");
            using (IDataReader reader = cmd.ExecuteReader())
            {
                LoggingComponent.LogInfo("Lectura de datos de la biblia");
                reader.Read();
                nombre = (string)reader["bibletext"];
                reader.Read();
                codigo = (string)reader["bibletext"];

                CrearBiblia(codigo, nombre);
               
            }
            LoggingComponent.LogInfo("Cerrando conexion al archivo MDB");
            cnn.Close();

            

            List<Versiculo> versiculos = new List<Versiculo>();

            cmd = new OleDbCommand(string.Format(cmdVersiculos, codigo), cnn);

            cnn.Open();
            LoggingComponent.LogInfo("Iniciando conexion al archivo MDB");
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    
                    int libro = int.Parse(reader["book"].ToString());
                    int capitulo = int.Parse(reader["chapter"].ToString());
                    int versiculo = int.Parse(reader["verse"].ToString());
                    string texto = (string)reader["bibletext"];

                    versiculos.Add(new Versiculo(versiculo, texto) { Capitulo = capitulo, CodigoBiblia = codigo, Libro = libro });
                    
                }
            }
            cnn.Close();
            LoggingComponent.LogInfo("Iniciando conexion al archivo MDB");
            LoggingComponent.LogInfo(string.Format("Se importaran {0} registros", versiculos.Count));
            CrearVersiculos(versiculos);

            }
            catch (Exception ex)
            {
                throw new HandleException("Se produjo un error al importar la biblia", ex);
            }

            
        }

        internal static void CrearVersiculos(List<Versiculo> versiculos)
        {
            try
            {
                //SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
                

                
                //SqlCommand cmd = null;
                //LoggingComponent.LogInfo("Iniciando conexion a la base de datos para guardar los versiculos");
                //cnn.Open();
                foreach (var item in versiculos)
                {
                    var cmd = db.GetStoredProcCommand("IBVD_CrearVersiculo");

                    db.AddInParameter(cmd, "CodigoBiblia", DbType.String, item.CodigoBiblia);
                    db.AddInParameter(cmd, "Libro", DbType.Int32, item.Libro);
                    db.AddInParameter(cmd, "Capitulo", DbType.Int32, item.Capitulo);
                    db.AddInParameter(cmd, "Versiculo", DbType.Int32, item.Numero);
                    db.AddInParameter(cmd, "Texto", DbType.String, item.Texto);


                    db.ExecuteNonQuery(cmd);
                    //cmd = new SqlCommand("IBVD_CrearVersiculo", cnn);
                    //cmd.CommandType = CommandType.StoredProcedure;

                    //cmd.Parameters.AddWithValue("CodigoBiblia", item.CodigoBiblia);
                    //cmd.Parameters.AddWithValue("Libro", item.Libro);
                    //cmd.Parameters.AddWithValue("Capitulo", item.Capitulo);
                    //cmd.Parameters.AddWithValue("Versiculo", item.Numero);
                    //cmd.Parameters.AddWithValue("Texto", item.Texto);

                    //cmd.ExecuteNonQuery();
                }

                //cnn.Close();

                LoggingComponent.LogInfo(string.Format("Se guardaron {0} registros", versiculos.Count));

            }
            catch (Exception ex)
            {
                LoggingComponent.LogError("Se produjo un error al guardar los versículos en la base de datos",ex);
                throw ex;
            }
        }

        internal static void CrearBiblia(string codigo, string nombre)
        {
            try
            {
                var cmd = db.GetStoredProcCommand("IBVD_CrearBiblia");

                db.AddInParameter(cmd, "Codigo", DbType.String, codigo);
                db.AddInParameter(cmd, "Nombre", DbType.String, nombre);

                db.ExecuteNonQuery(cmd);
               
            }
            catch (Exception ex)
            {
                LoggingComponent.LogError("Se produjo un error al guardar la biblia en la base de datos",ex);
                throw ex;
            }
        }


        internal static IList<Versiculo> GetVersiculos(Query query)
        {
            IList<Versiculo> versiculos = new List<Versiculo>();

            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetVersiculos");
                db.AddInParameter(cmd, "PageIndex", DbType.Int32, query.PageIndex);
                db.AddInParameter(cmd, "PageSize", DbType.Int32, query.PageSize);
                db.AddInParameter(cmd, "Paginate", DbType.Boolean, query.Paginate);
                db.AddInParameter(cmd, "SortFieldName", DbType.String, query.Order.Value);
                db.AddInParameter(cmd, "SortFieldType", DbType.String, query.Order.OrderType);
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Versiculo versiculo = BuildVersiculo(reader);
                        versiculos.Add(versiculo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener los versiculos de la base de datos: BibliaMapper.GetVersiculos",ex);
            }

            return versiculos;
        }

        private static Versiculo BuildVersiculo(IDataReader reader)
        {
            string biblia = (string)reader["CodigoBiblia"];
            int idLibro = (int)reader["Libro"];
            string libroTexto = (string)reader["LibroTexto"];
            int versiculo = (int)reader["Versiculo"];
            int capitulo = (int)reader["Capitulo"];
            string texto = (string)reader["Texto"];

            return new Versiculo(biblia, idLibro, libroTexto,capitulo, versiculo, texto);
        }

        internal static int GetTotalVersiculos(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetTotalVersiculos");
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de versiculos de la base de datos: BibliaMapper.GetTotalVersiculos",ex);
            }

            return total;
        }

        internal static IDictionary<string, string> GetBibliasBasico()
        {
            IDictionary<string, string> biblias = new Dictionary<string, string>();

            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetBibliasData");

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        biblias.Add((string)reader["Codigo"], (string)reader["Nombre"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las biblias de la base de datos: BibliaMapper.GetBibliasBasico", ex);
            }

            return biblias;
        }

        internal static IDictionary<string, string> GetLibrosBasico()
        {
            IDictionary<string, string> biblias = new Dictionary<string, string>();

            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetLibrosData");

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        biblias.Add(reader["NroLibro"].ToString(), (string)reader["Libro"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las biblias de la base de datos: BibliaMapper.GetBibliasBasico", ex);
            }

            return biblias;
        }



        internal static IList<Pasaje> BuscarPasaje(int capituloNro, int libroNro, IList<string> codigoBiblias, IList<int> versiculosList)
        {
            IList<Pasaje> pasajes = new List<Pasaje>();
            IList<Biblia> biblias = null;
            Libro libro = null;
            try
            {
                var codBiblias = ListToString(codigoBiblias);
                var codVersiculos = ListToString(versiculosList);
                if (codBiblias.Equals(string.Empty))
                    codBiblias = null;

                if (codVersiculos.Equals(string.Empty))
                    codVersiculos = null;
               
                DbCommand cmd = db.GetStoredProcCommand("IBVD_BuscarPasaje", codBiblias,codVersiculos,
                    capituloNro > 0? (int?)capituloNro : null,
                    libroNro);

                using (DataSet dataSet = db.ExecuteDataSet(cmd))
                {
                    biblias = BuildBiblias(dataSet.Tables[0]);
                    libro = BuildLibro(dataSet.Tables[1].Rows[0]);

                    foreach (var biblia in biblias)
                    {
                        pasajes.Add(new Pasaje(biblia, libro, new Capitulo(capituloNro), new List<Versiculo>()));
                    }

                    foreach (DataRow row in dataSet.Tables[2].Rows)
                    {
                        Versiculo versiculo = BuildVersiculo(row);
                        var pasaje = pasajes.First(m => m.Biblia.Codigo.Equals(versiculo.CodigoBiblia));
                        versiculo.LibroTexto = libro.Descripcion;
                        pasaje.Versiculos.Add(versiculo);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el pasaje de la base de datos: BibliaMapper.BuscarPasaje", ex);
            }

            return pasajes;
        }

        private static string ListToString(IList<int> versiculosList)
        {
            string resultado = string.Empty;

            for (int i = 0; i < versiculosList.Count; i++)
            {
                resultado += "" + versiculosList[i] + "";
                if (i < versiculosList.Count - 1)
                    resultado += ",";
            }

            return resultado;
        }

        private static string ListToString(IList<string> codigoBiblias)
        {
            string resultado = string.Empty;

            for (int i = 0; i < codigoBiblias.Count; i++)
            {
                resultado += "" + codigoBiblias[i] + "";
                if (i < codigoBiblias.Count - 1)
                    resultado += ",";
            }
            
            return resultado;
        }



        private static Versiculo BuildVersiculo(DataRow row)
        {
            string biblia = (string)row["CodigoBiblia"];
            int idLibro = (int)row["Libro"];
            int versiculo = (int)row["Versiculo"];
            string texto = (string)row["Texto"];
            int capitulo = (int)row["Capitulo"];

            return new Versiculo(biblia, idLibro, string.Empty,capitulo, versiculo, texto);
        }

        private static Libro BuildLibro(DataRow row)
        {
            int numero = (int)row["NroLibro"];
            string descripcion = (string)row["Libro"];

            return new Libro(numero, descripcion);
        }

        private static IList<Biblia> BuildBiblias(DataTable dataTable)
        {
            IList<Biblia> biblias = new List<Biblia>();

            foreach (DataRow rows in dataTable.Rows)
            {
                string codigo = (string)rows["Codigo"];
                string nombre = (string)rows["Nombre"];

                biblias.Add(new Biblia(nombre, codigo));
            }

            return biblias;
        }
    }
}
