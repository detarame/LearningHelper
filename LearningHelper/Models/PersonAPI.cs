using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningHelper.Models
{
    public class PersonAPI
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Int16 MainLanguageId { get; set; }
    }
}