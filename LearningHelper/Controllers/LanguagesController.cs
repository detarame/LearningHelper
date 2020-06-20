using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using LearningHelper.Models;

namespace LearningHelper.Controllers
{
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
        public List<LanguageAPI> Get()
        {
            var users = mapper.Map<List<LanguageAPI>>(database.GetLanguages());
            return users;
        }
    }
}
