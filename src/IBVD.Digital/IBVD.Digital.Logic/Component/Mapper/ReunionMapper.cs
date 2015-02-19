using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using IBVD.Digital.Common.Fault;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Entities;

namespace IBVD.Digital.Logic.Component.Mapper
{
    public class ReunionMapper
    {
        private static readonly Database db = DataBaseManager.GetDataBase();

        internal static int Create(Reunion reunion)
        {
            int idReunion = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_CrearReunion");
                db.AddInParameter(cmd, "Titulo", DbType.String, reunion.Titulo);
                db.AddInParameter(cmd, "FechaCreacion", DbType.DateTime, reunion.FechaCreacion);
                db.AddInParameter(cmd, "FechaCulto", DbType.DateTime, reunion.FechaCulto);
                db.AddInParameter(cmd, "FechaEnsayo", DbType.DateTime, reunion.FechaEnsayo);
                db.AddInParameter(cmd, "IdEstado", DbType.Int32, (int)reunion.Estado);
                db.AddInParameter(cmd, "UserEncargado", DbType.String, reunion.Encargado.UserName);
                db.AddInParameter(cmd, "HayCena", DbType.Boolean, reunion.HayCena);
                db.AddInParameter(cmd, "Descripcion", DbType.String, reunion.Descripcion);

                idReunion = int.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear la reunión de la base de datos: ReunionesMapper.Create");
            }

            return idReunion;
        }

        internal static void AgregarItemReunion(ItemReunion item,int orden, int idReunion)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_AgregarItemReunion");
                db.AddInParameter(cmd, "Id", DbType.Int32, item.GetId());
                db.AddInParameter(cmd, "IdReunion", DbType.Int32, idReunion);
                db.AddInParameter(cmd, "Tipo", DbType.Int32, (int)item.GetTipo());
                db.AddInParameter(cmd, "Orden", DbType.Int32, orden);

                db.ExecuteNonQuery(cmd).ToString();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear un item reunion a la reunión de la base de datos: ReunionesMapper.AgregarItemReunion", ex);
            }
        }

        internal static void EliminarItemsReunion(int idReunion)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_EliminarItemsReunion");
                db.AddInParameter(cmd, "IdReunion", DbType.Int32, idReunion);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al eliminar los items de la reunión de la base de datos: ReunionesMapper.EliminarItemsReunion", ex);
            }
        }

        internal static Reunion GetReunion(int idReunion)
        {
            Reunion reunion = null;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetReunion");
                db.AddInParameter(cmd, "IdReunion", DbType.Int32, idReunion);

                DataSet ds = db.ExecuteDataSet(cmd);
                // Bind de datos de la reunion
                if(ds.Tables[0].Rows.Count == 1){
                    IList<Pasaje> pasajes = BuildPasajes(ds.Tables[2].Rows);
                    IList<ItemLibre> libres = BuildItemLibres(ds.Tables[3].Rows);
                    IList<ItemReunion> itemsReunion = BuildItemsReunion(ds.Tables[1].Rows, pasajes, libres);
                    
                    reunion = BuildReunion(ds.Tables[0].Rows[0], itemsReunion);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error obtener la reunión de la base de datos: ReunionesMapper.GetReunion", ex);
            }

            return reunion;
        }

        private static IList<ItemLibre> BuildItemLibres(DataRowCollection dataRowCollection)
        {
            IList<ItemLibre> items = new List<ItemLibre>();

            foreach (DataRow row in dataRowCollection)
            {
                int id = (int)row["Id"];
                int reunionId = (int)row["IdReunion"];
                string titulo = (string)row["Titulo"];
                string detalle = (string)row["Detalle"];

                ItemLibre item = new ItemLibre(id, reunionId, titulo, detalle);
                items.Add(item);
            }

            return items;
        }

        private static Reunion BuildReunion(DataRow dataRow, IList<ItemReunion> itemsReunion)
        {
            Reunion reunion = null;
            int id = (int)dataRow["Id"];
            string titulo = (string)dataRow["Titulo"];
            DateTime fechaCreacion = (DateTime)dataRow["FechaCreacion"];
            DateTime fechaCulto = (DateTime)dataRow["FechaCulto"];
            DateTime fechaEnsayo = (DateTime)dataRow["FechaEnsayo"];
            Reunion.EstadoENUM estado = (Reunion.EstadoENUM)dataRow["IdEstado"];
            string uEncargado = (string)dataRow["UserEncargado"];
            Usuario encargado = SeguridadComponent.GetUsuario(uEncargado);
            bool hayCena = (bool)dataRow["Haycena"];
            string descripcion = (string)dataRow["Descripcion"];

            reunion = new Reunion(id, titulo, fechaCreacion, fechaCulto, fechaEnsayo,
                estado, encargado, hayCena, descripcion, itemsReunion);


            return reunion;
        }

        private static IList<Pasaje> BuildPasajes(DataRowCollection dataRowCollection)
        {
            IList<Pasaje> pasajes = new List<Pasaje>();

            foreach (DataRow row in dataRowCollection)
            {
                string codigoBiblia = (string)row["CodigoBiblia"];
                int id = (int)row["Id"];
                int nroLibro = (int)row["Libro"];
                int reunionId = (int)row["ReunionId"];
                string pasajeTexto = (string)row["Pasaje"];
                int capitulo = 0;
                IList<int> versiculos = new List<int>();

                BibliasComponent.GenerarPasajeData(pasajeTexto, out capitulo, versiculos);

                IList<string> biblias = new List<string>() { codigoBiblia };
                Pasaje pasaje = BibliasComponent.BuscarPasaje(capitulo, nroLibro, biblias, versiculos).First();
                pasaje.PasajeTexto = pasajeTexto;
                pasaje.SetId(id);
                pasajes.Add(pasaje);
            }

            return pasajes;
        }

        private static IList<ItemReunion> BuildItemsReunion(DataRowCollection dataRowCollection, IList<Pasaje> pasajes, IList<ItemLibre> itemsLibres)
        {
            IList<ItemReunion> itemsReunion = new List<ItemReunion>();

            foreach (DataRow row in dataRowCollection)
            {
                int id = (int)row["Id"];
                int idReunion = (int)row["IdReunion"];
                TipoItemReunion tipo = (TipoItemReunion)row["Tipo"];
                int orden = (int)row["Orden"];

                switch (tipo)
                {
                    case TipoItemReunion.CANCION:
                        Cancion cancion = CancionesComponent.GetCancion(id);
                        itemsReunion.Add(cancion);
                        break;
                    case TipoItemReunion.PASAJE:
                        itemsReunion.Add(pasajes.First(m => m.Id == id));
                        break;
                    case TipoItemReunion.ITEM_LIBRE:
                        itemsReunion.Add(itemsLibres.First(m => m.GetId() == id));
                        break;
                    default:
                        break;
                }
             
                
            }
            return itemsReunion;
           
        }





        internal static IList<Reunion> GetReuniones(Query query)
        {
            IList<Reunion> reuniones = new List<Reunion>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetReuniones");
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
                        Reunion reunion = BuildReunion(reader);
                        reuniones.Add(reunion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las reuniones de la base de datos: ReunionesMapper.GetReuniones", ex);
            }

            return reuniones;
        }

        private static Reunion BuildReunion(IDataReader reader)
        {
            Reunion reunion = null;
            int id = (int)reader["Id"];
            string titulo = (string)reader["Titulo"];
            DateTime fechaCreacion = (DateTime)reader["FechaCreacion"];
            DateTime fechaCulto = (DateTime)reader["FechaCulto"];
            DateTime fechaEnsayo = (DateTime)reader["FechaEnsayo"];
            Reunion.EstadoENUM estado = (Reunion.EstadoENUM)reader["IdEstado"];
            string uEncargado = (string)reader["UserEncargado"];
            Usuario encargado = SeguridadComponent.GetUsuario(uEncargado);
            bool hayCena = (bool)reader["Haycena"];
            string descripcion = (string)reader["Descripcion"];

            reunion = new Reunion(id, titulo, fechaCreacion, fechaCulto, fechaEnsayo,
                estado, encargado, hayCena, descripcion);


            return reunion;
        }

        internal static int GetTotalReuniones(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetTotalReuniones");
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de reuniones de la base de datos: ReunionesMapper.GetTotalReuniones",ex);
            }

            return total;
        }

        internal static void Save(Reunion reunion)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarReunion");
                db.AddInParameter(cmd, "IdReunion", DbType.Int32, reunion.Id);
                db.AddInParameter(cmd, "Titulo", DbType.String, reunion.Titulo);
                db.AddInParameter(cmd, "FechaCulto", DbType.DateTime, reunion.FechaCulto);
                db.AddInParameter(cmd, "FechaEnsayo", DbType.DateTime, reunion.FechaEnsayo);
                db.AddInParameter(cmd, "IdEstado", DbType.Int32, (int)reunion.Estado);
                db.AddInParameter(cmd, "UserEncargado", DbType.String, reunion.Encargado.UserName);
                db.AddInParameter(cmd, "HayCena", DbType.Boolean, reunion.HayCena);
                db.AddInParameter(cmd, "Descripcion", DbType.String, reunion.Descripcion);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al guardar la reunión de la base de datos: ReunionesMapper.Save");
            }
        }

        internal static void CrearItemLibre(ItemLibre itemLibre, int idReunion)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_AgregarItemLibre");
                db.AddInParameter(cmd, "IdReunion", DbType.Int32, idReunion);
                db.AddInParameter(cmd, "Index", DbType.Int32, itemLibre.Index);
                db.AddInParameter(cmd, "Titulo", DbType.String, itemLibre.Titulo);
                db.AddInParameter(cmd, "Detalle", DbType.String, itemLibre.Texto);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear el item libre enla base de datos: ReunionesMapper.CrearItemLibre", ex);
            }
        }

        internal static int GetTotalProximasFechas(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetTotalProximasFechas");
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de proximas fechas de la base de datos: ReunionesMapper.GetTotalProximasFechas",ex);
            }

            return total;
        }

        internal static IList<ProximaFecha> GetProximasFechas(Query query)
        {
            IList<ProximaFecha> proximas = new List<ProximaFecha>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetProximasFechas");
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
                        ProximaFecha proxima = BuildProximaFecha(reader);
                        proximas.Add(proxima);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las proximas fechas de reuniones de la base de datos: ReunionesMapper.GetProximasFechas", ex);
            }

            return proximas;
        }

        private static ProximaFecha BuildProximaFecha(IDataReader reader)
        {
            int id = (int)reader["Id"];
            string userName = (string)reader["UserName"];
            string tema = (string)reader["Tema"];
            DateTime  fecha = (DateTime)reader["Fecha"];

            Usuario encargado = SeguridadComponent.GetUsuario(userName);

            return new ProximaFecha(id, encargado, fecha, tema);
        }

        internal static void EliminarProximaFecha(int id)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_EliminarProximaFecha");
                db.AddInParameter(cmd, "Id", DbType.Int32, id);
                
                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al eliminar la proxima fecha de la base de datos: ReunionesMapper.EliminarProximaFecha", ex);
            }
        }

        internal static void ActualizarFechaCulto(ProximaFecha proxima)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarProximaFecha");

                db.AddInParameter(cmd, "Id", DbType.Int32, proxima.Id);
                db.AddInParameter(cmd, "UserName", DbType.String, proxima.Encargado.UserName);
                db.AddInParameter(cmd, "Tema", DbType.String, proxima.Tema);
                db.AddInParameter(cmd, "Fecha", DbType.DateTime, proxima.Fecha);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear la fecha de culto en la base de datos: ReunionesMapper.ActualizarFechaCulto", ex);
            }
        }

        internal static int CrearFechaCulto(ProximaFecha proxima)
        {
            int id = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_AgregarProximaFecha");
                db.AddInParameter(cmd, "UserName", DbType.String, proxima.Encargado.UserName);
                db.AddInParameter(cmd, "Tema", DbType.String, proxima.Tema);
                db.AddInParameter(cmd, "Fecha", DbType.DateTime, proxima.Fecha);

                id = int.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear la fecha de culto en la base de datos: ReunionesMapper.CrearFechaCulto", ex);
            }

            return id;
        }
    }
}
