using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IBVD.Digital.Files;

namespace IBVD.Digital.IBVD.Cache
{
    public class CacheItem : IFile
    {
        private Ticket ticket;
        private byte[] byteStream;
        private string contentType;
        private string name;
        private int length;
        private string description;
        private int id;
        private string extension;
        public CacheItem(IFile file, Ticket ticket):
            this(ticket, file.Name, file.Description, file.Content, file.ContentType,  file.ContentLength, file.Extension)
        {
        }

        public CacheItem(Ticket ticket, string name, string description, byte[] content, string contentType, int length, string extension)
        {
            this.id = 0;
            byteStream = content;
            this.contentType = contentType;
            this.ticket = ticket;
            this.name = name;
            this.length = length;
            this.description = description;
            this.extension = extension;
        }



        public Ticket Ticket 
        { 
            get { 
                return ticket; 
            } 
        }

        public byte[] Content
        {
            get
            {
                return byteStream;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
        }


        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string Description
        {
            get { return this.description; }
        }


        public string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        public int ContentLength
        {
            get
            {
                return this.length;
            }
        }


        public string Extension
        {
            get { return this.extension; }
        }
        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!obj.GetType().Equals(this.GetType())) return false;
            CacheItem other = (obj as CacheItem);
            return other.Ticket.Equals(this.Ticket) &&
                other.Name.Equals(this.Name) &&
                other.ContentType.Equals(this.ContentType) &&
                other.ContentLength.Equals(this.ContentLength) &&
                other.Content.Equals(this.Content);
        }

        /// <summary>
        /// Hash
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
