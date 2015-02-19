using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Common.Validation;

namespace IBVD.Digital.Logic.Entities
{

    [Serializable]
    public class Cancion : ItemReunion, IValidate
    {
        public int Id { get; private set; }
        public string Titulo { get; set; }
        public string Tono {get;set;}
        public string Compas { get; set; }
        public string Letra { get; set; }
        public bool Habilitado { get;  set; }
        public string FotoURI { get; set; }
        public int DuracionEstimada { get; set; }
        public string FormatData { get; set; }
        public string UserNameLastUpdate { get; set; }
        public Cancion()
        {
            FotoURI = string.Empty;
            FormatData = string.Empty;
        }

        public Cancion(int id, string titulo, string tono, string compas, string letra, bool habilitado, int duracionEstimada, string formatData)
        {
            this.Id = id;
            this.Titulo = titulo;
            this.Tono = tono;
            this.Compas = compas;
            this.Letra = letra;
            this.Habilitado = habilitado;
            this.FotoURI = string.Empty;
            this.DuracionEstimada = duracionEstimada;
            this.FormatData = formatData;
        }

        public Cancion(int id, string titulo, string tono, string compas, string letra, bool habilitado, int duracionEstimada, string fotoURI, string formatData, string userNameLastUpdate) :
            this(id, titulo, tono, compas, letra,habilitado,duracionEstimada, formatData)
        {
            this.FotoURI = fotoURI;
            this.UserNameLastUpdate = userNameLastUpdate;    
            
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
                yield return new RuleValidation("El título de la canción es requerido", "Titulo");
            if(DuracionEstimada < 0)
                yield return new RuleValidation("La duración estimada debe ser mayor o igual a cero", "DuracionEstimada");
            if (!string.IsNullOrEmpty(this.Letra) && this.Letra.Length > 10000)
                yield return new RuleValidation("La letra no puede tener más de 10000 caracteres", "Letra");

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
            if (!String.IsNullOrEmpty(this.Tono))
                builder.Append(this.Tono.GetHashCode());
            if (!String.IsNullOrEmpty(this.Compas))
                builder.Append(this.Compas.GetHashCode());
            if (!String.IsNullOrEmpty(this.Letra))
                builder.Append(this.Letra.GetHashCode());

            
            return builder.ToString().GetHashCode();
        }

        public override int GetId()
        {
            return this.Id;
        }

        public override TipoItemReunion GetTipo()
        {
            return TipoItemReunion.CANCION;
        }


        protected override void SetCurrentId(int id)
        {
        }

        public override string GetText()
        {
            if (this.DuracionEstimada > 0)
                return string.Format("{0} ({1} min)", this.Titulo, this.DuracionEstimada);
            else
                return this.Titulo;
        }

        public override string GetDetails()
        {
            return this.Letra;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public bool Match(string texto)
        {
            string normalizado = texto.SimpleText();

            return this.Titulo.SimpleText().Contains(texto);

        }
    }
}
