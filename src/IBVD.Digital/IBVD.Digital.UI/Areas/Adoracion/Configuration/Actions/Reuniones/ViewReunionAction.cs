using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions.Reuniones
{
    public class ViewReunionAction:IAction
    {
        private string onclick;


        #region IAction Members

        public string GetName()
        {
            return "Detalle de la reunión";
        }

        public string GetAction()
        {
            return UIConfigurationHelper.GetActions().Actions.First(m => m.Key.Equals("Detalle")).GetAction(onclick, GetName());
            
        }

        public bool Validate(Dictionary<string, object> data)
        {
            bool valid = true;

            if (!data.ContainsKey(DataCommonKeys.USUARIO))
                throw new ArgumentException("Se esperaba el usuario");

            Usuario usuario = data[DataCommonKeys.USUARIO] as Usuario;

            var operaciones = usuario.GetOperaciones();

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Reuniones.Listar"));

            return valid;
        }

        #endregion

        public ViewReunionAction(string onclick)
        {
            this.onclick = onclick;
        }
    }
}
