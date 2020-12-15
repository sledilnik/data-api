using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class ModelsPredictionintervaltype
    {
        public ModelsPredictionintervaltype()
        {
            ModelsPredictions = new HashSet<ModelsPrediction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ModelsPrediction> ModelsPredictions { get; set; }
    }
}
