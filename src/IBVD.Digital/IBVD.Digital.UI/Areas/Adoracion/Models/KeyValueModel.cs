using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class KeyValueModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValueModel(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public KeyValueModel()
        {
        }
    }
}
