using System;
using System.Collections.Immutable;

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
        public virtual bool IsEmpty => !From.HasValue && !To.HasValue;
        public DataFilter(DateTime? from, DateTime? to)
        {
            From = from;
            To = to;
        }
    }

    public record SchoolsStatusesFilter: DataFilter
    {
        public ImmutableArray<string> Schools { get; init; }
        public override bool IsEmpty => Schools.IsDefaultOrEmpty && base.IsEmpty;
        public SchoolsStatusesFilter(ImmutableArray<string> schools, DateTime? from, DateTime? to): base(from, to)
        {
            Schools = schools;
        }
    }
}
