using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IBVD.Digital.Files
{
    public class FileSystemFile : IFile
    {
        private FileInfo fi;
        private string contenType;
        private string description;
        private int id;

        public FileSystemFile(string fullPath, int id, string contenType, string description)
        {
            this.id = id;
            fi = new FileInfo(fullPath);
            this.contenType = contenType;
            this.description = description;
        }

        #region IFile Members

        public byte[] Content
        {
            get
            {
                byte[] byteStream = File.ReadAllBytes(fi.FullName);
                return byteStream;
            }
        }

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return fi.Name; }
        }

        public string Description
        {
            get { return fi.Name; }
        }

        public string ContentType
        {
            get { return contenType; }
        }

        public int ContentLength
        {
            get { return (int)fi.Length; }
        }

        #endregion

        public static void Save(IFile file, string fullFileName)
        {
            File.WriteAllBytes(fullFileName, file.Content);
        }

        public string Extension
        {
            get { return fi.Extension; }
        }
    }
}
