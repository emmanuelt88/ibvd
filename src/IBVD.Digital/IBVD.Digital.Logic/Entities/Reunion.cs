using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Common.Validation;
using System.Collections.ObjectModel;
using IBVD.Digital.Logic.Helper;

namespace IBVD.Digital.Logic.Entities
{
    public class Reunion:IValidate
    {
        public enum EstadoENUM
        {
            CREADO = 1,
            CANCELADO = 2,
            FINALIZADO = 3
        }

        public int Id{ get; private set; }
        public string Titulo { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime FechaCulto { get; private set; }
        public DateTime FechaEnsayo { get; private set; }
        public EstadoENUM Estado { get; set; }
        public Usuario Encargado { get; private set; }
        public bool HayCena { get; private set; }
        public string Descripcion { get; private set; }
        public IList<ItemReunion> ItemsReunion { get; set; }

        public Reunion()
        {
            ItemsReunion = new List<ItemReunion>();
        }

        public Reunion(int id, string titulo, DateTime fechaCreacion, DateTime fechaCulto, DateTime fechaEnsayo, EstadoENUM estado, Usuario encargado,
            bool hayCena, string descripcion )
        {
            this.Id = id;
            this.Titulo = titulo;
            this.FechaCreacion = fechaCreacion;
            this.FechaCulto = fechaCulto;
            this.FechaEnsayo = fechaEnsayo;
            if (fechaCulto < DateTime.Now && estado == EstadoENUM.CREADO)
                this.Estado = EstadoENUM.FINALIZADO;
            else
            {
                this.Estado = estado;
            }
            this.Encargado = encargado;
            this.HayCena = hayCena;
            this.Descripcion = descripcion;
            this.ItemsReunion = new List<ItemReunion>();
        }

        public Reunion(int id, string titulo, DateTime fechaCreacion, DateTime fechaCulto, DateTime fechaEnsayo, EstadoENUM estado, Usuario encargado,
           bool hayCena, string descripcion, IList<ItemReunion> itemsReunion):
            this(id, titulo, fechaCreacion, fechaCulto, fechaEnsayo, estado, encargado, hayCena, descripcion)
        {
            this.ItemsReunion = itemsReunion;
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
            
            if (string.IsNullOrEmpty(this.Titulo))
                yield return new RuleValidation("El título de la reunión es requerido", "Titulo");
            if (this.Titulo.Trim().Length < 5)
                yield return new RuleValidation("El título debe tener al menos 5 caracteres", "Titulo");
            if (this.FechaEnsayo <= DateTime.Now)
                yield return new RuleValidation("La fecha y horario de ensayo debe ser superior a la actual", "FechaEnsayo");
            if(this.FechaCulto <= DateTime.Now)
                yield return new RuleValidation("La fecha y horario de culto debe ser superior a la actual", "FechaEnsayo");
            if (this.FechaEnsayo > this.FechaCulto)
                yield return new RuleValidation("La fecha de ensayo no puede ser posterior al culto", "FechaEnsayo");
            if (this.Encargado == null)
                yield return new RuleValidation("Debe ingresar un usuario como encargado", "UserEncargado");
            if (this.Descripcion.Length > 1000)
                yield return new RuleValidation("La descripción no puede superar los 1000 caracteres", "Descripcion");


            var duracionMaxima = ItemsReunion.Where(m => m.GetTipo() == TipoItemReunion.CANCION).Sum(m => ((Cancion)m).DuracionEstimada);
            if (duracionMaxima != 0 && duracionMaxima > ConfigurationHelper.Reunion_Canciones_MaxDuracion)
            {
                yield return new RuleValidation("La duración total de las canciones debe ser menor a " + ConfigurationHelper.Reunion_Canciones_MaxDuracion + " minutos", "DuracionTotal");
            }
            yield break;
        }

        public override bool Equals(object obj)
        {
            if (obj is Cancion)
                return this.GetHashCode().Equals(obj.GetHashCode());
            else
                return false;
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();


            builder.Append(this.Id.GetHashCode());
            if (!String.IsNullOrEmpty(this.Titulo))
                builder.Append(this.Titulo.GetHashCode());
            if (this.Encargado != null)
                builder.Append(this.Encargado.ToString());

            return builder.ToString().GetHashCode();
        }
    }
}
