using IBVD.Digital.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace IBVD.Digital.UI.Controllers
{
    public class HomeController : Controller
    {
        private MailSender mailSender = new MailSender();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ViewBag.FacebookURL = ConfigurationManager.AppSettings["Facebook.URL"];
            ViewBag.WebSiteURL = ConfigurationManager.AppSettings["Website.URL"];

            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //
        // POST: /Home/Contactar

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Contactar(string fullname, string email, string message)
        {
            if (string.IsNullOrEmpty(fullname))
            {
                ModelState.AddModelError("fullname", "El nombre debe ser completado");    
            }

            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("email", "El email debe ser completado");
            }

            if (string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("message", "Debe ingresar un mensaje");
            }

            if (ModelState.IsValid)
            {

                message = fullname + " ha enviado la siguiente consulta: <br><br>" + message;
                message = message + "<br><br> Su correo: " + email;

                mailSender.Send("IBVD - Nueva consulta ", message);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["ErrorMessage"] = "Los datos para enviar la consulta son inválidos.";
                Session["ErrorMessages"] = ModelState.SelectMany(m => m.Value.Errors).Select(m=> m.ErrorMessage).ToArray();
                return RedirectToAction("Index", "Error", new { code = 0});    
            }
            
        }

    }
}
