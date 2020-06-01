using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class LanguagesBL
    {
        private IDbContext Database;
        public LanguagesBL(IDbContext dbContext)
        {
            Database = dbContext;
        }
        public List<Language> GetLanguages()
        {
            return Database.Languages.ToList();
        }
        public Language GetLanguage(Int16 id)
        {
            return Database.Languages.Where(w => w.Id == id).First();
        }
    }
}
