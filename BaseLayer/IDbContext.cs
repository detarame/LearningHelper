using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IDbContext
    {
        DbSet<Person> Persons { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<Word> Words { get; set; }
        DbSet<Vocabulary> Vocabularies { get; set; }
        DbSet<PersonVocabulary> PersonVocabulary { get; set; }
        DbSet<VocabularyWord> VocabularyWords { get; set; }
        DbSet<WordId> WordIds { get; set; }
        DbSet<WordOfTheDay> WordsOfTheDay { get; set; }
        int SaveChanges();
        DbContextConfiguration Configuration { get; }
        Task<int> GetWordOfTheDayAsync(Int16 PersonId);
    }
}
