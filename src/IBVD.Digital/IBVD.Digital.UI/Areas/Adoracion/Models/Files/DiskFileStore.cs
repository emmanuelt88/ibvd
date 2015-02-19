﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Hosting;

namespace IBVD.Digital.UI.Areas.Adoracion.Models.Files
{
    public class DiskFileStore:IFileStore
    {
        private string _uploadsFolder = HostingEnvironment.MapPath("~/Files");

        public Guid SaveUploadedFile(HttpPostedFileBase fileBase)
        {
            
            var identifier = Guid.NewGuid();
            fileBase.SaveAs(GetDiskLocation(identifier));
            return identifier;
        }

        private string GetDiskLocation(Guid identifier)
        {
            return Path.Combine(_uploadsFolder, identifier.ToString());
        }
    }
}
