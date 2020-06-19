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
    public class PersonsController : ApiController
    {
        public PersonBL personBL;
        public PersonsController(IDbContext t)
        {
            personBL = new PersonBL(t);
        }
        [HttpGet]
        [Route("api/Person")]
        public List<PersonAPI> Get()
        {
            List<PersonAPI> result = new List<PersonAPI>();
            foreach (var item in personBL.GetPeople())
            {
                result.Add(PersonAPI.DbToApi(item));
            }
            return result;
        }
        
        [HttpGet]
        [Route("api/Person/{id}")]
        public PersonAPI GetOne(Int16 id)
        {
            return PersonAPI.DbToApi(personBL.GetPerson(id));
        }
       
        [HttpGet]
        [Route("api/PersonName/{name}")]
        public List<PersonAPI> GetOne(string name)
        {
            var tmp = new List<PersonAPI>();
            foreach (var item in personBL.GetPeople((Person p) => p.Name.StartsWith(name)))
            {
                tmp.Add(PersonAPI.DbToApi(item));
            }
            return tmp;
        }
        
        [HttpGet]
        [Route("api/Person/{id}/Vocabulary")]
        public List<VocabularyAPI> GetPersonsVocs(Int16 id)
        {
            var temp = new List<VocabularyAPI>();
            foreach (var item in personBL.GetPersonVocabularies(id))
            {
                temp.Add(VocabularyAPI.DbToApi(item));
            }
            return temp;
        }
        
        [HttpPost]
        [Route("api/Person")]
        public PersonAPI Post(PersonAPI p)
        {
            var temp = personBL.AddPerson(p.ApiToDb());
            return PersonAPI.DbToApi(temp);
        }
        
        [HttpDelete]
        [Route("api/Person/{id}")]
        public IHttpActionResult Delete(Int16 id)
        {
            if (personBL.DeletePerson(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.NotModified);
        }
        
        [HttpDelete]
        [Route("api/Person/{personId}/Vocabulary")]
        public IHttpActionResult DeletePersonal(Int16 vocabId, Int16 personId)
        {
            if (personBL.DeletePersonVocab(vocabId, personId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.BadRequest);
        }
        
        [HttpPost]
        [Route("api/Person/{personId}/Vocabulary")]
        public IHttpActionResult PostByID(Int16 vocabId, Int16 personId)
        {
            if (personBL.AddVocabularyToPerson(vocabId, personId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.BadRequest);
        }
       
        [HttpPut]
        [Route("api/Person")]
        public PersonAPI Put(PersonAPI p)
        {
            return PersonAPI.DbToApi(personBL.Update(p.ApiToDb()));
        }
        
        [HttpGet]
        [Route("api/Person/{PersonId}/WordOfTheDay")]
        public WordOfTheDayAPI GetWordOfTheDay(Int16 PersonId)
        {
            return WordOfTheDayAPI.DbToApi(personBL.GetWordOfTheDay(PersonId));
        }
        
        [HttpGet]
        [Route("api/Person/{personId}/Words")]
        public List<WordAPI> GetPers(Int16 personId)
        {
            var temp = new List<WordAPI>();
            foreach (var item in personBL.GetPersonWords(personId))
            {
                temp.Add(WordAPI.DbToApi(item));
            }
            return temp;
        }
    }
}
