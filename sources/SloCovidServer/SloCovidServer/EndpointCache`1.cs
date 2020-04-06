using System.Collections.Immutable;
using System.Threading;

namespace SloCovidServer.Services.Implemented
{
    /// <summary>
    /// Provides synchronized access to <see cref="Cache"/> property. The type of the property has to be readonly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EndpointCache<T>
    {
        readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        ETagCacheItem<T> cache;
        public ETagCacheItem<T> Cache
        {
            get
            {
                sync.EnterReadLock();
                try
                {
                    return cache;
                }
                finally
                {
                    sync.ExitReadLock();
                }
            }
            set
            {
                sync.EnterWriteLock();
                try
                {
                    cache = value;
                }
                finally
                {
                    sync.ExitWriteLock();
                }
            }
        }
        public EndpointCache(ETagCacheItem<T> cache)
        {
            this.cache = cache;
        }
    }

    public class ArrayEndpointCache<T> : EndpointCache<ImmutableArray<T>>
    {
        public ArrayEndpointCache(ArrayETagCacheItem<T> cache) : base(cache)
        {
        }

        public ArrayEndpointCache() : this(new ArrayETagCacheItem<T>())
        { }
    }
}
