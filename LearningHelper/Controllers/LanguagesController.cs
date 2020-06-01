using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLayer;
using DataLayer;

namespace LearningHelper.Controllers
{
    public class LanguagesController : ApiController
    {
        public readonly ContextLogic database;
        public LanguagesController(ContextLogic t)
        {
            this.database = t;
        }
        [HttpGet]
        public List<Language> Get()
        {
            return database.GetLanguages();
        }
    }
}
