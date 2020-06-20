using AutoMapper;
using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using LearningHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LearningHelper.Controllers
{
    public class WordValueController : ApiController
    {
        WordsBL WordsBL;
        Mapper mapperToAPI;
        Mapper mapperToDB;
        public WordValueController(IDbContext t)
        {
            this.WordsBL = new WordsBL(t);
            mapperToAPI = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Word, WordAPI>()));
            mapperToDB = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<WordAPI, Word>()));
        }
        [HttpGet]
        [Route("api/Words")]
        public List<WordAPI> Get()
        {
            return mapperToAPI.Map<List<WordAPI>>(WordsBL.GetWords());
        }
        
        [HttpGet]
        [Route("api/Words/language/{langId}")]
        public List<WordAPI> GetLang(Int16 langId)
        {
            return mapperToAPI.Map<List<WordAPI>>(WordsBL.GetLanguageWords(langId));
        }
        
        [HttpPost]
        [Route("api/Words")]
        public WordAPI CreateWord(WordAPI p)
        {
            return mapperToAPI.Map< WordAPI>(WordsBL.AddWord(mapperToDB.Map<Word>(p)));
        }
       
        [HttpDelete]
        [Route("api/Words")]
        public IHttpActionResult DeleteWord(Int16 id)
        {
            if (WordsBL.Delete(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
        }
        [HttpPut]
        [Route("api/Words")]
        public WordAPI Update(WordAPI p)
        {
            return mapperToAPI.Map<WordAPI>(WordsBL.Update(mapperToDB.Map<Word>(p)));
        }

        [HttpGet]
        [Route("api/words/switchedLanguage")]
        public WordAPI SwitchLang(Int16 wordId, Int16 langId)
        {
            return mapperToAPI.Map<WordAPI>(WordsBL.SwitchLanguage(wordId, langId));
        }
    }
}
