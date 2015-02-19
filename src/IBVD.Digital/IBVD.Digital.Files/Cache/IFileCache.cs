using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBVD.Digital.IBVD.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFileCache
    {
        /// <summary>
        /// Adds the specified item to the System.Web.Caching.Cache object with dependencies,
        ///     expiration and priority policies, and a delegate you can use to notify your
        ///     application when the inserted item is removed from the Cache
        /// </summary>
        /// <param name="item">The item to be added to the cache</param>
        /// <returns>An Ticket identifiyng the item in cache, if the item was previously stored in the cache; otherwise, null.</returns>
        /// <exception cref="System.ArgumentNullException">The key or value parameter is set to null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The slidingExpiration parameter is set to less than TimeSpan.Zero or more than one year</exception>
        /// <exception cref="System.ArgumentException">The absoluteExpiration and slidingExpiration parameters are both set for the item you are trying to add to the Cache</exception>
        Ticket Add(CacheItem item);

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <param name="ticket">The identifier for the cache item to retrieve</param>
        /// <returns>The retrieved cache item, or null if the key is not found</returns>
        CacheItem Get(Ticket ticket);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        void Remove(Ticket ticket);
    }
}
