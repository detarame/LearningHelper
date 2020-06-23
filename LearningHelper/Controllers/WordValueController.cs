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
using System.Threading.Tasks;
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
        public async Task<List<WordAPI>> Get()
        {
            return mapperToAPI.Map<List<WordAPI>>(await WordsBL.GetWordsAsync());
        }
        
        [HttpGet]
        [Route("api/Words/language/{langId}")]
        public async Task<List<WordAPI>> GetLang(Int16 langId)
        {
            return mapperToAPI.Map<List<WordAPI>>(await WordsBL.GetLanguageWordsAsync(langId));
        }
        
        [HttpPost]
        [Route("api/Words")]
        public async Task<WordAPI> CreateWord(WordAPI p)
        {
            return mapperToAPI.Map< WordAPI>(await WordsBL.AddWordAsync(mapperToDB.Map<Word>(p)));
        }
       
        [HttpDelete]
        [Route("api/Words")]
        public async Task<IHttpActionResult> DeleteWord(Int16 id)
        {
            if (await WordsBL.DeleteAsync(id))
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
        public async Task<WordAPI> Update(WordAPI p)
        {
            return mapperToAPI.Map<WordAPI>(await WordsBL.UpdateAsync(mapperToDB.Map<Word>(p)));
        }

        [HttpGet]
        [Route("api/words/switchedLanguage")]
        public async Task<WordAPI> SwitchLang(Int16 wordId, Int16 langId)
        {
            return mapperToAPI.Map<WordAPI>(await WordsBL.SwitchLanguageAsync(wordId, langId));
        }
    }
}
