using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    public class Operacion
    {
       
        public int Id { get; private set; }
        public string Nombre { get; private set; }
        public string Codigo { get; private set; }
        public bool Habilitado { get; private set; }

        public Operacion()
        {

        }

        public Operacion(int id, string nombre, string codigo, bool habilitado)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Codigo = codigo;
            this.Habilitado = habilitado;
        }
    }

    public static class Operaciones
    {
        
        public const string CANCIONES_CREAR = "Canciones.Crear";
        public const string CANCIONES_ELIMINAR = "Canciones.Eliminar";
        public const string CANCIONES_LISTAR = "Canciones.Listar";
        public const string CANCIONES_IMPORTAR = "Canciones.Importar";
        public const string CANCIONES_EXPORTAR = "Canciones.Exportar";
        public const string CANCIONES_NOTIFICACIONES = "Canciones.Notificaciones";

        public const string USUARIOS_CREAR = "Administracion.CrearUsuario";
        public const string USUARIOS_LISTAR = "Administracion.ListarUsuarios";
        public const string USUARIOS_RESET = "Administracion.ResetearPassword";
        public const string USUARIOS_MODIFICAR_ROL = "Administracion.ModificarRoleUsuario";

        public const string REUNIONES_CREAR = "Reuniones.Crear";
        public const string REUNIONES_ANULAR = "Reuniones.Eliminar";
        public const string REUNIONES_NOTIFICACIONES = "Reuniones.Notificaciones";
        public const string REUNIONES_LISTAR = "Reuniones.Listar";
        public const string REUNIONES_DIRIGIR = "Reuniones.DirigirCulto";

        public const string DIRECTORIO_LISTAR = "Imagenes.Listar";
        public const string DIRECTORIO_EDITAR = "Imagenes.Editar";

        public const string ROLES_LISTAR = "Administracion.ListarRoles";
        public const string ROLES_MODIFICAR = "Administracion.ModificarRole";

        public const string BIBLIA_CONSULTAR = "Biblias.Consultar";
        public const string BIBLIA_IMPORTAR = "Biblias.Importar";

        public const string REUNIONES_FECHAS_CREAR = "Reuniones.CrearProximaFecha";
        public const string REUNIONES_FECHAS_LISTAR = "Reuniones.ListarProximasFechas";
        public const string REUNIONES_FECHAS_NOTIFICACIONES = "Reuniones.ProximasFechasNotificaciones";

        
    }
}
