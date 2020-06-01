using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ContextLogic
    {
        public readonly IDbContext database;
        //public List<Word> GetWords()
        //{
        //    return database.Words.ToList();
        //}
        public ContextLogic(IDbContext db)
        {
            database = db;
        }
        public List<Person> GetPeople()
        {
            return database.Persons.ToList();
        }
        public List<Vocabulary> GetPersonsVocabularies(Int16 PersonId)
        {
            return database.PersonVocabulary.Where(i => i.PersonId == PersonId)
                .Select(s => s.Vocabulary).ToList();
        }
        public List<Word> GetVocabularyWords(int VocabularyId)
        {
            return database.VocabularyWords.Where(i => i.VocabularyId == VocabularyId)
                .Select(s => s.Word).ToList();
        }
        public List<Word> GetWords()
        {
            return database.VocabularyWords.Select(s => s.Word).ToList();
        }
        public List<Language> GetLanguages()
        {
            return database.Languages.ToList();
        }
        public void AddPerson(Person person)
        {
            database.Configuration.ValidateOnSaveEnabled = false;
            database.Persons.Add(person);
            database.SaveChanges();
        }
    }
}
