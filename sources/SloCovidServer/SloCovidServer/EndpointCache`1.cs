using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Implemented
{
    /// <summary>
    /// Provides synchronized access to <see cref="Cache"/> property. The type of the property has to be readonly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EndpointCache<T>
    {
        readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
        private TaskCompletionSource<bool> initPromise = new TaskCompletionSource<bool>();
        private bool initialized = false;
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
                    if (!this.initialized)
                    {
                        // resolve promise on first instance
                        this.initialized = true;
                        this.initPromise.SetResult(true);
                    }
                    cache = value;
                }
                finally
                {
                    sync.ExitWriteLock();
                }
            }
        }
        public ETagCacheItem<T> CacheBlocking
        {
            get
            {
                if (!this.initialized)
                {
                    // wait for promise resolution on first request
                    // owid countries requires a lot of time
                    if (!this.initPromise.Task.Wait(120000))
                    {
                        throw new System.Exception("Timeout waiting cache");
                    }
                }
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

    public class DictionaryEndpointCache<TKey, TValue> : EndpointCache<ImmutableDictionary<TKey, TValue>>
    {
        public DictionaryEndpointCache(DictionaryETagCacheItem<TKey, TValue> cache) : base(cache)
        {
        }

        public DictionaryEndpointCache() : this(new DictionaryETagCacheItem<TKey, TValue>())
        { }
    }
}
