using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record SchoolCasesDay(int Year, int Month, int Day, ImmutableDictionary<string, ImmutableDictionary<string, ImmutableDictionary<string, int>>> SchoolType) { }
    public record SchoolAbsenceDay(int Year, int Month, int Day, Date AbsentFrom, Date AbsentTo, 
        string SchoolType, int School, string PersonType, string PersonClass, string Reason);
    public record SchoolRegimeDay(int Year, int Month, int Day, Date ChangedFrom, Date ChangedTo, 
        string SchoolType, int School, string PersonClass, int? Attendees, string Regime, string Reason);

    public record SchoolStatus(ImmutableArray<SchoolAbsenceDay> Absences, ImmutableArray<SchoolRegimeDay> Regimes);
}
