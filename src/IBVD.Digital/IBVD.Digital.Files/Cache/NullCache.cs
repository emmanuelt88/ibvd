using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.IBVD.Cache
{
    public class NullCache: IFileCache
    {
        #region IFileCache Members

        public Ticket Add(CacheItem item)
        {
            return null;
        }

        public CacheItem Get(Ticket ticket)
        {
            return null;
        }

        public void Remove(Ticket ticket)
        {
            
        }

        #endregion
    }
}
