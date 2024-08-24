namespace SloCovidServer.Models;

public record OpsiCase(YearWeek YearWeek, int Year, int Month, int Day, int TestPcr, int TestHagt, int ConfirmedCases);
public record YearWeek(int Year, int Week);