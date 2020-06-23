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
    public class LanguagesBL
    {
        private IDbContext Database;
        public LanguagesBL(IDbContext dbContext)
        {
            Database = dbContext;
        }
        public async Task<List<Language>> GetLanguages()
        {
            return await Database.Languages.ToListAsync();
        }
        public async Task<Language> GetLanguage(Int16 id)
        {
            return await Database.Languages.Where(w => w.Id == id).FirstOrDefaultAsync();
        }
    }
}
