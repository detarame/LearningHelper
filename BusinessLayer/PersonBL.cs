using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data.Entity;
using System.Linq.Expressions;

namespace BusinessLayer
{
    public class PersonBL
    {
        private IDbContext Database;
        public PersonBL(IDbContext db)
        {
            this.Database = db;
        }
        public async Task<List<Person>> GetPeopleAsync()
        {
            return await Database.Persons.ToListAsync();
        }
        public async Task<List<Person>> GetPeopleAsync(Expression<Func<Person, bool>> selector)
        {
            return await Database.Persons.Where<Person>(selector).ToListAsync();
        }
        public async Task<Person> GetPersonAsync(Int16 id) 
        {
            return await Database.Persons.Where(p => p.Id == id).FirstOrDefaultAsync(); 
        }
        public async Task<List<Person>> GetOrderedPeopleAsync<TKey>(Expression<Func<Person, TKey>> selector)
        {
           return await Database.Persons.OrderBy(selector).ToListAsync();
        }
        public async Task<List<Vocabulary>> GetPersonVocabulariesAsync(Int16 PersonId)
        {
            var exists = await Database.PersonVocabulary.AnyAsync(w => w.PersonId == PersonId);
            if (!exists)
            {
                return null;
            }
            return await Database.PersonVocabulary.Where(w => w.PersonId == PersonId).Select(s => s.Vocabulary)
                .ToListAsync();
        }
        public async Task<List<Word>> GetPersonWordsAsync(Int16 personId)
        {
            var exists = await Database.PersonVocabulary.AnyAsync(w => w.PersonId == personId);
            if (!exists)
            {
                return null;
            }
            var listVocabIds = await Database.PersonVocabulary.Where(w => w.PersonId == personId)
                .Select(s => s.VocabularyId).ToListAsync();
            var wordLists = new List<List<Word>>(listVocabIds.Count);

            var count = 0;
            foreach (var vocabularyId in listVocabIds) 
            {
                wordLists.Add(await Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId)
                    .Select(s => s.Word).ToListAsync());
                count += wordLists.LastOrDefault().Count;
            }

            List<Word> result = new List<Word>(count);
            foreach (var wordList in wordLists)
            {
                result.AddRange(wordList);
            }
            return result;
        }
        public Person AddPerson(Person person)
        {
            Database.Persons.Add(person); 
            Database.SaveChanges(); 
            return person;
        }
        public async Task<bool> AddVocabularyToPersonAsync(Int16 vocabId, Int16 personId)
        {
            var exists = await Database.PersonVocabulary.Where(w => w.PersonId == personId).AnyAsync(a => a.VocabularyId == vocabId);
            if (!exists)
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
        public async Task<bool> DeletePersonAsync(Int16 id) 
        {
            var temp = await Database.Persons.Where(w => w.Id == id).FirstOrDefaultAsync();
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
        public async Task<bool> DeletePersonVocabAsync(Int16 VocabId, Int16 PersonId)
        {
            // Deleting person-vocabulary relation
            var temp = await Database.PersonVocabulary.Where(w => w.PersonId == PersonId)
                .Where(w => w.VocabularyId == VocabId).FirstOrDefaultAsync();
            if (temp == null) return false;
            Database.PersonVocabulary.Remove(temp);
            Database.SaveChanges();
            return true;
        }
        public async Task<Person> UpdateAsync(Person p)
        {
            var temp = await Database.Persons.Where(w => w.Id == p.Id).FirstOrDefaultAsync();
            if (temp == null) return null;
            temp.MainLanguageId = p.MainLanguageId;
            temp.Name = p.Name;
            temp.RegistrationDate = p.RegistrationDate;
            Database.SaveChanges();
            return temp;
        }
        public async Task<WordOfTheDay> GetWordOfTheDayAsync(Int16 PersonId)
        {
            var temp = await Database.GetWordOfTheDayAsync(PersonId);
           
            return Database.WordsOfTheDay.Where(w => w.PersonId == PersonId).LastOrDefault();
        }
    }
}
