using DataLayer.Models;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;
using LearningHelper.Models;
using AutoMapper;

namespace LearningHelper.Controllers
{
    public class VocabulariesController : ApiController
    {
        public VocabulariesBL VocabulariesBL;
        Mapper mapperToAPI;
        Mapper mapperToDB;
        public VocabulariesController(IDbContext t)
        {
            VocabulariesBL = new VocabulariesBL(t);
            mapperToAPI = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Vocabulary, VocabularyAPI>()));
            mapperToDB = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<VocabularyAPI, Vocabulary>()));
        }
        [HttpGet]
        [Route("api/Vocabularies")]
        public List<VocabularyAPI> Get()
        {
            return mapperToAPI.Map<List<VocabularyAPI>>(VocabulariesBL.GetVocabularies());
        }
        
        [HttpGet]
        [Route("api/Vocabularies/{id}")]
        public VocabularyAPI GetVocabulary(Int16 id)
        {
            return mapperToAPI.Map<VocabularyAPI>(VocabulariesBL.GetVocabulary(id));
        }

        [HttpGet]
        [Route("api/Vocabulary/{vocId}/Words")]
        public List<WordAPI> GetVoc(Int16 vocId)
        {
            var wordMapperToAPI = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Word, WordAPI>()));
            return wordMapperToAPI.Map<List<WordAPI>>(VocabulariesBL.GetVocabularyWords(vocId));
        }
        
        [HttpPut]
        [Route("api/Vocabularies")]
        public VocabularyAPI Update(VocabularyAPI p)
        {
            return mapperToAPI.Map<VocabularyAPI>(VocabulariesBL.Update(mapperToDB.Map<Vocabulary>(p)));
        }
        
        [HttpPost]
        [Route("api/Vocabularies")]
        public VocabularyAPI Post(VocabularyAPI vocab)
        {
            return mapperToAPI.Map<VocabularyAPI>(VocabulariesBL.AddVocabulary(mapperToDB.Map<Vocabulary>(vocab)));
        }
                
        [HttpPost]
        [Route("api/Vocabulary/{vocId}/Word")]
        public IHttpActionResult AddWord(Int16 wordId, Int16 vocId)
        {
            if (VocabulariesBL.AddWord(wordId, vocId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
        }
        
        [HttpDelete]
        [Route("api/Vocabulary/{vocId}/Word")]
        public IHttpActionResult DeleteWord(Int16 wordId, Int16 vocId)
        {
            if (VocabulariesBL.DeleteFromVoc(wordId, vocId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return StatusCode(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/Vocabularies")]
        public IHttpActionResult Delete(Int16 id)
        {
            if (VocabulariesBL.Delete(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            return StatusCode(HttpStatusCode.NotFound);
        }
    }
}
