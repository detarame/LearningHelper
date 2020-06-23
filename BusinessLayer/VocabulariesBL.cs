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
    public class VocabulariesBL
    {
        private IDbContext Database;
        public VocabulariesBL(IDbContext db)
        {
            this.Database = db;
        }
        public async Task<List<Vocabulary>> GetVocabulariesAsync()
        {
            return await Database.Vocabularies.ToListAsync();
        }
        public async Task<Vocabulary> GetVocabularyAsync(Int16 id)
        {
            return await Database.Vocabularies.Where(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Word>> GetVocabularyWordsAsync(Int16 vocabularyId)
        {
            return await Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId)
                .Select(s => s.Word).ToListAsync();
        }

        public Vocabulary AddVocabulary(Vocabulary vocabulary)
        {
            Database.Vocabularies.Add(vocabulary);
            Database.SaveChanges();
            return vocabulary;
        }
        public async Task<bool> AddWordAsync(Int16 wordId, Int16 vocId)
        {
            var exists = await Database.VocabularyWords.AnyAsync(w => w.VocabularyId == vocId && w.WordId == wordId);
            if (exists)
            {
                return false; // word already exists
            }
            var temp = new VocabularyWord();
            temp.VocabularyId = vocId;
            temp.WordId = wordId;
            Database.VocabularyWords.Add(temp);
            Database.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAsync(Int16 id)
        {
            // Delete vocab and its relations everywhere
            var requestedVocab = await Database.Vocabularies.Where(w => w.Id == id).FirstOrDefaultAsync();
            if (requestedVocab == null)
            {
                return false;
            }
            var wordLinks = await Database.VocabularyWords.Where(w => w.VocabularyId == id).ToListAsync();
            Database.VocabularyWords.RemoveRange(wordLinks);
            var personLinks = Database.PersonVocabulary.Where(w => w.VocabularyId == id).ToList();
            Database.PersonVocabulary.RemoveRange(personLinks);
            Database.Vocabularies.Remove(requestedVocab);
            Database.SaveChanges();
            return true;
        }
        public async Task<bool> DeleteFromVocAsync(Int16 wordId, Int16 vocId)
        {
            var temp = await Database.VocabularyWords.Where(w => w.WordId == wordId && w.VocabularyId == vocId)
                .FirstOrDefaultAsync();
            if (temp == null)
            { 
                return false; 
            }
            Database.VocabularyWords.Remove(temp);
            Database.SaveChanges();
            return true;
        }
        public async Task<Vocabulary> UpdateAsync(Vocabulary vocabulary)
        {
            var temp = await Database.Vocabularies.Where(w => w.Id == vocabulary.Id).FirstOrDefaultAsync();
            if (temp == null)
            {
                return null;
            }
            temp.LanguageId = vocabulary.LanguageId;
            temp.Name = vocabulary.Name;
            temp.Theme = vocabulary.Theme;
            temp.CreationDate = vocabulary.CreationDate;
            Database.SaveChanges();
            return temp;
        }
    }
}
