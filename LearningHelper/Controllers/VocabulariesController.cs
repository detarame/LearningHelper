﻿using DataLayer.Models;
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
using System.Threading.Tasks;
using LearningHelper.Filters;

namespace LearningHelper.Controllers
{
    [ExtendedLoggingFilter]
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
        public async Task<List<VocabularyAPI>> Get()
        {
            return mapperToAPI.Map<List<VocabularyAPI>>(await VocabulariesBL.GetVocabulariesAsync());
        }
        
        [HttpGet]
        [Route("api/Vocabularies/{id}")]
        public async Task<VocabularyAPI> GetVocabulary(Int16 id)
        {
            return mapperToAPI.Map<VocabularyAPI>(await VocabulariesBL.GetVocabularyAsync(id));
        }

        [HttpGet]
        [Route("api/Vocabulary/{vocId}/Words")]
        public async Task<List<WordAPI>> GetVoc(Int16 vocId)
        {
            var wordMapperToAPI = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Word, WordAPI>()));
            return wordMapperToAPI.Map<List<WordAPI>>(await VocabulariesBL.GetVocabularyWordsAsync(vocId));
        }
        
        [HttpPut]
        [Route("api/Vocabularies")]
        public async Task<VocabularyAPI> Update(VocabularyAPI p)
        {
            return mapperToAPI.Map<VocabularyAPI>(await VocabulariesBL.UpdateAsync(mapperToDB.Map<Vocabulary>(p)));
        }
        
        [HttpPost]
        [Route("api/Vocabularies")]
        public VocabularyAPI Post(VocabularyAPI vocab)
        {
            return mapperToAPI.Map<VocabularyAPI>(VocabulariesBL.AddVocabulary(mapperToDB.Map<Vocabulary>(vocab)));
        }
                
        [HttpPost]
        [Route("api/Vocabulary/{vocId}/Word")]
        public async Task<IHttpActionResult> AddWord(Int16 wordId, Int16 vocId)
        {
            if (await VocabulariesBL.AddWordAsync(wordId, vocId))
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
        public async Task<IHttpActionResult> DeleteWord(Int16 wordId, Int16 vocId)
        {
            if (await VocabulariesBL.DeleteFromVocAsync(wordId, vocId))
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
        public async Task<IHttpActionResult> Delete(Int16 id)
        {
            if (await VocabulariesBL.DeleteAsync(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            return StatusCode(HttpStatusCode.NotFound);
        }
    }
}
