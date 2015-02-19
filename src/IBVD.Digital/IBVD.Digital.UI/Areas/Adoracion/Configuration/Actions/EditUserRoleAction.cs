using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using IBVD.Digital.Common;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions
{
    public class EditUserRoleAction:IAction
    {
        private string onclick;

        public string GetName()
        {
            return "Modificar los roles del usuario";
        }

        public string GetAction()
        {
            return UIConfigurationHelper.GetActions().Actions.First(m => m.Key.Equals("Editar")).GetAction(onclick,GetName());
        }

        public bool Validate(Dictionary<string, object> data)
        {
             bool valid = true;

            if(!data.ContainsKey(DataCommonKeys.USUARIO))
                throw new ArgumentException("Se esperaba el usuario");

            Usuario usuario = data[DataCommonKeys.USUARIO] as Usuario;

            var operaciones = usuario.GetOperaciones();

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Administracion.ModificarRoleUsuario"));

            return valid;
        }

         public EditUserRoleAction(string onclick)
        {
            this.onclick = onclick;                        
        }
    }
}
