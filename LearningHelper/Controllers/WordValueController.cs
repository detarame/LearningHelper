using BusinessLayer;
using DataLayer;
using DataLayer.Models;
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
        public ContextLogic database;
        public WordValueController(ContextLogic t)
        {
            this.database = t;
        }
        [HttpGet]
        public List<Word> GetVoc(Int16 VocabularyId)
        {
            return database.GetVocabularyWords(VocabularyId);
        }
    }
}
