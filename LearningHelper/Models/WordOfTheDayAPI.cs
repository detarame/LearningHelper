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
    }
}