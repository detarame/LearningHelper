using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PersonBL
    {
        private IDbContext Database;
        public PersonBL(IDbContext db)
        {
            this.Database = db;
        }
        public List<Person> GetPeople()
        {
            return Database.Persons.ToList();
        }
        public List<Person> GetPeople(Func<Person, bool> selector)
        {
            return Database.Persons.Where(selector).ToList();
        }
        public Person GetPerson(Int16 id)
        {
            return Database.Persons.Where(p => p.Id == id).FirstOrDefault();
        }
        public void AddPerson(Person person)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            // set ID here or set it in API and check here?
            Database.Persons.Add(person);
            Database.SaveChanges();
        }
        public void AddPerson(Person person, string mainLanguageName)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            person.MainLanguage = Database.Languages
                .Where(w => w.Name.TrimEnd().Equals(mainLanguageName)).FirstOrDefault();
            person.MainLanguageId = person.MainLanguage.Id;
            Database.Persons.Add(person);
            Database.SaveChanges();
        }
        public List<Person> Sort<TKey>(Func<Person, TKey> selector)
        {
           return Database.Persons.OrderBy(selector).ToList();
        }
        public bool Exists(Person person)
        {
            // change the way of comparing
            return Database.Persons.Any(p => p.Id == person.Id);
        }
        public bool DeletePerson(Int16 id)
        {
            var temp = Database.Persons.Where(w => w.Id == id).FirstOrDefault();
            if (temp != null)
            {
                Database.Configuration.ValidateOnSaveEnabled = false;
                Database.Persons.Remove(temp);
                Database.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
