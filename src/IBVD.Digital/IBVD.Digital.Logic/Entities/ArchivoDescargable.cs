using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    public class ArchivoDescargable
    {
        /// <summary>
        /// Gets a System.IO.Stream object that points to an uploaded file to prepare for reading the
        /// contents of the file
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets the size of an uploaded file, in bytes
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        /// Gets the MIME content type of the file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets the fully qualified name of the file in the client
        /// </summary>
        public string FileName { get; set; }
    }
}
