namespace DataLayer
{
    using DataLayer.Models;
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class LearningHelperContext : DbContext, IDbContext
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LearningHelperContext()
            : base("name=IDbContext")
        {
            this.Configuration.ValidateOnSaveEnabled = false;
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<PersonVocabulary> PersonVocabulary { get; set; }
        public DbSet<VocabularyWord> VocabularyWords { get; set; }
        public DbSet<WordId> WordIds { get; set; }
        public DbSet<WordOfTheDay> WordsOfTheDay { get; set; }
        public async Task<int> GetWordOfTheDayAsync(Int16 PersonId)
        {
            var clientIdParameter = new SqlParameter("@PersonId", PersonId);
            try
            {
                var temp = await this.Database.ExecuteSqlCommandAsync("ChooseWordOfTheDay @PersonId", clientIdParameter);
                return temp;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
            }
            return -1;
        }
    }
}