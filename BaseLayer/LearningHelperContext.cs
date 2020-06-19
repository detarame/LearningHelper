namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;

    public class LearningHelperContext : DbContext, IDbContext
    {
        public LearningHelperContext()
            : base("name=IDbContext")
        {
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<PersonVocabulary> PersonVocabulary { get; set; }
        public DbSet<VocabularyWord> VocabularyWords { get; set; }
        public DbSet<WordId> WordIds { get; set; }
        public DbSet<WordOfTheDay> WordsOfTheDay { get; set; }
        public int GetWordOfTheDay(Int16 PersonId)
        {
            var clientIdParameter = new SqlParameter("@PersonId", PersonId);

            var temp = this.Database.ExecuteSqlCommand("ChooseWordOfTheDay @PersonId", clientIdParameter);
            
            return temp;
        }
    }
}