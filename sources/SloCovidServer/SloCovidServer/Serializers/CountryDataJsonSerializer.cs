using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SloCovidServer.Models.Owid;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace SloCovidServer.Serializers
{
    public class CountryDataJsonSerializer : JsonConverter<CountryData>
    {
        public override bool CanWrite => false;
        public override CountryData ReadJson(JsonReader reader, Type objectType, [AllowNull] CountryData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var source = JToken.ReadFrom(reader) as JObject;
            var date = DateTime.Parse((string)source["date"], CultureInfo.InvariantCulture);
            var data = ((IDictionary<string, JToken>)source)
                .Where(p => !string.Equals(p.Key, "date", StringComparison.Ordinal) && !string.Equals(p.Key, "tests_units", StringComparison.Ordinal))
                .ToImmutableDictionary(p => p.Key, p => p.Value.ToObject<float?>());
            return new CountryData(date, (string)source["tests_units"], data);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] CountryData value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
