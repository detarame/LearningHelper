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
        public Person ApiToDb()
        {
            var temp = new Person();
            temp.Id = this.Id;
            temp.Name = this.Name;
            temp.RegistrationDate = this.RegistrationDate;
            temp.MainLanguageId = this.MainLanguageId;
            return temp;
        }
        public static PersonAPI DbToApi(Person p)
        {
            if (p == null) return null;
            var temp = new PersonAPI();
            temp.Id = p.Id;
            temp.MainLanguageId = p.MainLanguageId;
            temp.Name = p.Name;
            temp.RegistrationDate = p.RegistrationDate;
            return temp;
        }
    }
}