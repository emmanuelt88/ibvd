using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Logic.Entities
{
    public class ArchivoData
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public int Size { get; private set; }
        public DateTime Fecha { get; private set; }
        public string FullPath { get; set; }
        public ArchivoData(Guid id, string fileName, string contentType, int size)
            :this(id, fileName, contentType, size, DateTime.Now)
        {
            
        }

        public ArchivoData(Guid id, string fileName, string contentType, int size, DateTime fecha)
        {
            this.Id = id;
            this.FileName = fileName;
            this.ContentType = contentType;
            this.Size = size;
            this.Fecha = fecha;
        }

        public ArchivoData(Guid id, string fileName, string contentType, int size, DateTime fecha, string fullPath)
            : this(id, fileName, contentType, size, DateTime.Now)
        {
            this.FullPath = fullPath;
        }


        public ArchivoData()
        {
            this.Id = Guid.Empty;
            this.FileName = string.Empty;
            this.ContentType = string.Empty;
            this.Size = 0;
        }

    }
}
