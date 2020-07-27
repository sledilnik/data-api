using System.Collections.Immutable;
using Righthand.Immutable;

namespace SloCovidServer.Models
{
    public class HealthCentersDay : IModelDate
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public HealthCentersDayItem All { get; }
        public ImmutableDictionary<string, ImmutableDictionary<string, HealthCentersDayItem>> Municipalities { get; }
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
    public class HealthCentersDayItem
    {
        public static HealthCentersDayItem Empty { get; } = new HealthCentersDayItem(
            HealthCentersExaminations.Empty,
            HealthCentersPhoneTriage.Empty,
            HealthCentersTests.Empty,
            HealthCentersSentTo.Empty);
        public HealthCentersExaminations Examinations { get; }
        public HealthCentersPhoneTriage PhoneTriage { get; }
        public HealthCentersTests Tests { get; }
        public HealthCentersSentTo SentTo { get; }

        public HealthCentersDayItem(HealthCentersExaminations examinations, HealthCentersPhoneTriage phoneTriage, HealthCentersTests tests, HealthCentersSentTo sentTo)
        {
            Examinations = examinations;
            PhoneTriage = phoneTriage;
            Tests = tests;
            SentTo = sentTo;
        }

        public HealthCentersDayItem Clone(Param<HealthCentersExaminations>? examinations = null, Param<HealthCentersPhoneTriage>? phoneTriage = null, Param<HealthCentersTests>? tests = null, Param<HealthCentersSentTo>? sentTo = null)
        {
            return new HealthCentersDayItem(examinations.HasValue ? examinations.Value.Value : Examinations,
				phoneTriage.HasValue ? phoneTriage.Value.Value : PhoneTriage,
				tests.HasValue ? tests.Value.Value : Tests,
				sentTo.HasValue ? sentTo.Value.Value : SentTo);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (HealthCentersDayItem)obj;
            return Equals(Examinations, o.Examinations) && Equals(PhoneTriage, o.PhoneTriage) && Equals(Tests, o.Tests) && Equals(SentTo, o.SentTo);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (Examinations != null ? Examinations.GetHashCode() : 0);
				hash = hash * 37 + (PhoneTriage != null ? PhoneTriage.GetHashCode() : 0);
				hash = hash * 37 + (Tests != null ? Tests.GetHashCode() : 0);
				hash = hash * 37 + (SentTo != null ? SentTo.GetHashCode() : 0);
				return hash;
			}
        }
    }

    public class HealthCentersExaminations
    {
        public static HealthCentersExaminations Empty { get; } = new HealthCentersExaminations(null, null);
        public int? MedicalEmergency { get; }
        public int? SuspectedCovid { get; }

        public HealthCentersExaminations(int? medicalEmergency, int? suspectedCovid)
        {
            MedicalEmergency = medicalEmergency;
            SuspectedCovid = suspectedCovid;
        }

        public HealthCentersExaminations Clone(Param<int?>? medicalEmergency = null, Param<int?>? suspectedCovid = null)
        {
            return new HealthCentersExaminations(medicalEmergency.HasValue ? medicalEmergency.Value.Value : MedicalEmergency,
				suspectedCovid.HasValue ? suspectedCovid.Value.Value : SuspectedCovid);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (HealthCentersExaminations)obj;
            return Equals(MedicalEmergency, o.MedicalEmergency) && Equals(SuspectedCovid, o.SuspectedCovid);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (MedicalEmergency != null ? MedicalEmergency.GetHashCode() : 0);
				hash = hash * 37 + (SuspectedCovid != null ? SuspectedCovid.GetHashCode() : 0);
				return hash;
			}
        }
    }

    public class HealthCentersPhoneTriage
    {
        public static HealthCentersPhoneTriage Empty { get; } = new HealthCentersPhoneTriage(null);
        public int? SuspectedCovid { get; }

        public HealthCentersPhoneTriage(int? suspectedCovid)
        {
            SuspectedCovid = suspectedCovid;
        }

        public HealthCentersPhoneTriage Clone(Param<int?>? suspectedCovid = null)
        {
            return new HealthCentersPhoneTriage(suspectedCovid.HasValue ? suspectedCovid.Value.Value : SuspectedCovid);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (HealthCentersPhoneTriage)obj;
            return Equals(SuspectedCovid, o.SuspectedCovid);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (SuspectedCovid != null ? SuspectedCovid.GetHashCode() : 0);
				return hash;
			}
        }
    }

    public class HealthCentersTests
    {
        public static HealthCentersTests Empty { get; } = new HealthCentersTests(null, null);
        public int? Performed { get; }
        public int? Positive { get; }

        public HealthCentersTests(int? performed, int? positive)
        {
            Performed = performed;
            Positive = positive;
        }

        public HealthCentersTests Clone(Param<int?>? performed = null, Param<int?>? positive = null)
        {
            return new HealthCentersTests(performed.HasValue ? performed.Value.Value : Performed,
				positive.HasValue ? positive.Value.Value : Positive);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (HealthCentersTests)obj;
            return Equals(Performed, o.Performed) && Equals(Positive, o.Positive);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (Performed != null ? Performed.GetHashCode() : 0);
				hash = hash * 37 + (Positive != null ? Positive.GetHashCode() : 0);
				return hash;
			}
        }
    }

    public class HealthCentersSentTo
    {
        public static HealthCentersSentTo Empty { get; } = new HealthCentersSentTo(null, null);
        public int? Hospital { get; }
        public int? SelfIsolation { get; }

        public HealthCentersSentTo(int? hospital, int? selfIsolation)
        {
            Hospital = hospital;
            SelfIsolation = selfIsolation;
        }

        public HealthCentersSentTo Clone(Param<int?>? hospital = null, Param<int?>? selfIsolation = null)
        {
            return new HealthCentersSentTo(hospital.HasValue ? hospital.Value.Value : Hospital,
				selfIsolation.HasValue ? selfIsolation.Value.Value : SelfIsolation);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            var o = (HealthCentersSentTo)obj;
            return Equals(Hospital, o.Hospital) && Equals(SelfIsolation, o.SelfIsolation);
}

        public override int GetHashCode()
        {
            unchecked
			{
				int hash = 23;
				hash = hash * 37 + (Hospital != null ? Hospital.GetHashCode() : 0);
				hash = hash * 37 + (SelfIsolation != null ? SelfIsolation.GetHashCode() : 0);
				return hash;
			}
        }
    }
}
