using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBVD.Digital.UI.Areas.Adoracion.Models.Files
{
    public interface IFileStore
    {
        Guid SaveUploadedFile(HttpPostedFileBase fileBase);
    }
}
