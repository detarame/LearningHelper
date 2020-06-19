using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessLayer;
using DataLayer;
using LearningHelper.Models;

namespace LearningHelper.Controllers
{
    public class LanguagesController : ApiController
    {
        public  LanguagesBL database;
        public LanguagesController(IDbContext t)
        {
            this.database = new LanguagesBL(t);
        }
        [HttpGet]
        public List<LanguageAPI> Get()
        {
            var temp = new List<LanguageAPI>();
            foreach (var item in database.GetLanguages())
            {
                temp.Add(LanguageAPI.DbToApi(item));
            }
            return temp;
        }
    }
}
