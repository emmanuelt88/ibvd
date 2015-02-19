using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class JSONResultModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Action { get; set; }
        private string error;
        public string Error
        {
            get
            {
                return error;
            }
            set
            {
                Success = false;
                error = value;
            }
        }
        private string[] errores;
        public string[] Errores
        {
            get
            {
                return errores;
            }
            set
            {
                ShowPopup = false;
                Success = false;
                errores = value;
            }
        }

        public bool ShowPopup { get; set; }
        public object Data { get; set; }

        public JSONResultModel()
        {
            Success = true;
        }

        public JSONResultModel(string message, object data)
        {
            this.Success = true;
            this.Message = message;
            this.Data = data;
            Success = true;
        }

        public JSONResultModel(string error, bool showPopup, object data)
        {
            this.Success = false;
            this.ShowPopup = showPopup;
            this.Error = error;
            this.Data = data;
        }

        public JSONResultModel(string error, string[] errores, bool showPopup, object data)
        {
            this.ShowPopup = showPopup;
            this.Error =error;
            this.Errores = errores;
            this.Success = false;
            this.Data = data;
        }
    }
}
