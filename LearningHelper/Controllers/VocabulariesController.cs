using DataLayer.Models;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataLayer;

namespace LearningHelper.Controllers
{
    public class VocabulariesController : ApiController
    {
        //public ContextLogic database;
        public VocabulariesBL VocabulariesBL;
        public LanguagesBL LanguagesBL;
        public VocabulariesController(IDbContext t)
        {
            VocabulariesBL = new VocabulariesBL(t);
            LanguagesBL = new LanguagesBL(t);
        }
        [HttpGet]
        public List<Vocabulary> Get()
        {
            return VocabulariesBL.GetVocabularies();
        }
        [HttpPost]
        public void Post(Vocabulary p, Int16 personId)
        {
            var voc = new Vocabulary();
            voc.Name = p.Name;
            voc.Theme = p.Theme;
            voc.CreationDate = p.CreationDate;
            voc.Language = LanguagesBL.GetLanguage(p.LanguageId);
            VocabulariesBL.AddVocabulary(voc, personId);
        }
    }
}
