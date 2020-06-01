using BusinessLayer;
using DataLayer;
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
        public List<Person> Get()
        {
            return personBL.GetPeople();
        }
        [HttpPost]
        public void Post(Person p, string LanguageName)
        {
            var person = new Person();
            person.Name = p.Name;
            person.RegistrationDate = p.RegistrationDate;
            person.Id = p.Id;
            // how to get Language from another table? in BL now
            personBL.AddPerson(person, LanguageName);
        }
        [HttpDelete]
        public IHttpActionResult Delete(Int16 id)
        {
            if (personBL.DeletePerson(id))
            {
                return StatusCode(HttpStatusCode.OK);
            }
            else return StatusCode(HttpStatusCode.BadRequest); 
        }
        //[HttpPut]
        //public void Put(int id, [FromBody] Person p)
        //{
        //    var selected = database.Persons.FirstOrDefault(i => i.PersonId == id);
        //    selected.PersonName = p.PersonName;
        //    selected.PersonLanguage = p.PersonLanguage;
        //    database.SaveChanges();
        //}
    }
}
