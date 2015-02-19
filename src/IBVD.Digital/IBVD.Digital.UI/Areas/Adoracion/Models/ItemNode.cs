using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    /// <summary>
    /// Item
    /// </summary>
    public class ItemNode
    {
        public int Id { get; set; }

        public string Title { get; set; }

        private bool IsPrimary 
        {
            get { return this.Childrens.Count > 0; }
        }

        public bool IsSelected { get; set; }

        public string KeyParent { get; set; }

        public string Key { get; set; }
        /// <summary>
        /// Lista de nodos hijos
        /// </summary>
        public List<ItemNode> Childrens { get; set; }

        public ItemNode(int id, string title, bool isSelected,string keyParent, string key)
        {
            this.Id = id;
            this.Title = title;
            this.IsSelected = isSelected;
            this.KeyParent = keyParent;
            this.Key = key;
            this.Childrens = new List<ItemNode>();
        }

        public string GetItemTreeView()
        {
            StringBuilder builder = new StringBuilder();

            if (!IsPrimary)
                return string.Format("<li class='handHover'><input type='checkbox' {0} id='{1}' value='{1}'/><label  for='{1}'>{2}</label></li>", this.IsSelected ? "checked" : string.Empty, this.Id, this.Title);

            if (IsPrimary && this.Childrens.Count(m=> m.IsSelected) == 0)
                builder.Append("<li class='closed'>");
            else
                builder.Append("<li>");

            builder.Append(string.Format("<span class='folder'><label>{0}</label></span>", this.Title));
            builder.Append("<ul>");
            foreach (var item in this.Childrens)
            {
                builder.Append(item.GetItemTreeView());
            }

            builder.Append("</ul>");
            builder.Append("</li>");
            
            return builder.ToString();
        }

        
        public void LoadChildrens(IList<ItemNode> childrenList)
        {
            foreach (var item in childrenList.Where(m=> m.KeyParent.Equals(this.Key)))
            {
                this.Childrens.Add(item);
            }

            foreach (var item in this.Childrens)
            {
                item.LoadChildrens(childrenList);
            }
        }
    }
}
