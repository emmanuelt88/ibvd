using System;
using System.Collections.Generic;

using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Entities;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBVD.Digital.Common.Fault;

namespace IBVD.Digital.Logic.Component.Mapper
{
    internal static class CancionesMapper 
    {
        private static readonly Database db = DatabaseFactory.CreateDatabase("DB");

        internal static IList<Cancion> GetCanciones(Query query)
        {
            IList<Cancion> canciones = new List<Cancion>();

            try
            {
                
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetCanciones");
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
                        Cancion cancion= BuildCancion(reader);
                        canciones.Add(cancion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las canciones de la base de datos: CancionesMapper.GetCanciones",ex);
            }

            return canciones;
        }

        private static Cancion BuildCancion(IDataReader reader)
        {
            int id = (int)reader["Id"];
            string titulo = (string)reader["Titulo"];
            string compas = (string)reader["Compas"];
            string tono = (string)reader["Tono"];
            string letra = (string)reader["Letra"];
            bool habilitado = (bool)reader["Habilitado"];
            string fotoURI = (string)reader["FotoURI"];
            int duracionEstimada = (int)reader["DuracionEstimada"];
            string formatData = (string)reader["FormatData"];
            string userNameLastUpdate = (string)reader["UserNameLastUpdate"];

            return new Cancion(id, titulo, tono, compas, letra, habilitado, duracionEstimada, fotoURI, formatData, userNameLastUpdate);
        }

        internal static int GetTotalCanciones(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetTotalCanciones");
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de canciones de la base de datos: CancionesMapper.GetTotalCanciones",ex);
            }

            return total;
        }

        internal static Cancion GetCancion(int idCancion)
        {
            Cancion cancion = null;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetCancion");
                db.AddInParameter(cmd, "IdCancion", DbType.Int32, idCancion);

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        cancion = BuildCancion(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener la canción de la base de datos: CancionesMapper.GetCancion", ex);
            }
            return cancion;
        }

        internal static void Save(Cancion cancion)
        {
            try
            {


                
                    
                
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarCancion");
                db.AddInParameter(cmd, "Id", DbType.Int32, cancion.Id);
                db.AddInParameter(cmd, "Titulo", DbType.String, cancion.Titulo);
                db.AddInParameter(cmd, "Tono", DbType.String, cancion.Tono);
                db.AddInParameter(cmd, "Compas", DbType.String, cancion.Compas);
                db.AddInParameter(cmd, "Letra", DbType.String, cancion.Letra);
                db.AddInParameter(cmd, "Habilitado", DbType.Boolean, cancion.Habilitado);
                db.AddInParameter(cmd, "FotoURI", DbType.String, cancion.FotoURI);
                db.AddInParameter(cmd, "DuracionEstimada", DbType.Int32, cancion.DuracionEstimada);
                db.AddInParameter(cmd, "FormatData", DbType.String, cancion.FormatData);
                db.AddInParameter(cmd, "UserNameLastUpdate", DbType.String, cancion.UserNameLastUpdate);

                db.ExecuteNonQuery(cmd);
               
                
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al actualizar la canción de la base de datos: CancionesMapper.GetCancion",ex);
            }
        }

        internal static int Create(Cancion cancion)
        {
            int idCancion = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_CrearCancion");
                db.AddInParameter(cmd, "Titulo", DbType.String, cancion.Titulo);
                db.AddInParameter(cmd, "Tono", DbType.String, cancion.Tono);
                db.AddInParameter(cmd, "Compas", DbType.String, cancion.Compas);
                db.AddInParameter(cmd, "Letra", DbType.String, cancion.Letra);
                db.AddInParameter(cmd, "Habilitado", DbType.Boolean, cancion.Habilitado);
                db.AddInParameter(cmd, "FotoURI", DbType.String, cancion.FotoURI);
                db.AddInParameter(cmd, "DuracionEstimada", DbType.Int32, cancion.DuracionEstimada);
                db.AddInParameter(cmd, "FormatData", DbType.String, cancion.FormatData);
                db.AddInParameter(cmd, "UserNameLastUpdate", DbType.String, cancion.UserNameLastUpdate);
                idCancion = int.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear la canción de la base de datos: CancionesMapper.GetCancion",ex);
            }

            return idCancion;
        }

        internal static int GetIdCancion(string titulo)
        {
            int idCancion = 0;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetIdCancion");
                db.AddInParameter(cmd, "Titulo", DbType.String, titulo);

                idCancion = int.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener la canción de la base de datos: CancionesMapper.GetIdCancion",ex);
            }

            return idCancion;
        }
    }
}
