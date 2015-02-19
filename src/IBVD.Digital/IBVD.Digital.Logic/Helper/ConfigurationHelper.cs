using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IBVD.Digital.Logic.Configuration.ItemsConfiguration;
using IBVD.Digital.Logic.Configuration.Model;
using System.Xml;
using System.Xml.Serialization;

namespace IBVD.Digital.Logic.Helper
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool NotificarCambiosReunion()
        {
            return bool.Parse(ConfigurationManager.AppSettings["NotificacionesReunion"]);
        }

        public static string ImagenMiniatura
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.IMAGEN_MINIATURA]; }
        }

        public static string DirectorioImagenesRoot
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_IMAGENES_ROOT]; }
        }

        public static int ImagenAnchoMax
        {
            get { return int.Parse(ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.IMAGEN_ANCHO_MAX]); }
        }

        public static string RootImagenNotFound
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.IMAGEN_NO_ENCONTRADA]; }
        }

        public static int Reunion_Canciones_MaxDuracion
        {
            get { return int.Parse(ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.REUNION_CANCIONES_MAX]); }
        }

        public static string Cancion_FormatData
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.CANCION_FORMAT_DATA_BASICO]; }
        }

        public static string DirectorioImagenesPerfil
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_IMAGENES_PERFIL]; }
        }

        public static string DirectorioBiblias
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_BIBLIAS]; }
        }

        public static string DirectorioMailTemplates
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATES]; }
        }

        public static string DirectorioMailTemplateBase
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_BASE]; }
        }

        public static string DirectorioMailTemplateRegistracionUsuario
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_REGISTRACION_EMAIL]; }
        }

        public static string DirectorioMailTemplateRecuperacionPassword
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_RECUPERACION_PASSWORD]; }
        }

        public static string DirectorioMailTemplateModificacionReunion
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_REUNION_MODIFICACION]; }
        }

        public static string DirectorioMailTemplateCancelacionReunion
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_REUNION_CANCELACION]; }
        }

        public static string DirectorioMailTemplateActualizacionEncargadosCultos
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_ACTUALIZACION_ENCARGADOS]; }
        }

        public static string DirectorioMailTemplateActualizacionEncargadosCultos_ITEM
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_ACTUALIZACION_ENCARGADOS_ITEM]; }
        }


        
        public static string DirectorioMailTemplateInformativoConLink
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.DIRECTORIO_MAIL_TEMPLATE_INFORMATIVO_LINK]; }
        }
        public static string SiteRoot
        {
            get { return ConfigurationManager.AppSettings[ConfigurationWellKnowKeys.SITE_ROOT]; }
        }

        public static IList<KeyValueItem> GetItemsStyle()
        {
            ItemsConfiguration items = Deserialize<ItemsConfiguration>(ResolvePath("ItemsConfiguration"));

            return items.Items.Where(m => m.Name.Equals("EstilosConfiguration")).FirstOrDefault().Items.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string path)
        {
            XmlDocument doc = new XmlDocument();
            XmlReader reader = XmlReader.Create(path);


            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T element = (T)serializer.Deserialize(reader);

            reader.Close();

            return element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static string ResolvePath(string pathName)
        {
            string fullPath = string.Empty;

            if (ConfigurationManager.AppSettings["IsSite"].Equals("true"))
            {
                fullPath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[pathName]);
            }
            else
            {
                fullPath = ConfigurationManager.AppSettings[pathName];
            }
            return fullPath;
        }
    }
}
