using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class ModelsModel
    {
        public ModelsModel()
        {
            ModelsPredictions = new HashSet<ModelsPrediction>();
        }

        public Guid Id { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string Www { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ModelsPrediction> ModelsPredictions { get; set; }
    }
}
