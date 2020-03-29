using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Implemented
{
    public class Communicator : ICommunicator
    {
        const string root = "https://raw.githubusercontent.com/slo-covid-19/data/master/csv";
        readonly HttpClient client;
        readonly ILogger<Communicator> logger;
        readonly Mapper mapper;
        ETagCacheItem<ImmutableArray<StatsDaily>> statsCache;
        ETagCacheItem<ImmutableArray<RegionsDay>> regionCache;
        ETagCacheItem<ImmutableArray<PatientsDay>> patientsCache;
        ETagCacheItem<ImmutableArray<HospitalsDay>> hospitalsCache;
        ETagCacheItem<ImmutableArray<Hospital>> hospitalsListCache;
        readonly static object statsSync = new object();
        readonly static object regionSync = new object();
        readonly static object patientsSync = new object();
        readonly static object hospitalsSync = new object();
        readonly static object hospitalsListSync = new object();
        public Communicator(ILogger<Communicator> logger, Mapper mapper)
        {
            client = new HttpClient();
            this.logger = logger;
            this.mapper = mapper;
            statsCache = new ETagCacheItem<ImmutableArray<StatsDaily>>(null, ImmutableArray<StatsDaily>.Empty);
            regionCache = new ETagCacheItem<ImmutableArray<RegionsDay>>(null, ImmutableArray<RegionsDay>.Empty);
            patientsCache = new ETagCacheItem<ImmutableArray<PatientsDay>>(null, ImmutableArray<PatientsDay>.Empty);
            hospitalsCache = new ETagCacheItem<ImmutableArray<HospitalsDay>>(null, ImmutableArray<HospitalsDay>.Empty);
            hospitalsListCache = new ETagCacheItem<ImmutableArray<Hospital>>(null, ImmutableArray<Hospital>.Empty);
        }

        public async Task<(ImmutableArray<StatsDaily>? Data, string ETag)> GetStatsAsync(string callerEtag, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, statsSync, $"{root}/stats.csv",
                statsCache, mapFromString: mapper.GetStatsFromRaw, cacheItem => statsCache = cacheItem, ct);
            return result;
        }

        public async Task<(ImmutableArray<RegionsDay>? Data, string ETag)> GetRegionsAsync(string callerEtag, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, regionSync, $"{root}/regions.csv",
                regionCache, mapFromString: mapper.GetRegionsFromRaw, cacheItem => regionCache = cacheItem, ct);
            return result;
        }

        public async Task<(ImmutableArray<PatientsDay>? Data, string ETag)> GetPatientsAsync(string callerEtag, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, patientsSync, $"{root}/patients.csv",
                patientsCache, mapFromString: mapper.GetPatientsFromRaw, cacheItem => patientsCache = cacheItem, ct);
            return result;
        }

        public async Task<(ImmutableArray<HospitalsDay>? Data, string ETag)> GetHospitalsAsync(string callerEtag, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, hospitalsSync, $"{root}/hospitals.csv",
                hospitalsCache, mapFromString: mapper.GetHospitalsFromRaw, cacheItem => hospitalsCache = cacheItem, ct);
            return result;
        }

        public async Task<(ImmutableArray<Hospital>? Data, string ETag)> GetHospitalsListAsync(string callerEtag, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, hospitalsListSync, $"{root}/dict-hospitals.csv",
                hospitalsListCache, mapFromString: mapper.GetHospitalsListFromRaw, cacheItem => hospitalsListCache = cacheItem, ct);
            return result;
        }

        async Task<(TData? Data, string ETag)> GetAsync<TData>(string callerEtag, object sync, string url, ETagCacheItem<TData> cache,
            Func<string, TData> mapFromString, Action<ETagCacheItem<TData>> storeToCache, CancellationToken ct)
            where TData: struct
        {
            var policy = HttpPolicyExtensions
              .HandleTransientHttpError()
              .RetryAsync(3);

            string currentETag;
            TData currentData;
            lock (sync)
            {
                currentETag = cache.ETag;
                currentData = cache.Data;
            }

            var response = await policy.ExecuteAsync(() =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(currentETag))
                {
                    request.Headers.Add("If-None-Match", currentETag);
                }
                return client.SendAsync(request, ct);
            });

            string etagInfo = $"ETag {(string.IsNullOrEmpty(callerEtag) ? "none" : "present")}";
            if (response.IsSuccessStatusCode)
            {
                currentETag = response.Headers.GetValues("ETag").SingleOrDefault();
                string content = await response.Content.ReadAsStringAsync();
                currentData = mapFromString(content);
                lock (sync)
                {
                    storeToCache(new ETagCacheItem<TData>(currentETag, currentData));
                }
                if (string.Equals(currentETag, callerEtag, StringComparison.Ordinal))
                {
                    logger.LogInformation($"Cache refreshed, client cache hit, {etagInfo}");
                    return (null, currentETag);
                }
                logger.LogInformation($"Cache refreshed, client refreshed, {etagInfo}");
                return (currentData, currentETag);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotModified)
            {
                if (string.Equals(currentETag, callerEtag, StringComparison.Ordinal))
                {
                    logger.LogInformation($"Cache hit, client cache hit, {etagInfo}");
                    return (null, currentETag);
                }
                else
                {
                    logger.LogInformation($"Cache hit, client cache refreshed, {etagInfo}");
                    return (currentData, currentETag);
                }
            }
            throw new Exception($"Failed fetching data: {response.ReasonPhrase}");
        }
    }
}
