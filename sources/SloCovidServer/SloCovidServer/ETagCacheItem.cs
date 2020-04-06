using System.Collections.Immutable;

namespace SloCovidServer
{
    public class ETagCacheItem<T>
    {
        public string ETag { get; }
        public T Data { get; }
        public static ETagCacheItem<T> Create(string etag, T data)
        {
            return new ETagCacheItem<T>(etag, data);
        }
        public ETagCacheItem(string eTag, T data)
        {
            ETag = eTag;
            Data = data;
        }

        public ETagCacheItem<T> ShallowClone()
        {
            return (ETagCacheItem<T>)MemberwiseClone();
        }
    }

    public class ArrayETagCacheItem<T>: ETagCacheItem<ImmutableArray<T>>
    {
        public ArrayETagCacheItem(string eTag, ImmutableArray<T> data) : base(eTag, data)
        {
        }

        public ArrayETagCacheItem() : base(null, ImmutableArray<T>.Empty)
        {
        }
    }
}
