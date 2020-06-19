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
        public WordValueController(IDbContext t)
        {
            this.WordsBL = new WordsBL(t);
        }
        [HttpGet]
        [Route("api/Words")]
        public List<WordAPI> Get()
        {
            var temp = new List<WordAPI>();
            foreach (var item in WordsBL.GetWords())
            {
                temp.Add(WordAPI.DbToApi(item));
            }
            return temp;
        }
        
        [HttpGet]
        [Route("api/Words/language/{langId}")]
        public List<WordAPI> GetLang(Int16 langId)
        {
            var temp = new List<WordAPI>();
            foreach (var item in WordsBL.GetLanguageWords(langId))
            {
                temp.Add(WordAPI.DbToApi(item));
            }
            return temp;
        }
        
        [HttpPost]
        [Route("api/Words")]
        public WordAPI CreateWord(WordAPI p)
        {
            var temp = WordsBL.AddWord(p.ApiToDb());
            return WordAPI.DbToApi(temp);
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
            return WordAPI.DbToApi( WordsBL.Update(p.ApiToDb()));
        }

        [HttpGet]
        [Route("api/words/switchedLanguage")]
        public WordAPI SwitchLang(Int16 wordId, Int16 langId)
        {
            return WordAPI.DbToApi(WordsBL.SwitchLanguage(wordId, langId));
        }
    }
}
