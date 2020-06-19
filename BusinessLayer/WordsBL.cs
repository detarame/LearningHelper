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
           // Database.Configuration.ValidateOnSaveEnabled = false;
        }


        public List<Word> GetWords()
        {
            return Database.Words.ToList();
        }
        public List<Word> GetLanguageWords(Int16 langId)
        {
            return Database.Words.Where(w => w.LanguageId == langId).ToList();
        }
        public Word GetWord_ById(Int16 wordId)
        {
            return Database.Words.Where(w => w.Id == wordId).FirstOrDefault();
        }

        public Word AddWord(Word word)
        {
            var temp = Database.Words.Where(w => w.LanguageId == word.LanguageId && w.WordId == word.WordId).FirstOrDefault();
            if (temp != null)
            {
                return temp; 
            }
            Database.Words.Add(word);
            Database.SaveChanges();
            return word;
        }
        
        public bool Delete(Int16 wordId)
        {
            var temp = Database.Words.Where(w => w.Id == wordId).FirstOrDefault();
            if (temp == null)
            {
                return false;
            }
            var temp2 = Database.VocabularyWords.Where(w => w.WordId == wordId).ToList();
            Database.Words.Remove(temp);
            foreach (var item in temp2)
            {
                Database.VocabularyWords.Remove(item);
            }
            Database.SaveChanges();
            return true;
        }
      
        public Word Update(Word p)
        {
            var temp = Database.Words.Where(w => w.Id == p.Id).FirstOrDefault();
            temp.LanguageId = p.LanguageId;
            temp.Value = p.Value;
            temp.WordId = p.WordId;
            Database.SaveChanges();
            return temp;
        }

        public Word SwitchLanguage(Int16 wordId, Int16 languageId)
        {
            return Database.Words.Where(w => w.WordId == wordId && w.LanguageId == languageId)
                .FirstOrDefault();
        }

    }
}
