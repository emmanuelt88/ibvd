using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace IBVD.Digital.Logic.Configuration.Model
{
    public class MenuItem
    {
        [XmlAttribute("Path")]
        public string Path { get; set; }
        [XmlAttribute("Title")]
        public string Title { get; set; }
        [XmlAttribute("Permission")]
        public string Permission { get; set; }
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlArray(ElementName = "Childrens")]
        [XmlArrayItem(ElementName = "MenuItem")]
        public MenuItem[] Items { get; set; }
    }
}
