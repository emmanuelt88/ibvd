using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using IBVD.Digital.Logic.Configuration.Model;

namespace IBVD.Digital.Logic.Configuration.ItemsConfiguration
{
    [XmlType("ItemsConfiguration")]
    public class ItemsConfiguration
    {
        [XmlArray(ElementName = "Childrens")]
        [XmlArrayItem(ElementName = "ItemConfiguration")]
        public ItemConfiguration[] Items { get; set; }
    }
}
