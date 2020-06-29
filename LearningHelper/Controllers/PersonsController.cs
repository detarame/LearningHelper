using AutoMapper;
using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using LearningHelper.Filters;
using LearningHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;

namespace LearningHelper.Controllers
{
    [ControllerExceptionFilter]
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
        [ControllerExceptionFilter]
        public async Task<List<PersonAPI>> GetPeople()
        {
            var tmp = await personBL.GetPeopleAsync();
            return mapperToAPI.Map<List<PersonAPI>>(tmp);
        }

        [HttpGet]
        [Route("api/Person/{id}")]
        public async Task<PersonAPI> GetOne(Int16 id)
        {
            return mapperToAPI.Map<PersonAPI>(await personBL.GetPersonAsync(id));
        }
       
        [HttpGet]
        [Route("api/PersonName/{name}")]
        public async Task<List<PersonAPI>> GetOne(string name)
        {
            return mapperToAPI.Map<List<PersonAPI>>(await personBL.GetPeopleAsync((Person p) => p.Name.StartsWith(name)));
        }
        
        [HttpGet]
        [Route("api/Person/{id}/Vocabulary")]
        public async Task<List<VocabularyAPI>> GetPersonsVocs(Int16 id)
        {
            var vocabMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Vocabulary, VocabularyAPI>()));
            return vocabMapper.Map<List<VocabularyAPI>>(await personBL.GetPersonVocabulariesAsync(id));
        }
        
        [HttpPost]
        [Route("api/Person")]
        public PersonAPI Post(PersonAPI p)
        {
            return mapperToAPI.Map<PersonAPI>(personBL.AddPerson(mapperToDB.Map<Person>(p)));
        }
        
        [HttpDelete]
        [Route("api/Person/{id}")]
        public async Task<IHttpActionResult> DeletePerson(Int16 id)
        {
            if (await personBL.DeletePersonAsync(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return StatusCode(HttpStatusCode.NotModified); 
            }
        }
        
        [HttpDelete]
        [Route("api/Person/{personId}/Vocabulary")]
        public async Task<IHttpActionResult> DeletePersonVocab(Int16 vocabId, Int16 personId)
        {
            if (await personBL.DeletePersonVocabAsync(vocabId, personId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.BadRequest);
        }
        
        [HttpPost]
        [Route("api/Person/{personId}/Vocabulary")]
        public async Task<IHttpActionResult> PostByID(Int16 vocabId, Int16 personId)
        {
            if (await personBL.AddVocabularyToPersonAsync(vocabId, personId))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.BadRequest);
        }
       
        [HttpPut]
        [Route("api/Person")]
        public async Task<PersonAPI> Put(PersonAPI p)
        {
            return mapperToAPI.Map<PersonAPI>(await personBL.UpdateAsync(mapperToDB.Map<Person>(p)));
        }
        
        [HttpGet]
        [Route("api/Person/{PersonId}/Word-of-the-day")]
        public async Task<WordOfTheDayAPI> GetWordOfTheDay(Int16 PersonId)
        {
            var wordMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<WordOfTheDay, WordOfTheDayAPI>()));
            return wordMapper.Map<WordOfTheDayAPI>(await personBL.GetWordOfTheDayAsync(PersonId));
        }
        
        [HttpGet]
        [Route("api/Person/{personId}/Words")]
        public async Task<List<WordAPI>> GetPers(Int16 personId)
        {
            var wordMapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Word, WordAPI>()));
            return wordMapper.Map<List<WordAPI>>(await personBL.GetPersonWordsAsync(personId));
        }
    }
}
