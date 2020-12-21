using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class ModelsPredictiondatum
    {
        public int Id { get; set; }
        public int Icu { get; set; }
        public int? IcuLowerBound { get; set; }
        public int? IcuUpperBound { get; set; }
        public int Hospitalized { get; set; }
        public int? HospitalizedLowerBound { get; set; }
        public int? HospitalizedUpperBound { get; set; }
        public int Deceased { get; set; }
        public int? DeceasedLowerBound { get; set; }
        public int? DeceasedUpperBound { get; set; }
        public int DeceasedToDate { get; set; }
        public int? DeceasedToDateLowerBound { get; set; }
        public int? DeceasedToDateUpperBound { get; set; }
        public int PredictionId { get; set; }
        public DateTime Date { get; set; }

        public virtual ModelsPrediction Prediction { get; set; }
    }
}
