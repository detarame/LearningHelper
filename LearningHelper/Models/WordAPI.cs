using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningHelper.Models
{
    public class WordAPI
    {
        public Int16 Id { get; set; }
        public Int16 WordId { get; set; }
        public Int16 LanguageId { get; set; }
        public string Value { get; set; }
    }
}