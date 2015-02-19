using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Text;
using IBVD.Digital.Logic.Entities;

namespace IBVD.Digital.UI.Areas.Adoracion.Configuration.Model
{
    public class MenuItem
    {
        private const string templateItem = "<li><a href='{0}' class='{1}'><span>{2}</span></a>{3}</li>";
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

        public bool HasChildren()
        {
            return Items != null && Items.Count() > 0;
        }

        public void CheckPermission(IList<Operacion> operaciones)
        {
            HasPermission = string.IsNullOrEmpty(Permission) || operaciones.FirstOrDefault(m => m.Codigo.Equals(Permission)) != null;

            if (HasChildren())
            {
                foreach (var item in Items)
                {
                    item.CheckPermission(operaciones);
                }
            }
        }

        public bool HasPermission { get; set; }
        public string GetHTML()
        {
            if (!HasTreePermission())
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            StringBuilder children = new StringBuilder();

            string classItem = (HasChildren()) ? "parent" : string.Empty;

            if (HasChildren())
            {
                children.Append("<ul>");
                foreach (var item in Items.Where(m=> m.HasPermission))
                {
                    children.Append(item.GetHTML());
                }
                children.Append("</ul>");
            }

            builder.AppendFormat(templateItem, Path, classItem, Title, children.ToString());


            return builder.ToString();
        }

        public bool HasTreePermission()
        {
            if (HasChildren())
            {
                return this.Items.Where(m => m.HasTreePermission()).Count() > 0;
            }
            else
            {
                return HasPermission;
            }
        }

        public MenuItem()
        {
            HasPermission = true;
        }
    }
}
