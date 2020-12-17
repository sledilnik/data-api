using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class RestrictionsRestriction
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleSl { get; set; }
        public string TitleEn { get; set; }
        public string Rule { get; set; }
        public string RuleSl { get; set; }
        public string RuleEn { get; set; }
        public string Regions { get; set; }
        public string RegionsSl { get; set; }
        public string RegionsEn { get; set; }
        public string Exceptions { get; set; }
        public string ExceptionsSl { get; set; }
        public string ExceptionsEn { get; set; }
        public string ExtraRules { get; set; }
        public string ExtraRulesSl { get; set; }
        public string ExtraRulesEn { get; set; }
        public DateTime? ValidSince { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string ValidityComment { get; set; }
        public string ValidityCommentSl { get; set; }
        public string ValidityCommentEn { get; set; }
        public string Comments { get; set; }
        public string CommentsSl { get; set; }
        public string CommentsEn { get; set; }
        public string LegalLink { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Published { get; set; }
        public int Order { get; set; }
    }
}
