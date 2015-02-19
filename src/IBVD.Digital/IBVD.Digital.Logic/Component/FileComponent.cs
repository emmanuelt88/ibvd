using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Globalization;
using System.Web;
using System.Configuration;
using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Logic.Component.Mapper;
using System.Web.Hosting;
using System.IO;
using IBVD.Digital.IBVD.Cache;
using IBVD.Digital.Files;
using IBVD.Digital.Logic.Helper;
using TSSA.Components.Media.ImageResizer;
using Ionic.Zip;
using IBVD.Digital.Common.Fault;
using IBVD.Digital.Files;

namespace IBVD.Digital.Logic.Component
{
    /// <summary>
    /// Manages temporal files
    /// </summary>
    public static class FileComponent
    {
        const int TIME_TO_PLACE_INTO_CACHE = 5;
        private static readonly string FileSize = ConfigurationManager.AppSettings["FILES.MAXUPLOADSIZE"];

        /// <summary>
        /// Default file size allowed for upload
        /// </summary>
        private static int MaxFileSize
        {
            get
            {
                int maxFileSize = 10 * 1024;
                if (!string.IsNullOrEmpty(FileSize))
                {
                    if (int.TryParse(FileSize, out maxFileSize))
                    {
                        maxFileSize = maxFileSize * 1024 * 1024;
                    }
                }
                return maxFileSize;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        public static string AddFileToCache(ArchivoDescargable archivo)
        {
            string key = System.Guid.NewGuid().ToString();
            Cache cache = GetCache();

            cache.Add(
                key,
                archivo, //value
                null, //dependencies
                DateTime.Now.AddMinutes(TIME_TO_PLACE_INTO_CACHE),
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null
                );

            return key;
        }

        /// <summary>
        /// Adds a file to temp cache
        /// </summary>
        /// <valueParameters name="file">the posted file</valueParameters>
        /// <returns>generated key to obtain file</returns>
        /// <exception cref="FileException"></exception>
        public static string AddContentFileToCache(string key, byte[] data)
        {
            Cache cache = GetCache();

            string key_content = string.Format(CultureInfo.InvariantCulture, "{0}", key);

            cache.Add(
                key_content,
                data, //value
                null, //dependencies
                DateTime.Now.AddMinutes(TIME_TO_PLACE_INTO_CACHE),
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                null
                );

            return key;
        }


        public static Cache GetCache()
        {
            return HttpRuntime.Cache;
        }

        public static byte[] GetFileContentFromCache(string key)
        {
            Cache cache = GetCache();
            byte[] data = (byte[])cache[key];
            return data;
        }

        public static ArchivoDescargable GetFileFromCache(string key)
        {
            Cache cache = GetCache();
            ArchivoDescargable file = (ArchivoDescargable)cache[key];
            return file;
        }

        public static bool ExistsFile(string key)
        {
            return GetCache()[key] != null;
        }


        public static void SaveFileData(string name, Guid guid, string contentType, int size)
        {
            ArchivoData file = new ArchivoData(guid, name, contentType, size);
            FilesMapper.SaveFile(file);
        }


        private static IFileCache _uploadsCache = new UploadsCache();


        public static CacheItem GetFileFromCache(Ticket ticket)
        {
            return _uploadsCache.Get(ticket);
        }

        private static IList<ArchivoData> GetFilesData()
        {
            return FilesMapper.GetFiles();
        }

        public static void ClearTempFiles()
        {
            var files = GetFilesData();
            string tempFolder = HostingEnvironment.MapPath("~/Files");
            //string tempPath = GetDiskLocation(tempFolder, item);

            var filesTemp = Directory.GetFiles(tempFolder);
            
            //var filesToDelete = new List<string>();
            foreach (var item in filesTemp)
            {
                string fileName = item.Replace(tempFolder, string.Empty).Replace("\\",string.Empty);
                if (files.FirstOrDefault(m => m.Id.ToString().Equals(fileName)) == null)
                {
                    //filesToDelete.Add(item);

                    var creationTime = File.GetCreationTime(item);

                    var comparation = DateTime.Now.AddHours(-1).CompareTo(creationTime);
                    if (comparation == 1 || comparation == 0 )
                    {
                        File.Delete(item);
                    }
                    
                   
                }
            }

            

        }

        public static Ticket AddFileToCache(HttpPostedFileAdapter postedFile)
        {
            string[] fileDescription = postedFile.FileName.Split('.');
            string description = string.Empty;
            string extension = fileDescription[fileDescription.Length-1];
            for (int i = 0; i < fileDescription.Length-1; i++)
			{
                description+=description;
                if(i < fileDescription.Length-2){
                    description+=".";
                }
			}
            IFile file = new Archivo(postedFile.FileName,
                description,
                postedFile.Content,
                postedFile.ContentType,
                extension);
            Ticket ticket = Ticket.Create();
            postedFile.CacheKey = ticket.Value;

            _uploadsCache.Add(new CacheItem(file, ticket));

            return ticket;
        }



        public static string SaveFile(Ticket ticket, byte[] fileContent)
        {
            return SaveFile(ticket, fileContent, "Files");
        }

        public static string SaveFile(Ticket ticket, byte[] fileContent, string directorio)
        {
            string folder = IOHelper.GetDiskLocation(directorio);
            IOHelper.CheckAndCreateFolder(folder);

            var item = GetFileFromCache(ticket);
            var aux = item.Name.Split('.');
            string extension = aux[aux.Length - 1];
            string path = IOHelper.ConbinePath(folder, ticket.Value + "." + extension);
            
            string fullPath = directorio + "/" + ticket.Value + "." + extension;
            FileSystemFile.Save(new Archivo(item.Name, item.Description, fileContent, item.ContentType, extension),path);

            return fullPath;
        }

        public static string SaveFile(string name, byte[] fileContent, string contentType, string directorio, string extension)
        {
            string folder = IOHelper.GetDiskLocation(directorio);
            IOHelper.CheckAndCreateFolder(folder);

            string path = IOHelper.ConbinePath(folder, name + "." + extension);
            
            FileSystemFile.Save(new Archivo(name, name, fileContent, contentType, extension), path);

            return path;
        }

        public static string SaveFile(Ticket ticket)
        {
            var item = GetFileFromCache(ticket);
            return SaveFile(ticket, item.Content);
        }



        public static string SaveProfileImage(Ticket ticket, byte[] imageResized)
        {
            return SaveFile(ticket, imageResized, ConfigurationHelper.DirectorioImagenesPerfil);
        }
        public static ArchivoData GetFile(string id)
        {
            return FilesMapper.GetFile(new Guid(id));
        }

        public static IList<string> ListImagesInFolder(string directorio)
        {
            IList<string> archivos = new List<string>();
            string folder = IOHelper.GetDiskLocation(directorio);
            IOHelper.CheckAndCreateFolder(folder);
            archivos = Directory.GetFiles(folder,"*.jpg", SearchOption.AllDirectories).ToList();
            foreach (var item in Directory.GetFiles(folder,"*.png", SearchOption.AllDirectories).ToList())
            {
                archivos.Add(item);
            }

            return archivos.Select(m => m.Replace(HostingEnvironment.MapPath("~/"), string.Empty).Replace("\\","/")).ToList();
        }

        public static IList<string> ListFoldersInFolder(string directorio)
        {
            IList<string> directorios = new List<string>();
            string folder = IOHelper.GetDiskLocation(directorio);
            IOHelper.CheckAndCreateFolder(folder);
            directorios = Directory.GetDirectories(folder, "*", SearchOption.AllDirectories).ToList();

            return directorios.Select(m => "/" + m.Replace(IOHelper.GetRootDisk(), string.Empty).Replace("\\", "/")).ToList();
        }

        public static void SaveImagenes(IDictionary<string, KeyValuePair<string, bool>> data, IList<string> carpetasBorradas)
        {
            string root = ConfigurationHelper.DirectorioImagenesRoot;
            var nuevos = data.Where(m => m.Value.Value).ToList();
            var actualizaciones = data.Where(m => !m.Value.Value).ToList();
            var imagenes = ListImagesInFolder(root);
            
            foreach (var item in nuevos)
            {
                string directorio = item.Value.Key.Replace("_", "/");
                
                string keyImagenMiniatura = item.Key + ConfigurationHelper.ImagenMiniatura;
                CacheItem imagen = GetFileFromCache(Ticket.Create(item.Key));
                byte[] imagenMiniatura = GetFileContentFromCache(keyImagenMiniatura);
                byte[] imagenNormalizada = ImageHelper.NormalizeImage(imagen.Content, ConfigurationHelper.ImagenAnchoMax);

                if (!item.Value.Key.Equals("Papelera"))
                {
                    SaveFile(item.Key, imagenNormalizada, imagen.ContentType, directorio, imagen.Extension);
                    SaveFile(keyImagenMiniatura, imagenMiniatura, imagen.ContentType, directorio, imagen.Extension);
                }
            }

            foreach (var item in actualizaciones)
            {
                string keyImagenMiniatura = item.Key;
                string directoryOrigen = imagenes.FirstOrDefault(m=> m.Contains(keyImagenMiniatura));
                directoryOrigen = HostingEnvironment.MapPath("~/" + directoryOrigen);
                FileStream file= File.Open(directoryOrigen, FileMode.Open);

                string extension = string.Empty;
                directoryOrigen = GetFileWithoutExtension(file.Name, out extension).Replace(keyImagenMiniatura, string.Empty);
                string fileNameMinuatura = keyImagenMiniatura + "." + extension;
                string fileName = fileNameMinuatura.Replace(ConfigurationHelper.ImagenMiniatura, string.Empty);
                file.Close();

                if (item.Value.Key.Equals("Papelera"))
                {
                    File.Delete(directoryOrigen + fileName);
                    File.Delete(directoryOrigen + fileNameMinuatura);
                }
                else
                {
                    string directorio = IOHelper.GetDiskLocation("~/" + item.Value.Key.Replace("_", "/"));

                    File.Move(directoryOrigen + fileName, IOHelper.ConbinePath(directorio, fileName));
                    File.Move(directoryOrigen + fileNameMinuatura, IOHelper.ConbinePath(directorio, fileNameMinuatura));
                }

            }

            foreach (var carpeta in carpetasBorradas)
            {
                Directory.Delete(IOHelper.GetDiskLocation(carpeta), true);
            }

        }

        

        private static string GetFileWithoutExtension(string file, out string extension)
        {
            string[] items = file.Split('.');
            string fileExtension = items[items.Length - 1];
            extension = fileExtension;
            return file.Replace("." + fileExtension, string.Empty);
        }

        public static System.Web.Mvc.FileContentResult GetDirectoryImages(string root)
        {

            string folder = IOHelper.GetDiskLocation(root);
            IOHelper.CheckAndCreateFolder(folder);

            Ionic.Zip.ZipFile fileZip = new ZipFile();
            string[] directorios = Directory.GetDirectories(folder);
            MemoryStream output = new MemoryStream();
            
            fileZip.AddDirectory(folder);
            var miniaturas = fileZip.Entries.Where(m => m.FileName.Contains(ConfigurationHelper.ImagenMiniatura)).ToList();

            foreach (var item in miniaturas)
            {
                fileZip.RemoveEntry(item.FileName);
            }
            fileZip.Save(output);
            var result = new System.Web.Mvc.FileContentResult(output.ToArray(), "application/octet-stream");
            result.FileDownloadName = "IBVD_Imagenes.zip";
            return result; 
        }



        public static void CrearFolder(string name)
        {
            string root = ConfigurationHelper.DirectorioImagenesRoot;
            string path = IOHelper.GetDiskLocation(root) + "/" + name;
            if (Directory.Exists(path))
            {
                throw new HandleException("El directorio ya existe");
            }

            Directory.CreateDirectory(path);
        }
    }
}
