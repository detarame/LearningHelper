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
        public Word ApiToDb()
        {
            var temp = new Word();
            temp.Id = this.Id;
            temp.WordId = this.WordId;
            temp.LanguageId = this.LanguageId;
            temp.Value = this.Value;
            return temp;
        }
        public static WordAPI DbToApi(Word p)
        {
            if (p == null) return null;
            var temp = new WordAPI();
            temp.Id = p.Id;
            temp.WordId = p.WordId;
            temp.LanguageId = p.LanguageId;
            temp.Value = p.Value;
            return temp;
        }
    }
}