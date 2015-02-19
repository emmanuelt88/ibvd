using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions.Canciones
{
    public class EditCancionAction:IAction
    {
        private string onclick;

        #region IAction Members

        public string GetName()
        {
            return "Editar la canción";
        }

        public string GetAction()
        {
            return UIConfigurationHelper.GetActions().Actions.First(m => m.Key.Equals("Editar")).GetAction(onclick, GetName());
        }

        public bool Validate(Dictionary<string, object> data)
        {

            bool valid = true;

            if (!data.ContainsKey(DataCommonKeys.USUARIO))
                throw new ArgumentException("Se esperaba el usuario");

            Usuario usuario = data[DataCommonKeys.USUARIO] as Usuario;

            var operaciones = usuario.GetOperaciones();

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Canciones.Crear"));

            return valid;
        }

        #endregion

        public EditCancionAction(string onclick)
        {
            this.onclick = onclick;
        }
    }
}
