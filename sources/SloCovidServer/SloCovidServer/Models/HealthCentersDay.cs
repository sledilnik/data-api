using System.Collections.Immutable;
using Righthand.Immutable;

namespace SloCovidServer.Models
{
    public record HealthCentersDay : IModelDate
    {
        public int Year { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public HealthCentersDayItem All { get; init; }
        public ImmutableDictionary<string, ImmutableDictionary<string, HealthCentersDayItem>> Municipalities { get; init; }
        public HealthCentersDay(
            int year, int month, int day,
            HealthCentersDayItem all, ImmutableDictionary<string, ImmutableDictionary<string, HealthCentersDayItem>> municipalities)
        {
            Year = year;
            Month = month;
            Day = day;
            All = all;
            Municipalities = municipalities;
        }
    }
    public record HealthCentersDayItem
    {
        public static HealthCentersDayItem Empty { get; } = new HealthCentersDayItem(
            HealthCentersExaminations.Empty,
            HealthCentersPhoneTriage.Empty,
            HealthCentersTests.Empty,
            HealthCentersSentTo.Empty);
        public HealthCentersExaminations Examinations { get; init; }
        public HealthCentersPhoneTriage PhoneTriage { get; init; }
        public HealthCentersTests Tests { get; init; }
        public HealthCentersSentTo SentTo { get; init; }

        public HealthCentersDayItem(HealthCentersExaminations examinations, HealthCentersPhoneTriage phoneTriage, HealthCentersTests tests, HealthCentersSentTo sentTo)
        {
            Examinations = examinations;
            PhoneTriage = phoneTriage;
            Tests = tests;
            SentTo = sentTo;
        }
    }

    public record HealthCentersExaminations
    {
        public static HealthCentersExaminations Empty { get; } = new HealthCentersExaminations(null, null);
        public int? MedicalEmergency { get; init; }
        public int? SuspectedCovid { get; init; }

        public HealthCentersExaminations(int? medicalEmergency, int? suspectedCovid)
        {
            MedicalEmergency = medicalEmergency;
            SuspectedCovid = suspectedCovid;
        }
    }

    public record HealthCentersPhoneTriage
    {
        public static HealthCentersPhoneTriage Empty { get; } = new HealthCentersPhoneTriage((int?)null);
        public int? SuspectedCovid { get; init; }

        public HealthCentersPhoneTriage(int? suspectedCovid)
        {
            SuspectedCovid = suspectedCovid;
        }
    }

    public record HealthCentersTests
    {
        public static HealthCentersTests Empty { get; } = new HealthCentersTests(null, null);
        public int? Performed { get; init; }
        public int? Positive { get; init; }

        public HealthCentersTests(int? performed, int? positive)
        {
            Performed = performed;
            Positive = positive;
        }
    }

    public record HealthCentersSentTo
    {
        public static HealthCentersSentTo Empty { get; } = new HealthCentersSentTo(null, null);
        public int? Hospital { get; init; }
        public int? SelfIsolation { get; init; }

        public HealthCentersSentTo(int? hospital, int? selfIsolation)
        {
            Hospital = hospital;
            SelfIsolation = selfIsolation;
        }
    }
}
