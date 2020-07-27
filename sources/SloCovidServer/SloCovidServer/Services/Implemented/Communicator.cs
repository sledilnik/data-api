using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Prometheus;
using SloCovidServer.Mappers;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Implemented
{
    public class Communicator : ICommunicator
    {
        const string root = "https://raw.githubusercontent.com/sledilnik/data/master/csv";
        readonly HttpClient client;
        readonly ILogger<Communicator> logger;
        readonly Mapper mapper;
        readonly ISlackService slackService;
        protected static readonly Histogram RequestDuration = Metrics.CreateHistogram("source_request_duration_milliseconds",
            "Request duration to CSV sources in milliseconds",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(start: 20, factor: 2, count: 10),
                LabelNames = new[] { "endpoint", "is_exception" }
            });
        protected static readonly Counter RequestCount = Metrics.CreateCounter("source_request_total", "Total number of requests to source",
            new CounterConfiguration
            {
                LabelNames = new[] { "endpoint" }
            });
        protected static readonly Counter RequestMissedCache = Metrics.CreateCounter("source_request_missed_cache_total", 
            "Total number of missed cache when fetching from source",
            new CounterConfiguration
            {
                LabelNames = new[] { "endpoint" }
            });
        protected static readonly Counter RequestExceptions = Metrics.CreateCounter("source_request_exceptions_total", 
            "Total number of exceptions when fetching data from source",
            new CounterConfiguration
            {
                LabelNames = new[] { "endpoint" }
            });
        protected static readonly Gauge EndpointDown = Metrics.CreateGauge("endpoint_down",
           "When above 0 means that given endpoint is unreachable",
           new GaugeConfiguration
           {
               LabelNames = new[] { "endpoint" }
           });
        readonly ArrayEndpointCache<StatsDaily> statsCache;
        readonly ArrayEndpointCache<RegionsDay> regionCache;
        readonly ArrayEndpointCache<PatientsDay> patientsCache;
        readonly ArrayEndpointCache<HospitalsDay> hospitalsCache;
        readonly ArrayEndpointCache<Hospital> hospitalsListCache;
        readonly ArrayEndpointCache<Municipality> municipalitiesListCache;
        readonly ArrayEndpointCache<RetirementHome> retirementHomesListCache;
        readonly ArrayEndpointCache<RetirementHomesDay> retirementHomesCache;
        readonly ArrayEndpointCache<DeceasedPerRegionsDay> deceasedPerRegionsDayCache;
        readonly ArrayEndpointCache<MunicipalityDay> municipalityDayCache;
        readonly ArrayEndpointCache<HealthCentersDay> healthCentersDayCache;
        /// <summary>
        /// Holds error flags against endpoints
        /// </summary>
        readonly ConcurrentDictionary<string, object> errors;
        public Communicator(ILogger<Communicator> logger, Mapper mapper, ISlackService slackService)
        {
            client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            this.logger = logger;
            this.mapper = mapper;
            this.slackService = slackService;
            statsCache = new ArrayEndpointCache<StatsDaily>();
            regionCache = new ArrayEndpointCache<RegionsDay>();
            patientsCache = new ArrayEndpointCache<PatientsDay>();
            hospitalsCache = new ArrayEndpointCache<HospitalsDay>();
            hospitalsListCache = new ArrayEndpointCache<Hospital>();
            municipalitiesListCache = new ArrayEndpointCache<Municipality>();
            retirementHomesListCache = new ArrayEndpointCache<RetirementHome>();
            retirementHomesCache = new ArrayEndpointCache<RetirementHomesDay>();
            deceasedPerRegionsDayCache = new ArrayEndpointCache<DeceasedPerRegionsDay>();
            municipalityDayCache = new ArrayEndpointCache<MunicipalityDay>();
            healthCentersDayCache = new ArrayEndpointCache<HealthCentersDay>();
            errors = new ConcurrentDictionary<string, object>();
        }

        public async Task<(ImmutableArray<StatsDaily>? Data, string ETag, long? Timestamp)> GetStatsAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/stats.csv", statsCache, mapFromString: mapper.GetStatsFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<RegionsDay>? Data, string ETag, long? Timestamp)> GetRegionsAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/regions.csv",regionCache, mapFromString: mapper.GetRegionsFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<PatientsDay>? Data, string ETag, long? Timestamp)> GetPatientsAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/patients.csv", patientsCache, mapFromString: mapper.GetPatientsFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<HospitalsDay>? Data, string ETag, long? Timestamp)> GetHospitalsAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/hospitals.csv", hospitalsCache, mapFromString: mapper.GetHospitalsFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<Hospital>? Data, string ETag, long? Timestamp)> GetHospitalsListAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/dict-hospitals.csv",hospitalsListCache, mapFromString: mapper.GetHospitalsListFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<Municipality>? Data, string ETag, long? Timestamp)> GetMunicipalitiesListAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/dict-municipality.csv",municipalitiesListCache, mapFromString: mapper.GetMunicipalitiesListFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<RetirementHome>? Data, string ETag, long? Timestamp)> GetRetirementHomesListAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/dict-retirement_homes.csv", retirementHomesListCache, 
                mapFromString: mapper.GetRetirementHomesListFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<RetirementHomesDay>? Data, string ETag, long? Timestamp)> GetRetirementHomesAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/retirement_homes.csv", retirementHomesCache,
                mapFromString: mapper.GetRetirementHomesFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<DeceasedPerRegionsDay>? Data, string ETag, long? Timestamp)> GetDeceasedPerRegionsAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/deceased-regions.csv", deceasedPerRegionsDayCache,
                mapFromString: new DeceasedPerRegionsMapper().GetDeceasedPerRegionsDayFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<MunicipalityDay>? Data, string ETag, long? Timestamp)> GetMunicipalitiesAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/municipality.csv", municipalityDayCache,
                mapFromString: new MunicipalitiesMapper().GetMunicipalityDayFromRaw, filter, ct);
            return result;
        }

        public async Task<(ImmutableArray<HealthCentersDay>? Data, string ETag, long? Timestamp)> GetHealthCentersAsync(string callerEtag, DataFilter filter, CancellationToken ct)
        {
            var result = await GetAsync(callerEtag, $"{root}/health_centers.csv", healthCentersDayCache,
                mapFromString: new HealthCentersMapper().GetHealthCentersDayFromRaw, filter, ct);
            return result;
        }

        public class RegionsPivotCacheData
        {
            public ETagCacheItem<ImmutableArray<Municipality>> Municipalities { get; }
            public ETagCacheItem<ImmutableArray<RegionsDay>> Regions { get; }
            public ImmutableArray<ImmutableArray<object>> Data { get;}
            public RegionsPivotCacheData(ETagCacheItem<ImmutableArray<Municipality>> municipalities, ETagCacheItem<ImmutableArray<RegionsDay>> regions,
                ImmutableArray<ImmutableArray<object>> data)
            {
                Municipalities = municipalities;
                Regions = regions;
                Data = data;
            }
        }
        RegionsPivotCacheData regionsPivotCacheData = new RegionsPivotCacheData(
            new ETagCacheItem<ImmutableArray<Municipality>>(null, ImmutableArray<Municipality>.Empty, timestamp: null),
            new ETagCacheItem<ImmutableArray<RegionsDay>>(null, ImmutableArray<RegionsDay>.Empty, timestamp: null),
            data: ImmutableArray<ImmutableArray<object>>.Empty
        );
        readonly object syncRegionsPivot = new object();
        //public async Task<(ImmutableArray<ImmutableArray<object>>? Data, string ETag)>  GetRegionsPivotAsync(string callerEtag, CancellationToken ct)
        //{
        //    string[] callerETags = !string.IsNullOrEmpty(callerEtag) ? callerEtag.Split(',') : new string[2];
        //    if (callerETags.Length != 2)
        //    {
        //        callerETags = new string[2];
        //    }
        //    RegionsPivotCacheData localCache;
        //    lock(syncRegionsPivot)
        //    {
        //        localCache = regionsPivotCacheData;
        //    }
        //    var muncipalityTask = GetMunicipalitiesListAsync(localCache.Municipalities.ETag, ct);
        //    var regions = await GetRegionsAsync(localCache.Regions.ETag, ct);
        //    var municipalities = await muncipalityTask;
        //    if (regions.Data.HasValue || municipalities.Data.HasValue)
        //    {
        //        var data = mapper.MapRegionsPivot(municipalities.Data ?? localCache.Municipalities.Data, regions.Data ?? localCache.Regions.Data);
        //        localCache = new RegionsPivotCacheData(
        //            municipalities.Data.HasValue ? 
        //                new ETagCacheItem<ImmutableArray<Municipality>>(municipalities.ETag, municipalities.Data ?? ImmutableArray<Municipality>.Empty)
        //                : localCache.Municipalities,
        //            regions.Data.HasValue ? 
        //                new ETagCacheItem<ImmutableArray<RegionsDay>>(regions.ETag, regions.Data ?? ImmutableArray<RegionsDay>.Empty)
        //                : localCache.Regions,
        //            data
        //        );
        //        lock(syncRegionsPivot)
        //        {
        //            regionsPivotCacheData = localCache;
        //        }
        //        return (data, $"{municipalities.ETag},{regions.ETag}");
        //    }
        //    else
        //    {
        //        string resultTag = $"{municipalities.ETag},{regions.ETag}";
        //        if (string.Equals(callerETags[0], localCache.Municipalities.ETag, StringComparison.Ordinal)
        //            && string.Equals(callerETags[1], localCache.Regions.ETag, StringComparison.Ordinal))
        //        {
        //            return (null, resultTag);
        //        }
        //        else
        //        {
        //            return (localCache.Data, resultTag);
        //        }
        //    }
        //}
        //public async Task<(ImmutableArray<RegionSum>? Data, string ETag)> GetRegionsSummaryAsync(string callerEtag, CancellationToken ct)
        //{
        //    var regions = await GetRegionsAsync(callerEtag, ct);
        //    if (regions.Data != null)
        //    {
        //        var sum = new Dictionary<string, Dictionary<string, int?>>();
        //        foreach (var region in regions.Data.Value)
        //        {
        //            foreach (var pair in region.Regions)
        //            {
        //                if (!sum.TryGetValue(pair.Key, out var municipalities))
        //                {
        //                    municipalities = new Dictionary<string, int?>();
        //                    sum.Add(pair.Key, municipalities);
        //                }
        //                foreach (var source in pair.Value)
        //                {
        //                    municipalities.TryGetValue(source.Key, out int? municipality);
        //                    if (source.Value.HasValue && municipality.HasValue)
        //                    {
        //                        municipality += source.Value;
        //                        municipalities[source.Key] = municipality;
        //                    }
        //                }
        //            }
        //        }
        //        var result = new List<RegionSum>(sum.Count);
        //        foreach (var r in sum)
        //        {
        //            result.Add(
        //                new RegionSum(
        //                    r.Key,
        //                    name: "",
        //                    altName: "",
        //                    municipalities: r.Value.Select(m => new MunicipalitySum(m.Key, name: "", altName: "", m.Value)).ToImmutableArray()
        //                    )
        //                );
        //            return (result.ToImmutableArray(), regions.ETag);
        //        }
        //    }
        //    return (null, regions.ETag);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="url"></param>
        /// <param name="sync"></param>
        /// <param name="current"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <remarks>This method might update sync.Cache but only to refresh its Created property.</remarks>
        async Task<(HttpResponseMessage Response, ETagCacheItem<TData> Current, string Timestamp)> FetchDataAsync<TData>(
            string url, EndpointCache<TData> sync, ETagCacheItem<TData> current, CancellationToken ct)
            where TData : struct
        {
            async Task ProcessErrorAsync(string message)
            {
                if (errors.TryAdd(url, null))
                {
                    EndpointDown.WithLabels(url).Inc();
                    await slackService.SendNotificationAsync($"DATA API REST service started failing to retrieve data from {url} because {message}",
                        CancellationToken.None);
                }
                else
                {
                    await slackService.SendNotificationAsync($"DATA API REST service failed retrieving data from {url} because {message}",
                        CancellationToken.None);
                }
            }
            async Task ProcessErrorRemovalAsync()
            {
                // remove error flag
                if (errors.TryRemove(url, out _))
                {
                    EndpointDown.WithLabels(url).Dec();
                    await slackService.SendNotificationAsync($"DATA API REST service started retrieving data from {url}", CancellationToken.None);
                }
            }

            var policy = HttpPolicyExtensions
              .HandleTransientHttpError()
              .RetryAsync(1);

            HttpResponseMessage response;
            // cache responses for a minute
            if (current.ETag == null || (DateTime.UtcNow - current.Created) > TimeSpan.FromMinutes(1))
            {
                try
                {
                    response = await policy.ExecuteAsync(() =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, url);
                        if (!string.IsNullOrEmpty(current.ETag))
                        {
                            request.Headers.Add("If-None-Match", current.ETag);
                        }
                        RequestCount.WithLabels(url).Inc();
                        return client.SendAsync(request, ct);
                    });
                    if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotModified)
                    {
                        _ = ProcessErrorRemovalAsync();
                    }
                    else
                    {
                        _ = ProcessErrorAsync(response.ReasonPhrase);
                        // refresh created to avoid hitting source too much in this case
                        sync.Cache = new ETagCacheItem<TData>(current.ETag, current.Data, current.Timestamp);
                        // setting response to null, so the calling method will return cached data
                        response = null;
                    }
                }
                catch (Exception ex)
                {
                    // setting response to null, so the calling method will return cached data
                    response = null;
                    _ = ProcessErrorAsync(ex.Message);
                    // refresh created to avoid hitting source too much in this case
                    sync.Cache = new ETagCacheItem<TData>(current.ETag, current.Data, current.Timestamp);
                }
            }
            else
            {
                response = null;
            }
            string timestamp;
            if (response != null)
            {
                timestamp = await GetTimestampAsync(url);
            }
            else
            {
                timestamp = null;
            }
            return (response, current, timestamp);
        }

        async Task<string> GetTimestampAsync(string url)
        {
            string timestampUrl = $"{url}.timestamp";
            try
            {
                return await client.GetStringAsync(timestampUrl);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error {error} retrieving timestamp from {url}", ex.Message, timestampUrl);
                return null;
            }
        }

        // filters data based on date
        public ImmutableArray<TData> FilterData<TData>(ImmutableArray<TData> data, DataFilter filter)
        {
            if (typeof(IModelDate).IsAssignableFrom(typeof(TData)))
            {
                return data.Where(m =>
                {
                    var md = (IModelDate)m;
                    var date = new DateTime(md.Year, md.Month, md.Day);
                    if (filter.From.HasValue && date < filter.From)
                    {
                        return false;
                    }
                    if (filter.To.HasValue && date > filter.To.Value)
                    {
                        return false;
                    }
                    return true;
                }).ToImmutableArray();
            }
            else
            {
                return data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="callerEtag"></param>
        /// <param name="url"></param>
        /// <param name="sync"></param>
        /// <param name="mapFromString"></param>
        /// <param name="filter"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <remarks>
        /// In cache is always all data though filtered is returned.
        /// </remarks>
        async Task<(ImmutableArray<TData>? Data, string ETag, long? Timestamp)> GetAsync<TData>(string callerEtag, string url, 
            EndpointCache<ImmutableArray<TData>> sync, Func<string, ImmutableArray<TData>> mapFromString, DataFilter filter, CancellationToken ct)
            where TData: class
        {
            var stopwatch = Stopwatch.StartNew();

            ETagCacheItem<ImmutableArray<TData>> current = sync.Cache;

            bool isException = false;
            try
            {
                HttpResponseMessage response;
                string timestampText;
                // current might have been updated with new Created date
                (response, current, timestampText) = await FetchDataAsync(url, sync, current, ct);
                long? timestamp;
                if (long.TryParse(timestampText, out long ts))
                {
                    timestamp = ts;
                }
                else
                {
                    timestamp = null;
                }

                string etagInfo = $"ETag {(string.IsNullOrEmpty(callerEtag) ? "none" : "present")}";
                if (response?.IsSuccessStatusCode ?? false)
                {
                    RequestMissedCache.WithLabels(url).Inc();
                    string newETag = response.Headers.GetValues("ETag").SingleOrDefault();
                    string content = await response.Content.ReadAsStringAsync();
                    var newData = mapFromString(content);
                    current = new ETagCacheItem<ImmutableArray<TData>>(newETag, newData, timestamp);
                    sync.Cache = current;
                    if (string.Equals(current.ETag, callerEtag, StringComparison.Ordinal))
                    {
                        logger.LogInformation($"Cache refreshed, client cache hit, {etagInfo}");
                        return (null, current.ETag, current.Timestamp);
                    }
                    logger.LogInformation($"Cache refreshed, client refreshed, {etagInfo}");
                    var filteredData = FilterData(current.Data, filter);
                    return (filteredData, current.ETag, current.Timestamp);
                }
                else if (response == null || response.StatusCode == System.Net.HttpStatusCode.NotModified)
                {
                    // recreate cache if there was an actual request to update its Created field
                    if (response != null)
                    {
                        sync.Cache = new ETagCacheItem<ImmutableArray<TData>>(current.ETag, current.Data, current.Timestamp);
                    }
                    if (string.Equals(current.ETag, callerEtag, StringComparison.Ordinal))
                    {
                        logger.LogInformation($"Cache hit, client cache hit, {etagInfo}");
                        return (null, current.ETag, current.Timestamp);
                    }
                    else
                    {
                        logger.LogInformation($"Cache hit, client cache refreshed, {etagInfo}");
                        var filteredData = FilterData(current.Data, filter);
                        return (filteredData, current.ETag, current.Timestamp);
                    }
                }
                throw new Exception($"Failed fetching data: {response.ReasonPhrase}");
            }
            catch
            {
                isException = true;
                RequestExceptions.WithLabels(url).Inc();
                throw;
            }
            finally
            {
                RequestDuration.WithLabels(url, isException.ToString()).Observe(stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
