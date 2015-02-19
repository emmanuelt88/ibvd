using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.Files
{
    public interface IFile
    {
        int Id { get; }

        byte[] Content { get; }

        string Name { get; }

        string Description { get; }

        string ContentType { get; }

        int ContentLength { get; }

        string Extension { get; }
    }
}
