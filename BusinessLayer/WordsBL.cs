using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class WordsBL
    {
        private IDbContext Database;
        public WordsBL(IDbContext db)
        {
            this.Database = db;
        }
        public List<Word> GetVocabularyWords(Int16 vocabularyId)
        {
            return Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId).Select(s => s.Word).ToList();
        }
        public List<Word> GetPersonWords(Int16 personId)
        {
            var temp = Database.PersonVocabulary.Where(w => w.PersonId == personId).Select(s => s.VocabularyId).ToList();
            List<Word> result = new List<Word>();
            foreach (var vocabularyId in temp)
            {
                result.AddRange(Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId).Select(s => s.Word));
            }
            return result;
        }
        public List<Word> GetWords()
        {
            return Database.Words.ToList();
        }
        public List<Word> GetLanguageWords(string languageName)
        {
            return Database.Words.Where(w => w.Language.Name == languageName).ToList();
        }

        public Word SwitchLanguage(Word original, string languageName)
        {
            languageName = languageName.Trim();
            return Database.Words.Where(w => w.WordId == original.WordId && w.Language.Name == languageName)
                .FirstOrDefault();
        }
        public Word SwitchLanguage(Word original, Int16 languageId)
        {
            return Database.Words.Where(w => w.WordId == original.WordId && w.Language.Id == languageId)
                .FirstOrDefault();
        }
        public Word SwitchLanguage(Word original, Language language)
        {
            return Database.Words.Where(w => w.WordId == original.WordId && w.Language == language)
                .FirstOrDefault();
        }
        public void AddWord(Word word)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            // set ID here or set it in API and check here?
            if (Database.Words.Any(w => w.Id == word.Id))
            {
                throw new Exception("Id already exists");
            }
            // reference on WordId table
            if (Database.Words.Where(w => w.LanguageId == word.LanguageId && w.WordId == word.WordId).Count() > 0)
            {
                throw new Exception("Word already exists");
            }
            Database.Words.Add(word);
            Database.SaveChanges();
        }
        public void AddWord(IEnumerable<Word> words)
        {
            Database.Configuration.ValidateOnSaveEnabled = false;
            // set ID here or set it in API and check here?
            foreach (var word in words)
            {
                if (Database.Words.Any(w => w.Id == word.Id))
                {
                    throw new Exception("Id already exists");
                }
                // reference on WordId table
                if (Database.Words.Where(w => w.LanguageId == word.LanguageId && w.WordId == word.WordId).Count() > 0)
                {
                    throw new Exception("Word already exists");
                }
                Database.Words.Add(word);
            }
            Database.SaveChanges();
        }
        public List<Word> Sort<TKey>(Func<Word, TKey> selector)
        {
            return Database.Words.OrderBy(selector).ToList();
        }

    }
}
