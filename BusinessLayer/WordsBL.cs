using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<List<Word>> GetWordsAsync()
        {
            return await Database.Words.ToListAsync();
        }
        public async Task<List<Word>> GetLanguageWordsAsync(Int16 langId)
        {
            return await Database.Words.Where(w => w.LanguageId == langId).ToListAsync();
        }
        public async Task<Word> GetWordByIdAsync(Int16 wordId)
        {
            return await Database.Words.Where(w => w.Id == wordId).FirstOrDefaultAsync();
        }

        public async Task<Word> AddWordAsync(Word word)
        {
            var temp = await Database.Words.Where(w => w.LanguageId == word.LanguageId && w.WordId == word.WordId)
                .FirstOrDefaultAsync();
            if (temp != null)
            {
                return temp; 
            }
            Database.Words.Add(word);
            Database.SaveChanges();
            return word;
        }
        
        public async Task<bool> DeleteAsync(Int16 wordId)
        {
            var requestedWord = await Database.Words.Where(w => w.Id == wordId).FirstOrDefaultAsync();
            if (requestedWord == null)
            {
                return false;
            }
            var linkVocab = await Database.VocabularyWords.Where(w => w.WordId == wordId).ToListAsync();
            Database.Words.Remove(requestedWord);
            Database.VocabularyWords.RemoveRange(linkVocab);
            Database.SaveChanges();
            return true;
        }
      
        public async Task<Word> UpdateAsync(Word p)
        {
            var temp = await Database.Words.Where(w => w.Id == p.Id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return null;
            }
            temp.LanguageId = p.LanguageId;
            temp.Value = p.Value;
            temp.WordId = p.WordId;
            Database.SaveChanges();
            return temp;
        }

        public async Task<Word> SwitchLanguageAsync(Int16 wordId, Int16 languageId)
        {
            return await Database.Words.Where(w => w.WordId == wordId && w.LanguageId == languageId)
                .FirstOrDefaultAsync();
        }

    }
}
