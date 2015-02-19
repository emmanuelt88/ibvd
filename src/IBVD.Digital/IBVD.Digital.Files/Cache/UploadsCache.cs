using System;
using System.Web;
using System.Web.Caching;


namespace IBVD.Digital.IBVD.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadsCache : IFileCache
    {
        private DateTime absoluteExpiration = System.Web.Caching.Cache.NoAbsoluteExpiration;
        private TimeSpan slidingExpiration = new TimeSpan(0, 30, 0);

        public UploadsCache()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Ticket Add(CacheItem item)
        {
            GetCache().Insert(item.Ticket.Value, item,
                null, absoluteExpiration,
                slidingExpiration, CacheItemPriority.NotRemovable, null
                );
            return item.Ticket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public CacheItem Get(Ticket ticket)
        {
            return (CacheItem)GetCache().Get(ticket.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        public void Remove(Ticket ticket)
        {
            GetCache().Remove(ticket.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static System.Web.Caching.Cache GetCache()
        {
            return HttpRuntime.Cache;
        }
    }
}
