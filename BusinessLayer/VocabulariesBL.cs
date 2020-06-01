using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class VocabulariesBL
    {
        private IDbContext Database;
        public VocabulariesBL(IDbContext db)
        {
            this.Database = db;
        }
        public List<Vocabulary> GetVocabularies()
        {
            return Database.Vocabularies.ToList();
        }
        public List<Vocabulary> GetVocabularies(Func<Vocabulary, bool> selector)
        {
            return Database.Vocabularies.Where(selector).ToList();
        }
        public Vocabulary GetVocabulary(Int16 id)
        {
            return Database.Vocabularies.Where(p => p.Id == id).FirstOrDefault();
        }
        public void AddVocabulary(Vocabulary vocabulary)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            if (Database.Vocabularies.Any(v => v.Id == vocabulary.Id))
            {
                throw new Exception("Id already exists");
            }
            Database.Vocabularies.Add(vocabulary);
            Database.SaveChanges();
        }
        public void AddVocabulary(Vocabulary vocabulary, Int16 personId)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            if (Database.Vocabularies.Any(v => v.Id == vocabulary.Id))
            {
                throw new Exception("Id already exists");
            }
            Database.Vocabularies.Add(vocabulary);
            var person = Database.PersonVocabulary.Where(w => w.PersonId == personId).First();
            if (person == null)
            {
                throw new Exception("Pesron doesn't exist");
            }
            person.Vocabulary = vocabulary;
            Database.SaveChanges();
        }
        public List<Person> Sort<TKey>(Func<Person, TKey> selector)
        {
            return Database.Persons.OrderBy(selector).ToList();
        }
        public bool Exists(Person person)
        {
            return Database.Persons.Any(p => p.Id == person.Id);
        }
        public List<Person> Find(Func<Person, bool> selector)
        {
            return Database.Persons.Where(selector).ToList();
        }
    }
}
