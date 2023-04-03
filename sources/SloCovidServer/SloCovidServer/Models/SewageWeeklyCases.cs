namespace SloCovidServer.Models;

public record SewageWeeklyCases(int Year, int Month, int Day, string Station, int? Flow, SewageN3 N3,
    int? Cod, SewageCase Cases, float? Lat, float? Lon, string Region, int? Population, float? CoverageRatio) : IModelDate;

public record SewageN3(float? Raw, float? Norm);

public record SewageCase(float? Estimated, float? Active100k);
