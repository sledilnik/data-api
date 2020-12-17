using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class TastypieApiaccess
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Url { get; set; }
        public string RequestMethod { get; set; }
        public int Accessed { get; set; }
    }
}
