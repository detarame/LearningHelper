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
        public Vocabulary ApiToDb()
        {
            var temp = new Vocabulary();
            temp.Id = this.Id;
            temp.Name = this.Name;
            temp.CreationDate = this.CreationDate;
            temp.Theme = this.Theme;
            temp.LanguageId = this.LanguageId;
            return temp;
        }
        public static VocabularyAPI DbToApi(Vocabulary p)
        {
            if (p == null) return null;
            var temp = new VocabularyAPI();
            temp.Id = p.Id;
            temp.CreationDate = p.CreationDate;
            temp.Theme = p.Theme;
            temp.Name = p.Name;
            temp.LanguageId = p.LanguageId;
            return temp;
        }
    }
}