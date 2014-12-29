using IBVD.Digital.UI.Filters;
using System.Web;
using System.Web.Mvc;

namespace IBVD.Digital.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new CustomHandleErrorAttribute() { View = "~/Error?" });
        }
    }
}