using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Model
{
    public class KeyValueItem
    {
        [XmlAttribute("Key")]
        public string Key { get; set; }

        [XmlAttribute("Value")]
        public string Value { get; set; }

        [XmlAttribute("Default")]
        public bool Default { get; set; }
    }
}
