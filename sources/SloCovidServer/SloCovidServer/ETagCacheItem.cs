using System;

namespace SloCovidServer
{
    public class ETagCacheItem<T>
    {
        public string ETag { get; }
        public T Data { get; }
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
}
