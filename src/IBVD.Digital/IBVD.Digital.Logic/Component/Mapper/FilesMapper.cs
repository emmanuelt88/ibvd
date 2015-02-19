using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Fault;
using System.Data.Common;
using System.Data;

namespace IBVD.Digital.Logic.Component.Mapper
{
    internal static class FilesMapper
    {
        private static readonly Database db = DatabaseFactory.CreateDatabase("DB");


        internal static void SaveFile(ArchivoData file)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_SaveFile");

                db.AddInParameter(cmd, "Id", System.Data.DbType.Guid, file.Id);
                db.AddInParameter(cmd, "Name", System.Data.DbType.String, file.FileName);
                db.AddInParameter(cmd, "ContentType", System.Data.DbType.String, file.ContentType);
                db.AddInParameter(cmd, "Size", System.Data.DbType.Int32, file.Size);
                db.AddInParameter(cmd, "FullPath", System.Data.DbType.String, file.FullPath);
                db.ExecuteNonQuery(cmd);

            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error guardar los datos del archivo: FileMapper.SaveFile",ex);
            }
        }

        internal static ArchivoData GetFile(Guid fileGuid)
        {
            ArchivoData fileData = new ArchivoData();
            try
            {
                if (fileGuid == Guid.Empty)
                    return fileData;

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetFile");

                db.AddInParameter(cmd, "Id", System.Data.DbType.Guid, fileGuid);

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        fileData = BuildFile(reader);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error obtener los datos del archivo: FileMapper.GetFile", ex);
            }

            return fileData;
        }

        private static ArchivoData BuildFile(IDataReader reader)
        {
            Guid fileId = (Guid)reader["FileGUID"];
            string fileName = (string)reader["FileName"];
            string contentType = (string)reader["ContentType"];
            int size = (int)reader["Size"];
            DateTime fecha = (DateTime)reader["Fecha"];
            string fullPath = (string)reader["FullPath"];
            return new ArchivoData(fileId, fileName, contentType, size, fecha, fullPath);
        }

        internal static IList<ArchivoData> GetFiles(){
            IList<ArchivoData> files = new List<ArchivoData>();
            try
            {

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetFiles");


                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var fileData = BuildFile(reader);
                        files.Add(fileData);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error objeteners los datos de los archivos: FileMapper.GetFiles", ex);
            }

            return files;
        }
    }
}
