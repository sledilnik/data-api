using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class TastypieApikey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }

        public virtual AuthUser User { get; set; }
    }
}
