using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningHelper.Models
{
    public class VocabularyAPI
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Theme { get; set; }
        public Int16 LanguageId { get; set; }
    }
}