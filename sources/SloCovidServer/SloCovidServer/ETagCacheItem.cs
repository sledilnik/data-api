using System;
using System.Collections.Immutable;

namespace SloCovidServer
{
    public class ETagCacheItem<T>
    {
        public DateTime Created { get; } = DateTime.UtcNow;
        public string ETag { get; }
        public T Data { get; }
        public long? Timestamp { get; }
        public static ETagCacheItem<T> Create(string etag, T data, long? timestamp)
        {
            return new ETagCacheItem<T>(etag, data, timestamp);
        }
        public ETagCacheItem(string eTag, T data, long? timestamp)
        {
            ETag = eTag;
            Data = data;
            Timestamp = timestamp;
        }

        public ETagCacheItem<T> ShallowClone()
        {
            return (ETagCacheItem<T>)MemberwiseClone();
        }
    }

    public class ArrayETagCacheItem<T>: ETagCacheItem<ImmutableArray<T>>
    {
        public ArrayETagCacheItem(string eTag, ImmutableArray<T> data, long? timestamp) : base(eTag, data, timestamp)
        {
        }

        public ArrayETagCacheItem() : base(null, ImmutableArray<T>.Empty, timestamp: null)
        {
        }
    }
}
