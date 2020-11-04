using System;
using System.Collections.Immutable;

namespace SloCovidServer
{
    public class ETagCacheItem<T>
    {
        public DateTime Created { get; } = DateTime.UtcNow;
        public string ETag { get; }
        public T Data { get; }

        public string Raw { get; }
        public long? Timestamp { get; }
        public ETagCacheItem(string eTag, string raw, T data, long? timestamp)
        {
            ETag = eTag;
            Raw = raw;
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
        public ArrayETagCacheItem() : base(null, "", ImmutableArray<T>.Empty, timestamp: null)
        {
        }
    }
    public class DictionaryETagCacheItem<TKey, TValue> : ETagCacheItem<ImmutableDictionary<TKey, TValue>>
    {
        public DictionaryETagCacheItem() : base(null, "", ImmutableDictionary<TKey, TValue>.Empty, timestamp: null)
        {
        }
    }

}
