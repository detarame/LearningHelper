using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningHelper.Models
{
    public class WordOfTheDayAPI
    {
        public Int16 Id { get; set; }
        public Int16 WordId { get; set; }
        public Int16 PersonId { get; set; }
        public DateTime AddingDate { get; set; }
        public WordOfTheDayAPI ApiToDb()
        {
            var temp = new WordOfTheDayAPI();
            temp.Id = this.Id;
            temp.WordId = this.WordId;
            temp.PersonId = this.PersonId;
            temp.AddingDate = this.AddingDate;
            return temp;
        }
        public static WordOfTheDayAPI DbToApi(WordOfTheDay p)
        {
            if (p == null) return null;
            var temp = new WordOfTheDayAPI();
            temp.Id = p.Id;
            temp.WordId = p.WordId;
            temp.PersonId = p.PersonId;
            temp.AddingDate = p.AddingDate;
            return temp;
        }
    }
}