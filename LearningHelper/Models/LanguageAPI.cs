using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningHelper.Models
{
    public class LanguageAPI
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public Language ApiToDb()
        {
            var temp = new Language();
            temp.Id = this.Id;
            temp.Name = this.Name;
            return temp;
        }
        public static LanguageAPI DbToApi(Language p)
        {
            if (p == null) return null;
            var temp = new LanguageAPI();
            temp.Id = p.Id;
            temp.Name = p.Name;
            return temp;
        }
    }
}