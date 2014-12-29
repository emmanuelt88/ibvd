using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IBVD.Digital.UI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/

        public ActionResult Index(int code, string urlOriginal)
        {
            string[] errorMessages = new string[]{};
            string errorMessage = code + " - Se produjo un error inesperado";
            switch (code)
            {
                case 0:
                    {
                        errorMessages = Session["ErrorMessages"] != null ? (string[])Session["ErrorMessages"] : errorMessages;
                        errorMessage = Session["ErrorMessage"] != null ? (string)Session["ErrorMessage"] : errorMessage;
                    };
                    break;
                case 404:
                    errorMessage = code + "La página o recurso solicitado no existe";
                    break;
                default:
                    break;
            }

            ViewBag.ErrorMessage = errorMessage;
            ViewBag.ErrorMessages = errorMessages;
            return View("Error");
        }

    }
}
