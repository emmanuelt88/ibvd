using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using IBVD.Digital.Common.Fault;
using System.Data;
using System.Data.Common;
using IBVD.Digital.Common.Entities;
using IBVD.Digital.Common.Encriptacion;

namespace IBVD.Digital.Logic.Component.Mapper
{
    public static class AdministracionMapper
    {

        private static readonly Database db = DataBaseManager.GetDataBase();

        internal static Usuario GetUsuarioCompleto(string userName)
        {
            Usuario usuario = null;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetUsuarioCompleto");
                db.AddInParameter(cmd, "UserName", DbType.String, userName.Trim());

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        usuario = BuildUsuario(reader);
                    }
                }

                if (usuario != null)
                    usuario.Foto = FilesMapper.GetFile(usuario.FotoGUID);
            }
            catch (Exception ex )
            {
                throw new DataAccessException("Error al obtener el usuario de la base de datos: AdministracionMapper.GetUsuarioCompleto");
            }

            return usuario;
        }

        internal static Usuario GetUsuarioCompletoByEmail(string email)
        {
            Usuario usuario = null;
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetUsuarioCompletoByEmail");
                db.AddInParameter(cmd, "Email", DbType.String, email.Trim());

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        usuario = BuildUsuario(reader);
                    }
                }

                if (usuario != null)
                    usuario.Foto = FilesMapper.GetFile(usuario.FotoGUID);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el usuario de la base de datos: AdministracionMapper.GetUsuarioCompleto");
            }

            return usuario;
        }

        private static Usuario BuildUsuario(IDataReader reader)
        {
            Usuario usuario = null;
            string userName = reader["UserName"].ToString();
            string nombre = reader["Nombre"].ToString();
            string apellido = reader["Apellido"].ToString();
            string email = reader["Email"].ToString();
            string sexo = reader["Sexo"].ToString();
            DateTime fechaNacimiento = (DateTime)reader["FechaNacimiento"];
            string domicilio = reader["Domicilio"].ToString();
            string telefonoParticular = reader["TelefonoParticular"].ToString();
            string telefonoCelular = reader["TelefonoCelular"].ToString();
            object guid = reader["FotoGUID"];
            Guid fotoGUID = guid == null || guid.ToString() == string.Empty ? Guid.Empty : new Guid(guid.ToString());

            
            bool esMiembro = (bool)reader["EsMiembro"];
            bool habilitado = (bool)reader["Habilitado"];
            int idMiembro = (int)reader["IdMiembro"];
            bool pendiente = (bool)reader["Pendiente"];
            string encriptedPassword = (string)reader["Password"];
            string password = EncriptadorHelper.Decrypt(encriptedPassword);

            

            usuario = new Usuario(userName, password, nombre, apellido, email,sexo, fechaNacimiento, domicilio,
                telefonoParticular, telefonoCelular, esMiembro, idMiembro, new List<Role>(), new List<Operacion>(), pendiente, habilitado);
            usuario.EncriptedPassword = encriptedPassword;


            usuario.FotoGUID = fotoGUID;
            return usuario;
        }

        internal static IList<Role> GetRolesByUserName(string userName)
        {
            IList<Role> roles = new List<Role>();
            
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetOperacionesByUserName");
                db.AddInParameter(cmd, "UserName", DbType.String, userName.Trim());


                roles.Add(new Role(0, "Individuales", "Individuales", true));
                using (DataSet dataset = db.ExecuteDataSet(cmd))
                {
                    BuildRoles(dataset.Tables[0], roles);
                    BuildRoleOperacion(dataset.Tables[1], roles);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el usuario de la base de datos: AdministracionMapper.GetRolesByUserName",ex);
            }

            return roles;
        }

        private static void BuildRoles(DataTable dataTable, IList<Role> roles)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                int roleId = int.Parse(row["RoleId"].ToString());
                string roleName = row["Role"].ToString();
                string roleCodigo = row["RoleCodigo"].ToString();
                bool roleHabilitado = (bool)row["RoleHabilitado"];

                var role = new Role(roleId, roleName, roleCodigo, roleHabilitado);
                roles.Add(role);
            }
        }

        private static void BuildRoleOperacion(DataTable operaciones, IList<Role> roles)
        {
            foreach (DataRow row in operaciones.Rows)
            {
                int roleId = int.Parse(row["RoleId"].ToString());
                string operacionNombre = row["Operacion"].ToString();
                string operacionCodigo = row["OperacionCodigo"].ToString();
                bool operacionHabilitada = (bool)row["OperacionHabilitado"];
                int operacionId = int.Parse(row["OperacionId"].ToString());
                Role role = roles.First(m => m.Id.Equals(roleId));
                Operacion operacion = new Operacion(operacionId, operacionNombre, operacionCodigo, operacionHabilitada);

                role.AddOperacion(operacion);
            }
        }



        internal static IList<Role> GetRoles(Query query)
        {
            IList<Role> roles = new List<Role>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetRoles");
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
                        Role role = BuildRole(reader);
                        roles.Add(role);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener los roles de la base de datos: AdministracionMapper.GetRoles");
            }

            return roles;
        }

        internal static int GetTotalRoles(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_CountRoles");
                db.AddInParameter(cmd, "PageIndex", DbType.Int32, query.PageIndex);
                db.AddInParameter(cmd, "PageSize", DbType.Int32, query.PageSize);
                db.AddInParameter(cmd, "Paginate", DbType.Boolean, query.Paginate);
                db.AddInParameter(cmd, "SortFieldName", DbType.String, query.Order.Value);
                db.AddInParameter(cmd, "SortFieldType", DbType.String, query.Order.OrderType);
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de roles de la base de datos: AdministracionMapper.GetTotalRoles");
            }

            return total;
        }

        private static Role BuildRole(IDataReader reader)
        {
            int id = (int)reader["Id"];
            string nombre = reader["Nombre"].ToString();
            string codigo = reader["Codigo"].ToString();
            bool habilitado = (bool)reader["Habilitado"];

            return new Role(id, nombre, codigo, habilitado);
        }

        internal static void CrearUsuario(Usuario usuario)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_CreateUsuario");

                BindUserParameters(usuario, cmd);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear un nuevo usuario en la base de datos: AdministracionMapper.CrearUsuario",ex);
            }
        }

        internal static void ActualizarUsuario(Usuario usuario)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarUsuario");
                BindUserParameters(usuario, cmd);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear un nuevo usuario en la base de datos: AdministracionMapper.CrearUsuario", ex);
            }
        }

        internal static void ActualizarRolesUsuario(Usuario usuario)
        {
            try
            {
                string idRoles = GetIds(usuario.RolesList.Select(m => m.Id).ToList());

                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarRolesUsuario");
                db.AddInParameter(cmd, "UserName", DbType.String, usuario.UserName);
                db.AddInParameter(cmd, "IdRoles", DbType.String, idRoles);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al crear un nuevo usuario en la base de datos: AdministracionMapper.CrearUsuario", ex);
            }
        }

        private static string GetIds(List<int> list)
        {
            string result = string.Empty;

            for (int i = 0; i < list.Count; i++)
            {
                result += list[i];

                if (i < list.Count - 1)
                    result += ",";
            }

            return result;
        }

        private static void BindUserParameters(Usuario usuario, DbCommand cmd)
        {
            db.AddInParameter(cmd, "UserName", DbType.String, usuario.UserName);
            db.AddInParameter(cmd, "Nombre", DbType.String, usuario.Nombre);
            db.AddInParameter(cmd, "Apellido", DbType.String, usuario.Apellido);
            db.AddInParameter(cmd, "Sexo", DbType.String, usuario.Sexo);
            db.AddInParameter(cmd, "Email", DbType.String, usuario.Email);
            db.AddInParameter(cmd, "FechaNacimiento", DbType.DateTime, usuario.FechaNacimiento);
            db.AddInParameter(cmd, "Domicilio", DbType.String, usuario.Domicilio);
            db.AddInParameter(cmd, "TelefonoParticular", DbType.String, usuario.TelefonoParticular);
            db.AddInParameter(cmd, "TelefonoCelular", DbType.String, usuario.TelefonoCelular);
            db.AddInParameter(cmd, "FotoGUID", DbType.Guid, usuario.Foto.Id);
            db.AddInParameter(cmd, "EsMiembro", DbType.Boolean, usuario.EsMiembro);
            db.AddInParameter(cmd, "IdMiembro", DbType.Int32, usuario.IdMiembro);
            db.AddInParameter(cmd, "Pendiente", DbType.Boolean, usuario.Pendiente);
            db.AddInParameter(cmd, "Habilitado", DbType.Boolean, usuario.Habilitado);
            db.AddInParameter(cmd, "Password", DbType.String, usuario.EncriptedPassword);
        }

        internal static IList<Operacion> GetOperaciones()
        {
            IList<Operacion> operaciones = new List<Operacion>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetOperaciones");

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Operacion operacion = BuildOperacion(reader);
                        operaciones.Add(operacion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las operaciones de la base de datos: AdministracionMapper.GetOperaciones", ex);
            }

            return operaciones;
        }

        internal static IList<Operacion> GetOperacionesByRole(int idRole)
        {
            IList<Operacion> operaciones = new List<Operacion>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetOperacionesByRole");

                db.AddInParameter(cmd, "IdRole", DbType.Int32, idRole);
                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Operacion operacion = BuildOperacion(reader);
                        operaciones.Add(operacion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener las operaciones de la base de datos: AdministracionMapper.GetOperacionesByRole", ex);
            }

            return operaciones;
        }

        private static Operacion BuildOperacion(IDataReader reader)
        {
            int idOperacion = (int)reader["Id"];
            string nombre = (string)reader["Nombre"];
            string codigo = (string)reader["Codigo"];
            bool habilitado = (bool)reader["Habilitado"];

            return new Operacion(idOperacion, nombre, codigo, habilitado);
        }

        internal static void SaveRoleOperaciones(Role role)
        {
            try
            {
                string idOperaciones = GetIds(role.Operaciones.Select(m => m.Id).ToList());

                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarOperacionesRole");
                db.AddInParameter(cmd, "IdRole", DbType.Int32, role.Id);
                db.AddInParameter(cmd, "IdOperaciones", DbType.String, idOperaciones);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al actualizar las operaciones en la base de datos: AdministracionMapper.SaveRoleOperaciones", ex);
            }
        }


        internal static IList<Usuario> GetUsuariosByOperaciones(IList<string> list)
        {
            IList<Usuario> usuarios = new List<Usuario>();
            try
            {
                string operaciones = GetStrings(list);

                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetUsuariosByOperaciones");
                db.AddInParameter(cmd, "Operaciones", DbType.String, operaciones);

                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        Usuario usuario = BuildUsuario(reader);
                        usuarios.Add(usuario);
                    }
                }

                foreach (var usuario in usuarios)
                {
                    usuario.Foto = FilesMapper.GetFile(usuario.FotoGUID);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener los usuarios por operaciones en la base de datos: AdministracionMapper.GetUsuariosByOperaciones", ex);
            }

            return usuarios;
        }

        private static string GetStrings(IList<string> list)
        {
            string resultado = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                resultado += list[i];

                if (i < list.Count - 1)
                {
                    resultado += ",";
                }
            }
            return resultado;
        }

        internal static IList<Usuario> GetUsuarios(Query query)
        {
            IList<Usuario> usuarios = new List<Usuario>();

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetUsuarios");
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
                        Usuario usuario = BuildUsuario(reader);
                        usuarios.Add(usuario);
                    }
                }

                foreach (var usuario in usuarios)
                {
                    usuario.Foto = FilesMapper.GetFile(usuario.FotoGUID);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener los usuarios de la base de datos: AdministracionMapper.GetUsuarios",ex);
            }

            return usuarios;
        }

        internal static bool Exists(string userName, string password)
        {
            bool existe = false;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ExisteUsuario");
                db.AddInParameter(cmd, "UserName", DbType.String, userName);
                db.AddInParameter(cmd, "Password", DbType.String, password);

                existe = bool.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al comprobar la existencia del usuario en la base de datos: AdministracionMapper.Exists", ex);
            }

            return existe;
        }

        internal static int GetTotalUsuarios(Query query)
        {
            int total = 0;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_GetTotalUsuarios");
                db.AddInParameter(cmd, "Filters", DbType.String, query.ToSQL());

                total = (int)db.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al obtener el total de usuarios de la base de datos: AdministracionMapper.GetTotalUsuarios",ex);
            }

            return total;
        }

        internal static bool Exists(Usuario usuario)
        {
            bool existe = false;

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("IBVD_ExisteRegistracion");
                db.AddInParameter(cmd, "Email", DbType.String, usuario.Email);
                db.AddInParameter(cmd, "UserName", DbType.String, usuario.UserName);

                existe = bool.Parse(db.ExecuteScalar(cmd).ToString());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al comprobar la existencia del usuario en la base de datos: AdministracionMapper.Exists", ex);
            }

            return existe;
        }

        internal static void SaveUsuarioOperaciones(string userName, List<int> idOperaciones)
        {
            try
            {
                string idOperacionesString = GetIds(idOperaciones);

                DbCommand cmd = db.GetStoredProcCommand("IBVD_ActualizarOperacionesUsuario");
                db.AddInParameter(cmd, "UserName", DbType.String, userName);
                db.AddInParameter(cmd, "IdOperaciones", DbType.String, idOperacionesString);

                db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Error al actualizar las operaciones del usuario en la base de datos: AdministracionMapper.SaveUsuarioOperaciones", ex);
            }
        }
    }
}
