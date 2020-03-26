﻿using System.Collections.Generic;

namespace SloCovidServer.Models
{
    public class RegionsDay
    {
        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Dictionary<string, Dictionary<string, int?>> Regions { get; }
        public RegionsDay(int year, int month, int day, Dictionary<string, Dictionary<string, int?>> regions)
        {
            Year = year;
            Month = month;
            Day = day;
            Regions = regions;
        }
    }
}
