﻿using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SloCovidServer.Formatters
{
    /// <summary>
    /// Copied from https://github.com/damienbod/WebAPIContrib.Core
    /// because original needs to implement InvariantCulture formatting
    /// </summary>
    public class CsvFormatterOptions
    {
        public bool UseSingleLineHeaderInCsv { get; set; } = true;
        public string CsvDelimiter { get; set; } = ";";
        public Encoding Encoding { get; set; } = Encoding.Default;
        public bool IncludeExcelDelimiterHeader { get; set; } = false;
    }
    /// <summary>
    /// Original code taken from
    /// http://www.tugberkugurlu.com/archive/creating-custom-csvmediatypeformatter-in-asp-net-web-api-for-comma-separated-values-csv-format
    /// Adapted for ASP.NET Core and uses ; instead of , for delimiters
    /// </summary>
    public class CsvOutputFormatter : OutputFormatter
    {
        private readonly CsvFormatterOptions _options;

        private readonly bool useJsonAttributes = true;

        public string ContentType { get; private set; }

        public CsvOutputFormatter(CsvFormatterOptions csvFormatterOptions)
        {
            ContentType = "text/csv";
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/csv"));
            _options = csvFormatterOptions ?? throw new ArgumentNullException(nameof(csvFormatterOptions));
        }

        protected override bool CanWriteType(Type type)
        {

            if (type == null)
                throw new ArgumentNullException("type");

            return IsTypeOfIEnumerable(type);
        }

        private bool IsTypeOfIEnumerable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Returns the JsonProperty data annotation name
        /// </summary>
        /// <param name="pi">Property Info</param>
        /// <returns></returns>
        private string GetDisplayNameFromNewtonsoftJsonAnnotations(PropertyInfo pi)
        {
            if (pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.PropertyName is string value)
            {
                return value;
            }

            return pi.GetCustomAttribute<DisplayAttribute>(false)?.GetName() ?? pi.Name;
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            Type type = context.Object.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            var streamWriter = new StreamWriter(response.Body, _options.Encoding);

            if (_options.IncludeExcelDelimiterHeader)
            {
                await streamWriter.WriteLineAsync($"sep ={_options.CsvDelimiter}");
            }
            // TODO this is quick hack to convert countries (dictionary) to CSV
            if (context.Object is ImmutableDictionary<string, object>[] countries)
            {
                await OutputCountries(countries, streamWriter);
            }
            else if (context.Object is string text)
            {
                await streamWriter.WriteAsync(text);
            }
            else
            {
                await OutputGeneric(context, itemType, streamWriter);
            }

            await streamWriter.FlushAsync();
        }
        async Task OutputCountries(ImmutableDictionary<string, object>[] countries, StreamWriter streamWriter)
        { 
            if (countries?.Length == 0)
            {
                return;
            }
            var keys = countries[0].Keys.ToImmutableArray();
            if (_options.UseSingleLineHeaderInCsv)
            {
                await streamWriter.WriteLineAsync(string.Join(_options.CsvDelimiter, keys));
            }
            foreach (var country in countries)
            {
                await streamWriter.WriteLineAsync(
                    string.Join(
                        _options.CsvDelimiter,
                        keys.Select(k => Convert.ToString(country[k], CultureInfo.InvariantCulture))
                    )
                );
            }
        }

        private async Task OutputGeneric(OutputFormatterWriteContext context, Type itemType, StreamWriter streamWriter)
        {
            if (_options.UseSingleLineHeaderInCsv)
            {
                var values = useJsonAttributes
                    ? itemType.GetProperties().Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>(false).Any())    // Only get the properties that do not define JsonIgnore
                        .Select(pi => new
                        {
                            Order = pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                            Prop = pi
                        }).OrderBy(d => d.Order).Select(d => GetDisplayNameFromNewtonsoftJsonAnnotations(d.Prop))
                    : itemType.GetProperties().Select(pi => pi.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? pi.Name);

                await streamWriter.WriteLineAsync(string.Join(_options.CsvDelimiter, values));
            }


            foreach (var obj in (IEnumerable<object>)context.Object)
            {
                var vals = useJsonAttributes
                    ? obj.GetType().GetProperties()
                        .Where(pi => !pi.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                        .Select(pi => new
                        {
                            Order = pi.GetCustomAttribute<JsonPropertyAttribute>(false)?.Order ?? 0,
                            Value = pi.GetValue(obj, null)
                        }).OrderBy(d => d.Order).Select(d => new { d.Value })
                    : obj.GetType().GetProperties().Select(
                        pi => new
                        {
                            Value = pi.GetValue(obj, null)
                        });

                string valueLine = string.Empty;

                foreach (var val in vals)
                {
                    if (val.Value != null)
                    {

                        var _val = Convert.ToString(val.Value, CultureInfo.InvariantCulture);

                        //Substitute smart quotes in Windows-1252
                        if (_options.Encoding.EncodingName == "Western European (ISO)")
                            _val = _val.Replace('“', '"').Replace('”', '"');

                        //Escape quotes
                        _val = _val.Replace("\"", "\"\"");

                        //Check if the value contains a delimiter and place it in quotes if so
                        if (_val.Contains(_options.CsvDelimiter))
                            _val = string.Concat("\"", _val, "\"");

                        //Replace any \r or \n special characters from a new line with a space
                        if (_val.Contains("\r"))
                            _val = _val.Replace("\r", " ");
                        if (_val.Contains("\n"))
                            _val = _val.Replace("\n", " ");

                        valueLine = string.Concat(valueLine, _val, _options.CsvDelimiter);

                    }
                    else
                    {
                        valueLine = string.Concat(valueLine, string.Empty, _options.CsvDelimiter);
                    }
                }

                await streamWriter.WriteLineAsync(valueLine.Remove(valueLine.Length - _options.CsvDelimiter.Length));
            }
        }
    }
}
