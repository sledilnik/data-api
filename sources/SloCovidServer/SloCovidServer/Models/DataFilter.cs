using System;

namespace SloCovidServer.Models
{
    public readonly struct DataFilter
    {
        public readonly static DataFilter Empty = new DataFilter(null, null);
        /// <summary>
        /// Inclusive starting date.
        /// </summary>
        public DateTime? From { get; }
        /// <summary>
        /// Inclusive ending date.
        /// </summary>
        public  DateTime? To { get; }
        public DataFilter(DateTime? from, DateTime? to)
        {
            From = from;
            To = to;
        }
    }
}
