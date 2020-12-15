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
            var casesToDate = GetCasesToDate(toDate, stats);
            var casesActive = GetCasesActive(toDate, stats);
            var hospitalizedCurrent = GetHospitalizedCurrent(toDate, patients);
            var icuCurrent = GetIcuCurrent(toDate, patients);
            var deceasedToDay = GetDeceasedToDay(toDate, patients);
            var casesAvg7Days = GetCasesAvg7Days(toDate, stats);
            return new Summary(casesToDate, casesActive, casesAvg7Days, hospitalizedCurrent, icuCurrent, deceasedToDay);
        }
        internal static HospitalizedCurrent GetHospitalizedCurrent(DateTime? toDate, ImmutableArray<PatientsDay> patients)
        {
            var lastPatient = GetLastAndPreviousItem(toDate, patients, p => p.Total?.InHospital?.Today != null);
            return lastPatient.HasValue ?
                new HospitalizedCurrent(
                    lastPatient.Value.Last.Total?.InHospital?.Today,
                    lastPatient.Value.Last.Total?.InHospital?.In,
                    lastPatient.Value.Last.Total?.InHospital?.Out,
                    lastPatient.Value.Last.Total?.Deceased?.Hospital?.Today,
                    CalculateDifference(lastPatient.Value.Last.Total?.InHospital?.Today, lastPatient.Value.Previous?.Total?.InHospital?.Today),
                    lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                : null;
        }
        internal static DeceasedToDate GetDeceasedToDay(DateTime? toDate, ImmutableArray<PatientsDay> patients)
        {
            var lastPatient = GetLastAndPreviousItem(toDate, patients, p => p.Total?.Deceased?.ToDate != null);
            return lastPatient.HasValue ?
                            new DeceasedToDate(
                                lastPatient.Value.Last.Total?.Deceased?.ToDate,
                                lastPatient.Value.Last.Total?.Deceased?.Today,
                                CalculateDifference(lastPatient.Value.Last.Total?.Deceased?.ToDate, lastPatient.Value.Previous?.Total?.Deceased?.ToDate),
                                lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                            : null;
        }

        internal static ICUCurrent GetIcuCurrent(DateTime? toDate, ImmutableArray<PatientsDay> patients)
        {
            var lastPatient = GetLastAndPreviousItem(toDate, patients, p => p.Total?.ICU?.Today != null);
            return lastPatient.HasValue ?
                            new ICUCurrent(
                                lastPatient.Value.Last.Total?.ICU?.Today,
                                lastPatient.Value.Last.Total?.ICU?.In,
                                lastPatient.Value.Last.Total?.ICU?.Out,
                                lastPatient.Value.Last.Total?.Deceased?.Hospital?.Icu?.Today,
                                CalculateDifference(lastPatient.Value.Last.Total?.ICU?.Today, lastPatient.Value.Previous?.Total?.ICU?.Today),
                                lastPatient.Value.Last.Year, lastPatient.Value.Last.Month, lastPatient.Value.Last.Day)
                            : null;
        }

        internal static CasesActive GetCasesActive(DateTime? toDate, ImmutableArray<StatsDaily> stats)
        {
            var lastStats = GetLastAndPreviousItem(toDate, stats, s => s.Cases?.Active != null);
            if (lastStats.HasValue)
            {
                int currentActive = lastStats.Value.Last.Cases.Active.Value;
                int? previousActive = lastStats.Value.Previous?.Cases?.Active;
                int? today = lastStats.Value.Last.Cases?.ConfirmedToday;
                int? deceased = lastStats.Value.Last.StatePerTreatment?.Deceased;
                bool canCalculateOut = previousActive is not null && today is not null && deceased is not null;
                return new CasesActive(
                                    currentActive,
                                    today,
                                    canCalculateOut ? today - (currentActive - previousActive.Value) - deceased.Value : null,
                                    deceased,
                                    CalculateDifference(currentActive, previousActive),
                                    lastStats.Value.Last.Year, lastStats.Value.Last.Month, lastStats.Value.Last.Day);
            }
            else
            {
                return null;
            }
        }

        internal static CasesToDateSummary GetCasesToDate(DateTime? toDate, ImmutableArray<StatsDaily> stats)
        {
            var lastStats = GetLastAndPreviousItem(toDate, stats, s => s.Cases?.ConfirmedToDate != null);
            return lastStats.HasValue ?
                            new CasesToDateSummary(
                                lastStats.Value.Last.Cases?.ConfirmedToDate,
                                lastStats.Value.Last.Cases?.ConfirmedToday,
                                CalculateDifference(lastStats.Value.Last.Cases?.ConfirmedToDate, lastStats.Value.Previous?.Cases?.ConfirmedToDate),
                                lastStats.Value.Last.Year, lastStats.Value.Last.Month, lastStats.Value.Last.Day)
                            : null;
        }
        internal static CasesAvg7Days GetCasesAvg7Days(DateTime? toDate, ImmutableArray<StatsDaily> stats)
        {
            var lastStats = GetLastAndPreviousItem(toDate, stats, s => s.Cases?.ConfirmedToday != null);
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
                return lastValue.Value / (float)previousValue.Value;
            }
            return null;
        }
        internal static (T Last, T Previous, int IndexOfLast)? GetLastAndPreviousItem<T>(DateTime? toDate, ImmutableArray<T> items, Func<T, bool> isValid)
            where T : IModelDate
        {
            if (toDate.HasValue)
            {
                var lastStatPair = GetLastItemOnDate(toDate.Value, items, isValid);
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
                for (int i=items.Length-1; i >= 0; i--)
                {
                    var item = items[i];
                    if (isValid(item))
                    {
                        // TODO should it return previous or first valid previous item?
                        return (
                            item,
                            i > 0 ? items[i-1] : default,
                            i
                        );
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Return the item on date or if none exists, first item before that date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="date"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        internal static (T Item, int Index)? GetLastItemOnDate<T>(DateTime date, ImmutableArray<T> items, Func<T, bool> isValid)
            where T: IModelDate
        {
            for (int i = items.Length - 1; i >= 0; i--)
            {
                var item = items[i];
                if (isValid(item) && new DateTime(item.Year, item.Month, item.Day) <= date)
                {
                    return (item, i);
                }
            }
            return null;
        }
    }
}
