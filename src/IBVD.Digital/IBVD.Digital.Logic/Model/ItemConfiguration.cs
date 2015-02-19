using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IBVD.Digital.Logic.Configuration.Model
{
    public class ItemConfiguration
    {
        
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray(ElementName = "Items")]
        [XmlArrayItem(ElementName = "KeyValueItem")]
        public KeyValueItem[] Items { get; set; }
    }
}
