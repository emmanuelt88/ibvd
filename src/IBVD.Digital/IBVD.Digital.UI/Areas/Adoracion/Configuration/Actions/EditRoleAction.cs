using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions
{
    public class EditRoleAction:IAction
    {
        private string onclick;

        public string GetName()
        {
            return "Modificar permisos del rol";
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

            valid = valid && operaciones.Exists(m => m.Codigo.Equals("Administracion.ModificarRole"));

            return valid;
        }

        public EditRoleAction(string onclick)
        {
            this.onclick = onclick;                        
        }
    }
}
