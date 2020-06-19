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

namespace LearningHelper.Controllers
{
    public class VocabulariesController : ApiController
    {
        public VocabulariesBL VocabulariesBL;
        public VocabulariesController(IDbContext t)
        {
            VocabulariesBL = new VocabulariesBL(t);
        }
        [HttpGet]
        [Route("api/Vocabularies")]
        public List<VocabularyAPI> Get()
        {
            var temp = new List<VocabularyAPI>();
            foreach (var item in VocabulariesBL.GetVocabularies())
            {
                temp.Add(VocabularyAPI.DbToApi(item));
            }
            return temp;
        }
        
        [HttpGet]
        [Route("api/Vocabularies/{id}")]
        public VocabularyAPI GetVocabulary(Int16 id)
        {
            return VocabularyAPI.DbToApi(VocabulariesBL.GetVocabulary(id));
        }

        [HttpGet]
        [Route("api/Vocabulary/{vocId}/Words")]
        public List<WordAPI> GetVoc(Int16 vocId)
        {
            var temp = new List<WordAPI>();
            foreach (var item in VocabulariesBL.GetVocabularyWords(vocId))
            {
                temp.Add(WordAPI.DbToApi(item));
            }
            return temp;
        }
        
        [HttpPut]
        [Route("api/Vocabularies")]
        public VocabularyAPI Update(VocabularyAPI p)
        {
            return VocabularyAPI.DbToApi(VocabulariesBL.Update(p.ApiToDb()));
        }
        
        [HttpPost]
        [Route("api/Vocabularies")]
        public VocabularyAPI Post(VocabularyAPI vocab)
        {
            return VocabularyAPI.DbToApi( VocabulariesBL.AddVocabulary(vocab.ApiToDb()));
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
