using SloCovidServer.Models;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace SloCovidServer.Mappers
{
    public static class SummaryMapper
    {
        public static Summary CreateSummary(DateTime? toDate, ImmutableArray<StatsDaily> stats, ImmutableArray<PatientsDay> patients)
        {
            var lastStats = GetLastAndPreviousItem(toDate, stats);
            var lastPatient = GetLastAndPreviousItem(toDate, patients);

            var casesToDate = GetCasesToDate(lastStats);
            var casesActive = GetCasesActive(lastStats);
            var hospitalizedCurrent = lastPatient.HasValue ?
                new HospitalizedCurrent(
                    lastPatient.Value.Last.Total?.InHospital?.Today,
                    lastPatient.Value.Last.Total?.InHospital?.In,
                    lastPatient.Value.Last.Total?.InHospital?.Out,
                    lastPatient.Value.Last.Total?.Deceased?.Hospital?.Today,
                    CalculateDifference(lastPatient.Value.Last.Total?.InHospital?.Today, lastPatient.Value.Previous?.Total?.InHospital?.Today),
                    lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                : null;
            var icuCurrent = GetIcuCurrent(lastStats, lastPatient);
            var deceasedToDay = GetDeceasedToDay(lastPatient);
            var casesAvg7Days = GetCasesAvg7Days(lastStats, stats);
            return new Summary(casesToDate, casesActive, casesAvg7Days, hospitalizedCurrent, icuCurrent, deceasedToDay);
        }

        internal static DeceasedToDate GetDeceasedToDay((PatientsDay Last, PatientsDay Previous, int IndexOfLast)? lastPatient)
        {
            return lastPatient.HasValue ?
                            new DeceasedToDate(
                                lastPatient.Value.Last.Total?.Deceased?.ToDate,
                                CalculateDifference(lastPatient.Value.Last.Total?.Deceased?.ToDate, lastPatient.Value.Previous?.Total?.Deceased?.ToDate),
                                lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                            : null;
        }

        internal static ICUCurrent GetIcuCurrent((StatsDaily Last, StatsDaily Previous, int IndexOfLast)? lastStats, (PatientsDay Last, PatientsDay Previous, int IndexOfLast)? lastPatient)
        {
            return lastStats.HasValue ?
                            new ICUCurrent(
                                lastPatient.Value.Last.Total?.ICU?.Today,
                                lastPatient.Value.Last.Total?.ICU?.In,
                                lastPatient.Value.Last.Total?.ICU?.Out,
                                lastPatient.Value.Last.Total?.Deceased?.Hospital?.Icu?.Today,
                                CalculateDifference(lastPatient.Value.Last.Total?.ICU?.Today, lastPatient.Value.Previous?.Total?.ICU?.Today),
                                lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                            : null;
        }

        internal static CasesActive GetCasesActive((StatsDaily Last, StatsDaily Previous, int IndexOfLast)? lastStats)
        {
            return lastStats.HasValue ?
                            new CasesActive(
                                lastStats.Value.Last.Cases?.ActiveToDate,
                                lastStats.Value.Last.Cases?.ConfirmedToday,
                                0,
                                lastStats.Value.Last.StatePerTreatment?.Deceased,
                                CalculateDifference(lastStats.Value.Last.Cases?.ActiveToDate, lastStats.Value.Previous?.Cases?.ActiveToDate),
                                lastStats.Value.Last.Year, lastStats.Value.Last.Month, lastStats.Value.Last.Day)
                            : null;
        }

        internal static CasesToDateSummary GetCasesToDate((StatsDaily Last, StatsDaily Previous, int IndexOfLast)? lastStats)
        {
            return lastStats.HasValue ?
                            new CasesToDateSummary(
                                lastStats.Value.Last.Cases?.ConfirmedToDate,
                                lastStats.Value.Last.Cases?.ConfirmedToday,
                                CalculateDifference(lastStats.Value.Last.Cases?.ConfirmedToDate, lastStats.Value.Previous?.Cases?.ConfirmedToDate),
                                lastStats.Value.Last.Year, lastStats.Value.Last.Month, lastStats.Value.Last.Day)
                            : null;
        }
        internal static CasesAvg7Days GetCasesAvg7Days((StatsDaily Last, StatsDaily Previous, int IndexOfLast)? lastStats, ImmutableArray<StatsDaily> stats)
        {
            if (lastStats.HasValue)
            {
                int firstItemIndex = Math.Max(0, lastStats.Value.IndexOfLast - 6);
                float lastDayAvg = (float)stats.Skip(firstItemIndex).Take(lastStats.Value.IndexOfLast-firstItemIndex + 1)
                    .Where(s => s.Cases?.ConfirmedToday != null)
                    .Average(s => (int)s.Cases?.ConfirmedToday);
                float? previousDayAvg = firstItemIndex > 0 ? (float?)stats.Skip(firstItemIndex-1).Take(7)
                    .Where(s => s.Cases?.ConfirmedToday != null)
                    .Average(s => (int)s.Cases?.ConfirmedToday)
                    : null;
                return new CasesAvg7Days(
                    lastDayAvg,
                    Sublabel: true,
                    previousDayAvg.HasValue && previousDayAvg.Value != 0 ? lastDayAvg / previousDayAvg.Value: null,
                    lastStats.Value.Last.Year, lastStats.Value.Last.Month, lastStats.Value.Last.Day);
            }
            else
            {
                return null;
            }
        }
        internal static float? CalculateDifference(int? lastValue, int? previousValue)
        {
            if (lastValue.HasValue && previousValue.HasValue && previousValue != 0)
            {
                return lastValue.Value / previousValue.Value;
            }
            return null;
        }
        internal static (T Last, T Previous, int IndexOfLast)? GetLastAndPreviousItem<T>(DateTime? toDate, ImmutableArray<T> items)
            where T : IModelDate
        {
            if (toDate.HasValue)
            {
                var lastStatPair = GetLastItemOnDate(toDate.Value, items);
                if (lastStatPair.HasValue)
                {
                    return (
                        lastStatPair.Value.Item,
                        lastStatPair.Value.Index > 0 ? items[lastStatPair.Value.Index - 1] : default,
                        lastStatPair.Value.Index);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (items.Length > 0)
                {
                    return (
                        items.Last(),
                        items.Length > 1 ? items[^2] : default,
                        items.Length - 1
                    );
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Return the item on date or if none exists, first item before that date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="date"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        internal static (T Item, int Index)? GetLastItemOnDate<T>(DateTime date, ImmutableArray<T> items)
            where T: IModelDate
        {
            for (int i = items.Length - 1; i >= 0; i--)
            {
                var item = items[i];
                if (new DateTime(item.Year, item.Month, item.Day) <= date)
                {
                    return (item, i);
                }
            }
            return null;
        }
    }
}
