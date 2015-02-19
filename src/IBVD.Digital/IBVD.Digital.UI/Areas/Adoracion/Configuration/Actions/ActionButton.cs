using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Actions
{
    [XmlType("ActionButton")]
    public class ActionButton
    {
        [XmlAttribute("Value")]
        public string Value { get; set; }
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Key")]
        public string Key { get; set; }

        internal string GetAction(string onclick, string title)
        {
            string button = string.Empty;
            switch(Type)
            {
                case "Button":
                    {
                        button = string.Format("<a href='#' class='button' onclick='{0}' title='{1}'><span class='{2}'></span></a>", onclick, title, Value);
                    } break;
            }

            return button;
        }
    }
}
