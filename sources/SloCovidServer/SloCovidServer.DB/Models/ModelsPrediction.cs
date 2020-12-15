using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class ModelsPrediction
    {
        public ModelsPrediction()
        {
            ModelsPredictiondata = new HashSet<ModelsPredictiondatum>();
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Date { get; set; }
        public int? IntervalTypeId { get; set; }
        public int? IntervalWidthId { get; set; }
        public Guid ModelId { get; set; }
        public int ScenarioId { get; set; }

        public virtual ModelsPredictionintervaltype IntervalType { get; set; }
        public virtual ModelsPredictionintervalwidth IntervalWidth { get; set; }
        public virtual ModelsModel Model { get; set; }
        public virtual ModelsScenario Scenario { get; set; }
        public virtual ICollection<ModelsPredictiondatum> ModelsPredictiondata { get; set; }
    }
}
