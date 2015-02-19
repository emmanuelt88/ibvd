using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Xml;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using System.Text;
using IBVD.Digital.UI.Areas.Adoracion.Helpers.Session;
using System.Net;
using IBVD.Digital.UI.Areas.Adoracion.Models;

namespace ContentHelper
{
    public static class CurrentDirectory
    {
        public static string CurrentTheme()
        {
            return @"/Content/Themes/" + ConfigurationManager.AppSettings["CurrentTheme"];
        }
        public static string ThemesFiles()
        {
            if (HttpContext.Current.Session[UICommonKeys.MENU_SESSION] != null)
            {
                return HttpContext.Current.Session[UICommonKeys.MENU_SESSION].ToString();
            }

            StringBuilder themeFiles = new StringBuilder();
            string themeDirectory = @"/Content/Themes/base";
            XmlTextReader reader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath(themeDirectory + "/Files.xml"));
            XmlDocument document = new XmlDocument();
            document.Load(reader);

            XmlNodeList listFilesMyProperty =  document.GetElementsByTagName("cssFile");

            foreach (XmlNode item in listFilesMyProperty)
            {
                themeFiles.AppendLine(string.Format("<link href='{0}/{1}' rel='stylesheet' type='text/css' />", themeDirectory, item.InnerText));
            }


            themeDirectory = @"/Content/Themes/" + ConfigurationManager.AppSettings["CurrentTheme"];
            reader = new XmlTextReader(System.Web.HttpContext.Current.Server.MapPath(themeDirectory + "/Files.xml"));
            document = new XmlDocument();
            document.Load(reader);

            listFilesMyProperty =  document.GetElementsByTagName("cssFile");

            foreach (XmlNode item in listFilesMyProperty)
            {
                themeFiles.AppendLine(string.Format("<link href='{0}/{1}' rel='stylesheet' type='text/css' />", themeDirectory, item.InnerText));
            }

            HttpContext.Current.Session[UICommonKeys.MENU_SESSION] = themeFiles.ToString();
                
            return themeFiles.ToString();
        }

        public static string ScriptsDir()
        {
            return ConfigurationManager.AppSettings["CurrentScripts"];
        }

        public static string GetImage(string name, string parametros)
        {
            string img = string.Format("<img src='{0}/{1}' {2} />", CurrentTheme(), name, parametros);
            return img;
        }

        public static string GetScript(string name)
        {
            string script = string.Format("<script type='text/javascript' src='{0}/{1}'></script>", ScriptsDir(), name);

            return script;

        }

    }
}
