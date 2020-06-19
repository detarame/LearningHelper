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
            //Database.Configuration.ValidateOnSaveEnabled = false;
        }
        public List<Vocabulary> GetVocabularies()
        {
            return Database.Vocabularies.ToList();
        }
        public Vocabulary GetVocabulary(Int16 id)
        {
            return Database.Vocabularies.Where(p => p.Id == id).FirstOrDefault();
        }
        public List<Word> GetVocabularyWords(Int16 vocabularyId)
        {
            return Database.VocabularyWords.Where(w => w.VocabularyId == vocabularyId)
                .Select(s => s.Word).ToList();
        }

        public Vocabulary AddVocabulary(Vocabulary vocabulary)
        {
            Database.Vocabularies.Add(vocabulary);
            Database.SaveChanges();
            return vocabulary;
        }
        public bool AddWord(Int16 wordId, Int16 vocId)
        {
            if (Database.VocabularyWords.Where(w => w.VocabularyId == vocId && w.WordId == wordId).Count() > 0)
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

        public bool Delete(Int16 id)
        {
            // Delete vocab and its relations everywhere
            if (Database.Vocabularies.Where(w => w.Id == id).FirstOrDefault() == null)
            {
                return false;
            }
            var temp = Database.VocabularyWords.Where(w => w.VocabularyId == id).ToList();
            var temp2 = Database.Vocabularies.Where(w => w.Id == id).FirstOrDefault();
            var temp3 = Database.PersonVocabulary.Where(w => w.VocabularyId == id).ToList();
            foreach (var item in temp)
            {
                Database.VocabularyWords.Remove(item);
            }
            foreach (var item in temp3)
            {
                Database.PersonVocabulary.Remove(item);
            }
            Database.Vocabularies.Remove(temp2);
            Database.SaveChanges();
            return true;
        }
        public bool DeleteFromVoc(Int16 wordId, Int16 vocId)
        {
            var temp = Database.VocabularyWords.Where(w => w.WordId == wordId && w.VocabularyId == vocId)
                .FirstOrDefault();
            if (temp == null)
            { 
                return false; 
            }
            Database.VocabularyWords.Remove(temp);
            Database.SaveChanges();
            return true;
        }
        public Vocabulary Update(Vocabulary vocabulary)
        {
            var temp = Database.Vocabularies.Where(w => w.Id == vocabulary.Id).FirstOrDefault();
            temp.LanguageId = vocabulary.LanguageId;
            temp.Name = vocabulary.Name;
            temp.Theme = vocabulary.Theme;
            temp.CreationDate = vocabulary.CreationDate;
            Database.SaveChanges();
            return temp;
        }
    }
}
