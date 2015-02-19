using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBVD.Digital.Files;

namespace IBVD.Digital.Logic.Entities
{
    public class Archivo:IFile
    {
      /// <summary>
        /// Default constructor
        /// </summary>
        public Archivo()
        {
            this.Id = -1;
            this.Extension = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file"></param>
        public Archivo(IFile file):base()
        {
            this.Id = file.Id;
            if (this.Id == 0)
                this.Id = -1;
            this.Name = file.Name;
            this.Description = file.Description;
            this.Content = file.Content;
            this.ContentLength = file.ContentLength;
            this.ContentType = file.ContentType;
            this.Extension = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <param name="extension"></param>
        public Archivo(string name, string description, byte[] content, string contentType, string extension)
        {
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.ContentLength = content.Length;
            this.ContentType = contentType;
            this.Extension = extension;
        }


        /// <summary>
        /// Id
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// Data
        /// </summary>
        public virtual byte[] Content { get; set; }

        /// <summary>
        /// mime type
        /// </summary>
        public virtual string ContentType { get; set; }

        /// <summary>
        /// File Size
        /// </summary>
        public virtual int ContentLength { get; set; }

        /// <summary>
        /// Extension 
        /// </summary>
        public virtual string Extension { get; set; }

       
       
    }
}
