using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Common.Validation;

namespace IBVD.Digital.Logic.Entities
{
    public class ProximaFecha:IValidate
    {
        public int Id { get; private set; }
        public Usuario Encargado { get; private set; }
        public DateTime Fecha { get; private set; }
        public string Tema { get; private set; }

        public ProximaFecha(int id, Usuario encargado, DateTime fecha, string tema)
        {
            this.Id = id;
            this.Encargado = encargado;
            this.Fecha = fecha;
            this.Tema = tema;
        }


        #region IValidate Members

        public bool IsValid()
        {
            return GetRuleViolations().Count() == 0;
        }

        public IEnumerable<RuleValidation> GetRuleViolations()
        {
            if (this.Fecha <= DateTime.Now)
                yield return new RuleValidation("La fecha y horario debe ser superior a la actual", "Fecha");
            if (this.Encargado == null)
                yield return new RuleValidation("Debe ingresar un usuario como encargado", "Encargado");

            yield break;
        }

        #endregion
    }
}
