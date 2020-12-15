using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class ModelsPredictionintervalwidth
    {
        public ModelsPredictionintervalwidth()
        {
            ModelsPredictions = new HashSet<ModelsPrediction>();
        }

        public int Id { get; set; }
        public int Width { get; set; }

        public virtual ICollection<ModelsPrediction> ModelsPredictions { get; set; }
    }
}
