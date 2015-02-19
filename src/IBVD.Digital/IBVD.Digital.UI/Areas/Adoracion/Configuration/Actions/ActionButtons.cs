using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions
{
    [XmlRoot(ElementName = "ActionsButton")]
    public class ActionButtons
    {
        [XmlArray(ElementName = "Childrens")]
        [XmlArrayItem(ElementName = "ActionButton")]
        public ActionButton[] Actions { get; set; }
    }
}
