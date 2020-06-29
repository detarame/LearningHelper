using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.AsyncSetups;

namespace UnitTests
{
    [TestClass]
    public class UnitTests_PersonBL
    {
        Mock<IDbContext> mockContext;
        PersonBL personBL;
        public UnitTests_PersonBL()
        {
            mockContext = MockSetup.GetMock();

            personBL = new PersonBL(mockContext.Object);
        }
        [TestMethod]
        public async Task GetPersons_All_ProperCount()
        {
            //act
            var temp = await personBL.GetPeopleAsync();

            //assert
            Assert.AreEqual(4, temp.Count);
        }
        [TestMethod]
        public async Task GetPersons_All_OrderedByName()
        {
            //act
            var temp = await personBL.GetOrderedPeopleAsync<string>(s => s.Name);

            //assert
            Assert.AreEqual(2, temp.First().Id);
            Assert.AreEqual(1, temp.Last().Id);
        }
        [TestMethod]
        public async Task GetPerson_ById_ReturnsPaul()
        {
            //act
            var temp = await personBL.GetPersonAsync(1);

            //assert
            Assert.AreEqual("Paul", temp.Name);
        }
        [TestMethod]
        public async Task GetPersons_ByName_ReturnsTwo()
        {
            var name = "Ev";
            //act
            var temp = await personBL.GetPeopleAsync(s => s.Name.StartsWith(name));

            //assert
            Assert.AreEqual(2, temp.Count);
            Assert.IsTrue(temp.All(a => a.Name.StartsWith(name)));
        }
        [TestMethod]
        public async Task GetPersonWords_ByPersonId1_ReturnsAll()
        {
            //act
            var temp = await personBL.GetPersonWordsAsync(1);

            //assert
            Assert.AreEqual(4, temp.Count);
        }
        [TestMethod]
        public async Task GetPersonWords_ByPersonId3_ReturnsNull()
        {
            //act
            var temp = await personBL.GetPersonWordsAsync(3);

            //assert
            Assert.IsNull(temp);
        }
        [TestMethod]
        public async Task GetPersonVocabularies_ByPersonId1_ReturnsAll()
        {
            //act
            var temp = await personBL.GetPersonVocabulariesAsync(1);

            //assert
            Assert.AreEqual(2, temp.Count);
        }
        [TestMethod]
        public async Task GetPersonVocabularies_ByPersonId3_ReturnsNull()
        {
            //act
            var temp = await personBL.GetPersonVocabulariesAsync(3);

            //assert
            Assert.IsNull(temp);
        }


        [TestMethod]
        public async Task GetWordOfTheDay_ByPersonId()
        {
            // act
            var temp = await personBL.GetWordOfTheDayAsync(1);
            // assert
            Assert.IsNotNull(temp);
            Assert.IsTrue(temp.WordId == 1 || temp.WordId == 2);
            mockContext.Verify(m => m.GetWordOfTheDayAsync(It.IsAny<Int16>()), Times.Once());
        }
        
        [TestMethod]
        public void AddPerson_PassNew_ReturnsBack()
        {
            // arrange
            var temp = new Person();
            temp.Id = 10;
            // act
            var result = personBL.AddPerson(temp);
            // assert
            Assert.AreEqual(result.Id, temp.Id);
            Assert.IsNotNull(result);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        
        [TestMethod]
        public async Task AddVocabularyToPerson_PassExistingIds_ReturnsTrue()
        {
            // act
            var temp = await personBL.AddVocabularyToPersonAsync(1, 1);
            // assert
            Assert.IsTrue(temp);
        }
        [TestMethod]
        public async Task AddVocabularyToPerson_PassNonExistingIds_ReturnsFalse()
        {
            // act
            var temp = await personBL.AddVocabularyToPersonAsync(10, 9);
            // assert
            Assert.IsFalse(temp);
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }
        [TestMethod]
        public async Task DeletePerson_PassExistingId_ReturnsTrue()
        {
            // act
            var temp = await personBL.DeletePersonAsync(1);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsTrue(temp);
        }
        [TestMethod]
        public async Task DeletePerson_PassNonExistingId_ReturnsFalse()
        {
            // act
            var temp = await personBL.DeletePersonAsync(9);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.IsFalse(temp);
        }
        [TestMethod]
        public async Task DeletePersonVocab_PassExistingId_ReturnsTrue()
        {
            // act
            var temp = await personBL.DeletePersonVocabAsync(1, 1);

            // assert
            Assert.IsTrue(temp);
        }
        [TestMethod]
        public async Task DeletePersonVocab_PassNonExistingId_ReturnsFalse()
        {
            // act
            var temp = await personBL.DeletePersonVocabAsync(10, 3);

            // assert
            Assert.IsFalse(temp);
        }
        [TestMethod]
        public async Task Update_PassExistingPerson_ReturnsItAndUpdates()
        {
            // arrange
            var temp = new Person();
            temp.Id = 1;
            temp.Name = "NotPaul";
             // act
            var actual = await personBL.UpdateAsync(temp);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(mockContext.Object.Persons.Where(w => w.Id == 1).FirstOrDefault().Name, temp.Name);
            Assert.AreEqual(actual.Name, temp.Name);
        }
        [TestMethod]
        public async Task Update_PassNonExistingPerson_ReturnsNull()
        {
            // arrange
            var temp = new Person();
            temp.Id = 10;
            temp.Name = "NotPaul";
            // act
            var actual = await personBL.UpdateAsync(temp);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.IsNull(actual);
        }
    }
}
