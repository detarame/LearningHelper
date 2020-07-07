using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using LearningHelper.Filters;
using LearningHelper.Models;

namespace LearningHelper.Controllers
{
    [ExtendedLoggingFilter]
    public class LanguagesController : ApiController
    {
        public  LanguagesBL database;
        Mapper mapper;
        public LanguagesController(IDbContext t)
        {
            this.database = new LanguagesBL(t);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Language, LanguageAPI>());
            mapper = new Mapper(config);
        }
        [HttpGet]
        [Route("api/Language")]
        public async Task<List<LanguageAPI>> Get()
        {
            var users = mapper.Map<List<LanguageAPI>>(await database.GetLanguages());
            return users;
        }

        [HttpGet]
        [Route("api/Language/{langId}")]
        public async Task<LanguageAPI> GetById(Int16 langId)
        {
            var users = mapper.Map<LanguageAPI>(await database.GetLanguage(langId));
            return users;
        }
    }
}
