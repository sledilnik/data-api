using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class PostsPost
    {
        public int Id { get; set; }
        public bool Published { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Author { get; set; }
        public string AuthorSl { get; set; }
        public string AuthorEn { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string TitleSl { get; set; }
        public string TitleEn { get; set; }
        public string Blurb { get; set; }
        public string BlurbSl { get; set; }
        public string BlurbEn { get; set; }
        public string LinkTo { get; set; }
        public string LinkToSl { get; set; }
        public string LinkToEn { get; set; }
        public string Body { get; set; }
        public string BodySl { get; set; }
        public string BodyEn { get; set; }
        public bool Pinned { get; set; }
    }
}
