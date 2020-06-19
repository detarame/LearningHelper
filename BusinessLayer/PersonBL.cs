using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace BusinessLayer
{
    public class PersonBL
    {
        private IDbContext Database;
        public PersonBL(IDbContext db)
        {
            this.Database = db;
            //Database.Configuration.ValidateOnSaveEnabled = false;
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
        public List<Person> GetOrderedPeople<TKey>(Func<Person, TKey> selector)
        {
           return Database.Persons.OrderBy(selector).ToList();
        }
        public List<Vocabulary> GetPersonVocabularies(Int16 PersonId)
        {
            return Database.PersonVocabulary.Where(w => w.PersonId == PersonId).Select(s => s.Vocabulary).ToList();
        }
        public List<Word> GetPersonWords(Int16 personId)
        {
            var temp = Database.PersonVocabulary.Where(w => w.PersonId == personId)
                .Select(s => s.VocabularyId).ToList();
            List<Word> result = new List<Word>();
            foreach (var vocabularyId in temp)
            {
                result.AddRange(Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId)
                    .Select(s => s.Word));
            }
            return result;
        }
        public Person AddPerson(Person person)
        {
            Database.Persons.Add(person);
            Database.SaveChanges();
            return person;
        }
        public bool AddVocabularyToPerson(Int16 vocabId, Int16 personId)
        {
            if (Database.PersonVocabulary.Where(w => w.PersonId == personId).Any(a => a.VocabularyId == vocabId))
            {
                return false;
            }
            var temp = new PersonVocabulary();
            temp.PersonId = personId;
            temp.VocabularyId = vocabId;
            Database.PersonVocabulary.Add(temp);
            Database.SaveChanges();
            return true;
        }
        public bool DeletePerson(Int16 id)
        {
            var temp = Database.Persons.Where(w => w.Id == id).FirstOrDefault();
            if (temp != null)
            {
                Database.Persons.Remove(temp);
                Database.PersonVocabulary.RemoveRange(Database.PersonVocabulary.Where(w => w.PersonId == id));
                Database.WordsOfTheDay.RemoveRange(Database.WordsOfTheDay.Where(w => w.PersonId == id));
                Database.SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeletePersonVocab(Int16 VocabId, Int16 PersonId)
        {
            // Deleting person-vocabulary relation
            var temp = Database.PersonVocabulary.Where(w => w.PersonId == PersonId)
                .Where(w => w.VocabularyId == VocabId).FirstOrDefault();
            if (temp == null) return false;
            Database.PersonVocabulary.Remove(temp);
            Database.SaveChanges();
            return true;
        }
        public Person Update(Person p)
        {
            var temp = Database.Persons.Where(w => w.Id == p.Id).FirstOrDefault();
            if (temp == null) throw new Exception("Person doesn't exist");
            temp.MainLanguageId = p.MainLanguageId;
            temp.Name = p.Name;
            temp.RegistrationDate = p.RegistrationDate;
            Database.SaveChanges();
            return temp;
        }
        public WordOfTheDay GetWordOfTheDay(Int16 PersonId)
        {
            var temp = Database.GetWordOfTheDay(PersonId);
            // always returns -1 ???

            //if (temp == 0)
            //{
            //    return Database.WordsOfTheDay.Where(w => w.PersonId == PersonId).Last();
            //}
            //else
            //{
            //    return null;
            //};
            return Database.WordsOfTheDay.Where(w => w.PersonId == PersonId).AsEnumerable().LastOrDefault();
        }
    }
}
