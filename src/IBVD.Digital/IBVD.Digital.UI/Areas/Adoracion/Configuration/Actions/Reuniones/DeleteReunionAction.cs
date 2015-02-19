using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Common;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions.Reuniones
{
    public class DeleteReunionAction : IAction
    {
        private string onclick;

        #region IAction Members

        public string GetName()
        {
            return "Eliminar la canción";
        }

        public string GetAction()
        {
            return UIConfigurationHelper.GetActions().Actions.First(m => m.Key.Equals("Eliminar")).GetAction(onclick, GetName());
            
        }

        public bool Validate(Dictionary<string, object> data)
        {
            bool valid = true;

            if (!data.ContainsKey(DataCommonKeys.USUARIO))
                throw new ArgumentException("Se esperaba el usuario");

            Usuario usuario = data[DataCommonKeys.USUARIO] as Usuario;

            var operaciones = usuario.GetOperaciones();

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Canciones.Eliminar"));

            return valid;
        }

        #endregion

        public DeleteReunionAction(string onclick)
        {
            this.onclick = onclick;
        }
    }
}
