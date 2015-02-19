using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using IBVD.Digital.UI.Areas.Adoracion.Controllers.Attributes;
using IBVD.Digital.UI.Areas.Adoracion.Helpers.Session;
using IBVD.Digital.Logic.Component;
using IBVD.Digital.Common.Notifications;
using IBVD.Digital.Logic.Entities;
using MVCSecrets.Helpers;

namespace IBVD.Digital.UI.Areas.Adoracion.Controllers
{
    
    public class HomeController : BackendController
    {
        public ActionResult Index()
        {
            string resultado = Url.Action<HomeController>(m => m.Index());
            SetTitle("Iglesia Bautista de Villa Domínico");
            return View();
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult GetAll()
        {
            List<Cancion> canciones = new List<Cancion>();


            return Json(
                new
                {
                     total=34, 
  page= 3, 
  records= 3434,
                     rows = from a in canciones     // Datos de filas
                            select new
                            {
                                id = a.Id,                // ID único de la fila
                                cell = new string[] {     // Array de celdas de la fila
                       a.Id.ToString()                             // Primera columna,            
                   }
                            }
                }, JsonRequestBehavior.AllowGet);
        }
    }
}
