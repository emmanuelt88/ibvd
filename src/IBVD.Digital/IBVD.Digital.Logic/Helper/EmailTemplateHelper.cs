using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IBVD.Digital.Logic.Helper
{
    public static class EmailTemplateHelper
    {
        public static string GetHtml(string titulo, string body)
        {
            string templateFileName = IOHelper.GetDiskLocation(ConfigurationHelper.DirectorioMailTemplateBase);

            StreamReader file = new StreamReader(templateFileName);

            string template = file.ReadToEnd();

            template = template.Replace("{mailContent}", body);
            template = template.Replace("{mailTitulo}", titulo);

            var items = ConfigurationHelper.GetItemsStyle();

            foreach (var item in items)
            {
                template = template.Replace(item.Key, item.Value);
            }

            return template;
        }

        public static string FillTemplate(string titulo, string templateRelativePath, IDictionary<string, string> data)
        {
            string templateFileName = IOHelper.GetDiskLocation(templateRelativePath);
            StreamReader file = new StreamReader(templateFileName);

            string template = file.ReadToEnd();

            foreach (string key in data.Keys)
            {
                template = template.Replace(key, data[key]);
            }

            string fullContent = GetHtml(titulo, template);

            return fullContent;
        }

        public static string FillSimpleTemplate(string templateRelativePath, IDictionary<string, string> data)
        {
            string templateFileName = IOHelper.GetDiskLocation(templateRelativePath);
            StreamReader file = new StreamReader(templateFileName);

            string template = file.ReadToEnd();

            foreach (string key in data.Keys)
            {
                template = template.Replace(key, data[key]);
            }


            return template;
        }

        internal static string FillTemplateInformativoLink(string p, string p_2)
        {
            throw new NotImplementedException();
        }
    }
}
