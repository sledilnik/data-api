using System;

namespace SloCovidServer.Models
{
    public record DataFilter
    {
        public readonly static DataFilter Empty = new DataFilter(null, null);
        /// <summary>
        /// Inclusive starting date.
        /// </summary>
        public DateTime? From { get; init; }
        /// <summary>
        /// Inclusive ending date.
        /// </summary>
        public DateTime? To { get; init; }
        public DataFilter(DateTime? from, DateTime? to)
        {
            From = from;
            To = to;
        }
    }
}
