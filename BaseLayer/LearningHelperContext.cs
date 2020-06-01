namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Data.Entity;
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

    }


}