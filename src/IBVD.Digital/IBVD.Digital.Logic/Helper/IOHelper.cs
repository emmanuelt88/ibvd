using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Hosting;

namespace IBVD.Digital.Logic.Helper
{
    public static class IOHelper
    {
        public static void CheckAndCreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static string GetDiskLocation(string sitePath)
        {
            return HostingEnvironment.MapPath(sitePath);
        }

        public static string GetDiskLocation(string siteDirectory, string fileName)
        {
            return HostingEnvironment.MapPath(Path.Combine(siteDirectory, fileName));
        }

        public static string ConbinePath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }

        public static string GetFileName(string fullPath)
        {
            string[] items = fullPath.Split('/');
            string fileName = items[items.Length - 1];

            return fileName;
        }

        internal static string GetRootDisk()
        {
            return HostingEnvironment.MapPath("~/");
        }
    }
}
