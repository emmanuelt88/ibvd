using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBVD.Digital.Logic.Entities;
using System.Web.Mvc;
using IBVD.Digital.UI.Areas.Adoracion.Configuration.Model;
using IBVD.Digital.UI.Areas.Adoracion.Helpers;
using IBVD.Digital.UI.Areas.Adoracion.Configuration.ItemsConfiguration;
using IBVD.Digital.Logic.Helper;
using System.IO;
using IBVD.Digital.Logic.Component;

namespace IBVD.Digital.UI.Areas.Adoracion.Models
{
    public class CancionViewData
    {
        public Cancion Cancion { get; set; }
        public bool ShowCrearButton { get; set; }
        public bool ShowEditarButton { get; set; }
        public bool ShowEliminarButton { get; set; }

        public string CancionPath 
        {
            get 
            {
                string diskFileLocation = string.Empty;
                string virtualPath = "/Images/SinImagen.jpg";

                try
                {
                    if (Cancion != null && !string.IsNullOrEmpty(Cancion.FotoURI))
                    {
                        diskFileLocation = IOHelper.GetDiskLocation(Cancion.FotoURI);

                        if (File.Exists(diskFileLocation))
                            virtualPath = Cancion.FotoURI;
                    }

                }
                catch (Exception ex)
                {
                    LoggingComponent.LogError("Error al obtener la uri de la imagen de la cancion", ex);
                }


                return virtualPath;
            }
        }
        public DirectorioViewData DirectorioData { get; set; }
        private IList<KeyValueItem> tonos;
        private IList<KeyValueItem> compaces;
        public SelectList Tonos 
        {
            get
            {
                if (Cancion != null)
                {
                    return new SelectList(tonos, "Value", "Key", Cancion.Tono);
                }

                return new SelectList(tonos, "Value", "Key", tonos.Single(m => m.Default).Key);
            }
        }

        public SelectList Compaces
        {
            get
            {
                if (Cancion != null)
                {
                    return new SelectList(compaces, "Value", "Key", Cancion.Compas);
                }

                return new SelectList(compaces, "Value", "Key", compaces.Single(m => m.Default).Key);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CancionViewData()
        {
            Cancion = new Cancion();
            tonos = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.TONOS).Items.OrderBy(m=> m.Value).ToList();
            compaces = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.COMPACES).Items.OrderBy(m => m.Value).ToList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CancionViewData(Cancion cancion, DirectorioViewData viewData)
        {
            Cancion = cancion;
            tonos = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.TONOS).Items.OrderBy(m => m.Value).ToList();
            compaces = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.COMPACES).Items.OrderBy(m => m.Value).ToList();
            DirectorioData = viewData;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CancionViewData(Cancion cancion)
        {
            Cancion = cancion;
            tonos = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.TONOS).Items.OrderBy(m => m.Value).ToList();
            compaces = UIConfigurationHelper.GetItemConfigurationByName(ItemsConfigurationEnum.COMPACES).Items.OrderBy(m => m.Value).ToList();
        }



    }
}
