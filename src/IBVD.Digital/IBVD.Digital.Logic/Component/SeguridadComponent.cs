using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Fault;
using System.Linq.Expressions;
using System.Web.Security;
using IBVD.Digital.Logic.Component.Mapper;
using IBVD.Digital.Common.Entities;
using IBVD.Digital.Common.Encriptacion;
using IBVD.Digital.Common.Notifications;
using System.Web.Hosting;
using System.IO;
using IBVD.Digital.Logic.Helper;
using System.Collections.ObjectModel;

namespace IBVD.Digital.Logic.Component
{
    public static class SeguridadComponent
    {
                      // can be 192 or 128 

      

        public static ListCollection<Usuario> GetUsers(Query query)
        {
            ListCollection<Usuario> usuarios = new ListCollection<Usuario>();

            var resultado = AdministracionMapper.GetUsuarios(query);
            usuarios.Total = AdministracionMapper.GetTotalUsuarios(query);
            usuarios.AddRange(resultado);
            return usuarios;
        }
     
        internal static IList<Role> GetRolesByUserName(string userName)
        {
            IList<Role> roles = AdministracionMapper.GetRolesByUserName(userName);

            return roles;
        }

        public static ListCollection<Role> GetRoles()
        {
            Query query = new Query();
            query.Paginate = false;
            query.Order = new FieldOrder("Role.Nombre", "ASC");
            IList<Role> roles = AdministracionMapper.GetRoles(query);
            ListCollection<Role> rolesCollection = new ListCollection<Role>();
            rolesCollection.AddRange(roles);
            rolesCollection.Total = AdministracionMapper.GetTotalRoles(query);

            return rolesCollection;
        }

        public static ListCollection<Role> GetRoles(Query query)
        {
            IList<Role> roles = AdministracionMapper.GetRoles(query);
            ListCollection<Role> rolesCollection = new ListCollection<Role>();
            rolesCollection.AddRange(roles);
            rolesCollection.Total = AdministracionMapper.GetTotalRoles(query);

            return rolesCollection;
        }

        public static void DisableUser(string userName)
        {
            
            var user =  Membership.GetUser(userName);
            
            user.IsApproved = false;

            Membership.UpdateUser(user);
        }

        public static void EnableUser(string userName)
        {
            var user = Membership.GetUser(userName);
            user.IsApproved = true;

            Membership.UpdateUser(user);
        }

        public static Usuario GetUsuario(string userName)
        {
            Usuario usuario = null;

            if (string.IsNullOrEmpty(userName))
                throw new NullReferenceException("Se esperaba el nombre de usuario");

            usuario = AdministracionMapper.GetUsuarioCompleto(userName);
            var roles = GetRolesByUserName(userName);
            var individual = roles.FirstOrDefault(m=> m.Id == 0);
            usuario.Roles = new System.Collections.ObjectModel.ReadOnlyCollection<Role>(roles.Where(m=> m.Id>0).ToList());
            usuario.Individuales = new ReadOnlyCollection<Operacion>(individual != null ? individual.Operaciones.ToList() : new List<Operacion>());

            return usuario;
        }

      
        public static void SaveRolesUsuario(Usuario usuario, List<int> idRoles)
        {
            List<Role> roles = new List<Role>();

            foreach (var idRole in idRoles)
            {
                Role role = SeguridadComponent.GetRoleById(idRole);

                if (role == null)
                    throw new HandleException("El role ingresado no existe");

                roles.Add(role);
            }

            usuario.Roles = new System.Collections.ObjectModel.ReadOnlyCollection<Role>(roles);

            AdministracionMapper.ActualizarRolesUsuario(usuario);
        }

        public static void SaveUsuario(Usuario usuario)
        {
            usuario.EncriptedPassword = EncriptadorHelper.Encrypt(usuario.Password);
            AdministracionMapper.ActualizarUsuario(usuario);
            AdministracionMapper.ActualizarRolesUsuario(usuario);
        }

        public static void SaveUsuario(Usuario usuario, ArchivoData fotoPerfil)
        {
            SaveUsuario(usuario);
            FilesMapper.SaveFile(fotoPerfil);
        }

        public static Role GetRoleById(int idRole)
        {
            Role role = null;

            Query query = new Query("Id","ASC", 0,1,string.Empty);
            query.Paginate = false;
            query.AddRule(new Rule("Id", Query.Comparator.EQUALS, idRole.ToString()));
            
            var result = AdministracionMapper.GetRoles(query);

            if(result.Count == 0 )
                throw new HandleException("El role ingresado no existe");

            role = result[0];

            role.Operaciones = new System.Collections.ObjectModel.ReadOnlyCollection<Operacion>(AdministracionMapper.GetOperacionesByRole(idRole));
            return role;
        }

        public static IList<Operacion> GetOperaciones()
        {
            IList<Operacion> operaciones = new List<Operacion>();

            operaciones = AdministracionMapper.GetOperaciones();

            return operaciones;
        }

        public static void SaveRoleOperaciones(Role role, IList<int> operationsIDs)
        {
            IList<Operacion> operaciones = AdministracionMapper.GetOperaciones();
            IList<Operacion> operacionesRole = new List<Operacion>();

            foreach (var idOperacion in operationsIDs)
            {
                var operacion = operaciones.FirstOrDefault(m => m.Id.Equals(idOperacion));
                if (operacion == null)
                    throw new HandleException("La operación ingresada no existe");

                operacionesRole.Add(operacion);
            }

            role.Operaciones = new System.Collections.ObjectModel.ReadOnlyCollection<Operacion>(operacionesRole);

            AdministracionMapper.SaveRoleOperaciones(role);

        }

        public static IList<Usuario> GetUsuariosByOperaciones(IList<string> list)
        {
            IList<Usuario> usuarios = AdministracionMapper.GetUsuariosByOperaciones(list);

            return usuarios;
        }

        public static void RegistrarUsuario(Usuario usuario)
        {
            Query query = new Query("UserName", "asc","");
            query.Paginate = false;
            query.AddRule(new Rule("UserName", Query.Comparator.EQUALS, usuario.UserName));
            bool existe = AdministracionMapper.GetUsuarios(query).Count > 0;
            if (existe)
                throw new HandleException("Ya existe una persona registrada con el nombre de usuario ingresado");

            query = new Query("UserName", "asc", "");
            query.Paginate = false;
            query.AddRule(new Rule("Email", Query.Comparator.EQUALS, usuario.Email));
            existe = AdministracionMapper.GetUsuarios(query).Count > 0;

            if (existe)
                throw new HandleException("Ya existe una persona registrada con el email ingresado");

            usuario.EncriptedPassword = EncriptadorHelper.Encrypt(usuario.Password);
            AdministracionMapper.CrearUsuario(usuario);
            NotificarRegistracion(usuario, "Se ha generado su usuario en el sistema");
        }

        private static void NotificarRegistracion(Usuario usuario, string titulo)
        {
            string html = MapearTemplateHTML(usuario, titulo, ConfigurationHelper.DirectorioMailTemplateRegistracionUsuario);

            MailSender.Send(usuario.Email, titulo, html, true); 
        }

        private static void NotificarRecordarDatosUsuario(Usuario usuario, string titulo)
        {
            string html = MapearTemplateHTML(usuario, titulo, ConfigurationHelper.DirectorioMailTemplateRecuperacionPassword);

            MailSender.Send(usuario.Email, titulo, html, true);
        }

        private static string MapearTemplateHTML(Usuario usuario, string titulo, string templatePath)
        {
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            mapper.Add("{userName}", usuario.UserName);
            mapper.Add("{password}", usuario.Password);


            string template = EmailTemplateHelper.FillTemplate(titulo, templatePath, mapper);

            return template;
        }

        public static bool ExisteUsuario(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new HandleException("Se esperaba el nombre de usuario");

            if(string.IsNullOrEmpty(password))
                throw new HandleException("Se esperaba la contraseña del usuario");

            string encriptedPassword = EncriptadorHelper.Encrypt(password);
            bool exists = AdministracionMapper.Exists(userName, encriptedPassword);

            return exists;
        }

        public static void RecordarDatos(string email)
        {
            Usuario usuario = GetUsuarioByEmail(email);

            if (usuario == null)
                throw new HandleException("No existe un usuario registrado con el email ingresado");

            NotificarRecordarDatosUsuario(usuario, "Recordatorio de sus datos personales");
        }

        private static Usuario GetUsuarioByEmail(string email)
        {
            return AdministracionMapper.GetUsuarioCompletoByEmail(email);
        }

        public static void SaveOperacionesUsuario(Usuario usuario, List<int> idOperaciones)
        {
            IList<Operacion> operacionesUsuario = new List<Operacion>();

            var operaciones = GetOperaciones();
            foreach (var idOperacion in idOperaciones)
            {
                var operacion = operaciones.FirstOrDefault(m => m.Id.Equals(idOperacion));
                if (operacion == null)
                    throw new HandleException("La operación ingresada no existe");

                operacionesUsuario.Add(operacion);
            }

            AdministracionMapper.SaveUsuarioOperaciones(usuario.UserName, idOperaciones);
        }

        public static ListCollection<Usuario> GetAll()
        {
            Query query = new Query(" Usuario.UserName ", "asc", string.Empty);
            query.Paginate = false;

            return GetUsers(query);
        }
    }
}
