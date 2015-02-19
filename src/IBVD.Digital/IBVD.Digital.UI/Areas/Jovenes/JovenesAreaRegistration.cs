using System.Web.Mvc;

namespace IBVD.Digital.UI.Areas.Jovenes
{
    public class JovenesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Jovenes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Jovenes_default",
                "Jovenes/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IBVD.Digital.UI.Areas.Jovenes.Controllers" }
            );
        }
    }
}
