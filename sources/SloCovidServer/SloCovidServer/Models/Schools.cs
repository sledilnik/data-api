using System.Collections.Immutable;

namespace SloCovidServer.Models
{
    public record SchoolCasesDay(int Year, int Month, int Day, ImmutableDictionary<string, ImmutableDictionary<string, ImmutableDictionary<string, int>>> SchoolType): IModelDate;
    public record SchoolAbsenceDay(int Year, int Month, int Day, Date AbsentFrom, Date AbsentTo, 
        string SchoolType, string School, string PersonType, string PersonClass, string Reason): IModelDate;
    public record SchoolRegimeDay(int Year, int Month, int Day, Date ChangedFrom, Date ChangedTo, 
        string SchoolType, string School, string PersonClass, int? Attendees, string Regime, string Reason): IModelDate;

    public record SchoolStatus(ImmutableArray<SchoolAbsenceDay> Absences, ImmutableArray<SchoolRegimeDay> Regimes);
}
