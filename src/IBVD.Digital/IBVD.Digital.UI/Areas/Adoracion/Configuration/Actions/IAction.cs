using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions
{
    interface IAction
    {

        string GetName();

        string GetAction();

        bool Validate(Dictionary<string, object> data);
    }
}
