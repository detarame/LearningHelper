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
    public class PersonsController : ApiController
    {
        public PersonBL personBL;
        Mapper mapperToAPI;
        Mapper mapperToDB;
        public PersonsController(IDbContext t)
        {
            personBL = new PersonBL(t);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, PersonAPI>());
            mapperToAPI = new Mapper(config);
            mapperToDB = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<PersonAPI, Person>()));
        }
        [HttpGet]
        [Route("api/Person")]
        public List<PersonAPI> Get()
        {
            return mapperToAPI.Map<List<PersonAPI>>(personBL.GetPeople());
        }
        
        [HttpGet]
        [Route("api/Person/{id}")]
        public PersonAPI GetOne(Int16 id)
        {
            return mapperToAPI.Map<PersonAPI>(personBL.GetPerson(id));
        }
       
        [HttpGet]
        [Route("api/PersonName/{name}")]
        public List<PersonAPI> GetOne(string name)
        {
            return mapperToAPI.Map<List<PersonAPI>>(personBL.GetPeople((Person p) => p.Name.StartsWith(name)));
        }
        
        [HttpGet]
        [Route("api/Person/{id}/Vocabulary")]
        public List<VocabularyAPI> GetPersonsVocs(Int16 id)
        {
            var vocabMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Vocabulary, VocabularyAPI>()));
            return vocabMapper.Map<List<VocabularyAPI>>(personBL.GetPersonVocabularies(id));
        }
        
        [HttpPost]
        [Route("api/Person")]
        public PersonAPI Post(PersonAPI p)
        {
            return mapperToAPI.Map<PersonAPI>(personBL.AddPerson(mapperToDB.Map<Person>(p)));
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
            return mapperToAPI.Map<PersonAPI>(personBL.Update(mapperToDB.Map<Person>(p)));
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
            var wordMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Word, WordAPI>()));
            return wordMapper.Map<List<WordAPI>>(personBL.GetPersonWords(personId));
        }
    }
}
