using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions.Canciones
{
    public class ViewCancionAction:IAction
    {
        private string onclick;


        #region IAction Members

        public string GetName()
        {
            return "Detalle de la canción";
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

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Canciones.Listar"));

            return valid;
        }

        #endregion

        public ViewCancionAction(string onclick)
        {
            this.onclick = onclick;
        }
    }
}
