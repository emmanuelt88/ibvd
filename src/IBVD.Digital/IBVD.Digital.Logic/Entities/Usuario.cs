using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Common.Validation;
using System.Collections.ObjectModel;
using IBVD.Digital.Logic.Helper;
using System.IO;
using IBVD.Digital.Logic.Component;

namespace IBVD.Digital.Logic.Entities
{
    public class Usuario:IValidate
    {
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Sexo { get; private set; }
        public DateTime FechaNacimiento { get; private set; }
        public string Domicilio { get; private set; }
        public string TelefonoParticular { get; private set; }
        public string TelefonoCelular { get; private set; }
        public ArchivoData Foto { get; internal set; }
        public string UserFotoURL
        {
            get
            {
                string virtualFileName = "/Images/" + (string.IsNullOrEmpty(Sexo.Trim()) ? "PerfilHombreMujer.jpg" : string.Format("{0}_SinFoto.jpg", Sexo));
                try
                {
                    string diskFileLocation = IOHelper.GetDiskLocation(this.Foto.FullPath);
                    if (File.Exists(diskFileLocation))
                    {
                        virtualFileName = Foto.FullPath;
                    }
                }
                catch 
                {
                    //LoggingComponent.LogError("Error al obtener la uri de la imagen de perfil del usuario", ex);
                }


                return virtualFileName;
            }
        }
        public bool EsMiembro { get; private set; }
        public int IdMiembro { get; private set; }
        public bool Habilitado { get; set; }
        public bool Pendiente { get; set; }
        private ReadOnlyCollection<Role> _roles;
        internal Guid FotoGUID { get; set; }
        public ReadOnlyCollection<Role> RolesList { get { return _roles; } }
        public ReadOnlyCollection<Operacion> Individuales { get; set; }
        public ReadOnlyCollection<Role> Roles {
            get 
            {
                if (Pendiente)
                    return new ReadOnlyCollection<Role>(new List<Role>());
                else
                    return _roles;
            }
            internal set { _roles = value; }
        }
        public string UserName { get; private set; }
        public string Password { get; internal set; }
        public string EncriptedPassword { get;  set; }
        public string Email { get; private set; }

        public string FullNameOrUser
        {
            get { return FullName.Trim() == string.Empty ? UserName : FullName; }
        }
        public string FullNameUser
        {
            get { return string.IsNullOrEmpty(FullName.Trim()) ? UserName : FullName + " | " + UserName; }
        }
        public Usuario()
        {
            this.Foto = new ArchivoData();
            this.FechaNacimiento = DateTime.Now;
            this.Nombre = string.Empty;
            this.Apellido = string.Empty;
            this.Sexo = string.Empty;
        }


        public Usuario(string userName, string email, string password, IList<Role> roles,IList<Operacion> individuales, bool pendiente, bool habilitado)
        {
            this.Roles = new ReadOnlyCollection<Role>(roles);
            this.Pendiente = pendiente;
            this.Foto = new ArchivoData();
            this.FechaNacimiento = DateTime.Now;
            this.Nombre = string.Empty;
            this.Apellido = string.Empty;
            this.Sexo = string.Empty;
            this.Password = password;
            this.UserName = userName;
            this.Habilitado = habilitado;
            this.Email = email;
            this.Individuales = new ReadOnlyCollection<Operacion>(individuales);

        }

        public Usuario(string userName,string password, string nombre, string apellido, string email, string sexo, DateTime fechaNacimiento,
            string domicilio, string telefonoParticular, string telefonoCelular, bool esMiembro, int idMiembro,
            IList<Role> roles,IList<Operacion> individuales, bool pendiente, bool habilitado)
            : this(userName, email, password, roles,individuales, pendiente, habilitado)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Sexo = sexo;
            this.FechaNacimiento = fechaNacimiento;
            this.Domicilio = domicilio;
            this.TelefonoParticular = telefonoParticular;
            this.TelefonoCelular = telefonoCelular;
            
            this.EsMiembro = esMiembro;
            this.IdMiembro = idMiembro;

        }


        public Usuario(string userName, string password, string nombre, string apellido, string email, string sexo, DateTime fechaNacimiento,
         string domicilio, string telefonoParticular, string telefonoCelular, ArchivoData foto, bool esMiembro, int idMiembro,
         IList<Role> roles,IList<Operacion> individuales, bool pendiente, bool habilitado)
            : this(userName, password, nombre, apellido, email, sexo, fechaNacimiento,
         domicilio, telefonoParticular, telefonoCelular,  esMiembro, idMiembro,
         roles, individuales, pendiente, habilitado)
        {
            this.Foto = foto;
        }

        public string FullName
        {
            get { return this.Nombre + " " + this.Apellido; }
        }
        /// <summary>
        /// Si la entidad es válida
        /// </summary>
        /// <returns>Si es valida</returns>
        public bool IsValid()
        { 
            return this.GetRuleViolations().Count() == 0;
        }

        /// <summary>
        /// Obtiene las reglas de validacion de la entidad
        /// </summary>
        /// <returns><list type="RuleViolation">Lista de reglas de validacion violadas</list></returns>
        public IEnumerable<RuleValidation> GetRuleViolations()
        {
            
            if (string.IsNullOrEmpty(this.UserName))
                yield return new RuleValidation("El Nombre de usuario es requerido", "UserName");
       
            if (!ValidatorHelper.IsValidEmail(this.Email))
                yield return new RuleValidation("El Email ingresado es inválido", "Email");
            if (string.IsNullOrEmpty(this.Password) || this.Password.Length <  6)
                yield return new RuleValidation("La contraseña ingresada es inválida, debe tener al menos 6 caracteres", "Password");

            if (!Pendiente)
            {
                if (string.IsNullOrEmpty(this.Apellido))
                    yield return new RuleValidation("El Apellido es requerido", "Apellido");
                if (string.IsNullOrEmpty(this.Nombre))
                    yield return new RuleValidation("El Nombre es requerido", "Nombre");

                if (string.IsNullOrEmpty(this.Sexo))
                    yield return new RuleValidation("El Sexo es requerido", "Sexo");

                if (!string.IsNullOrEmpty(this.Sexo) && (!this.Sexo.Equals("M") && !this.Sexo.Equals("F")))
                    yield return new RuleValidation("El Sexo ingresado es inválido", "Sexo");

                if (this.FechaNacimiento > DateTime.Now.AddYears(-18))
                    yield return new RuleValidation("Debe ser mayor de 18 años", "Edad");
            }
           

            yield break;
        }


        public List<Operacion> GetOperaciones()
        {

            List<Operacion> operaciones = new List<Operacion>();

            // Checkeo las operaciones por roles
            foreach (var item in Roles)
            {
                foreach (var operacion in item.Operaciones)
	            {
            	    if(!operaciones.Exists(m=> m.Codigo.Equals(operacion.Codigo)))	
                    {
                        operaciones.Add(operacion);
                    }
	            }
            }

            // Checkeo las operaciones individuales
            foreach (var operacion in Individuales)
            {
                if (!operaciones.Exists(m => m.Codigo.Equals(operacion.Codigo)))
                {
                    operaciones.Add(operacion);
                }
            }

            return operaciones;
        }



        public bool TienePermiso(string codigoOperacion)
        {
            var operaciones = GetOperaciones();
            return operaciones.FirstOrDefault(m => m.Codigo.Equals(codigoOperacion)) != null;
        }



        public bool IsAdmin()
        {
            return this.UserName.Equals("admin");
        }
    }
}
