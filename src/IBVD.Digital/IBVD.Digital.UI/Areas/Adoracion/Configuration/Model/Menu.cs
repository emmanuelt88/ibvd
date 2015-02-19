using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Model
{
    [XmlType("Menu")]
    public class Menu
    {
        [XmlArray(ElementName = "Items")]
        [XmlArrayItem(ElementName = "MenuItem")]
        public MenuItem[] Items { get; set; }
    }
}
