using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SloCovidServer.Models.Owid;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SloCovidServer.Serializers
{
    public class CountryJsonSerializer : JsonConverter<Country>
    {
        public override bool CanWrite => false;
        public override Country ReadJson(JsonReader reader, Type objectType, [AllowNull] Country existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var source = JToken.ReadFrom(reader) as JObject;
            var data = (IDictionary<string, JToken>)source;
            var countryData = data["data"].ToObject<ImmutableArray<CountryData>>(serializer);
            var allColumns = new Dictionary<string, object>();
            foreach (var pair in data)
            {
                switch (pair.Key)
                {
                    case "data":
                        break;
                    default:
                        object value = pair.Value.Type switch
                        {
                            JTokenType.String => pair.Value.ToObject<string>(),
                            _ => pair.Value.ToObject<float?>()
                        };
                        allColumns.Add(pair.Key, value);
                        break;
                }
            }
            return new Country(allColumns.ToImmutableDictionary(), countryData);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] Country value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
