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
            mockContext = new Mock<IDbContext>();
            Setups();
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
        public void Setups()
        {

            #region Person
            var dataPerson = new List<Person>
            {
                new Person { Id = 1, Name = "Paul", MainLanguageId = 1, RegistrationDate = DateTime.Today },
                new Person { Id = 2, Name = "Anna", MainLanguageId = 2, RegistrationDate = DateTime.Today  },
                new Person { Id = 3, Name = "Eva", MainLanguageId = 3, RegistrationDate = DateTime.Today  },
                new Person { Id = 4, Name = "Evera", MainLanguageId = 3, RegistrationDate = DateTime.Today  },
            };

            var mockSetPerson = new MockAsyncData<Person>().MockAsyncQueryResult(dataPerson.AsQueryable());
            mockContext.Setup(c => c.Persons).Returns(mockSetPerson.Object);

            #endregion

            #region Vocabulary
            var dataVocab = new List<Vocabulary>
            {
                new Vocabulary {  Id = 1, Name = "MyVocab", LanguageId = 1, Theme = "Random", CreationDate = DateTime.Today },
                new Vocabulary { Id = 2, Name = "Favorite", LanguageId = 4, Theme = "Flowers", CreationDate = DateTime.Today },
                new Vocabulary { Id = 3, Name = "Ori", LanguageId = 3, Theme = "Games", CreationDate = DateTime.Today},
                new Vocabulary { Id = 4, Name = "Blabla", LanguageId = 2, Theme = "Speech", CreationDate = DateTime.Today},
            };

            var mockSetVocab = new MockAsyncData<Vocabulary>().MockAsyncQueryResult(dataVocab.AsQueryable());
            mockContext.Setup(c => c.Vocabularies).Returns(mockSetVocab.Object);
            #endregion

            #region PersonVocabulary
            var dataPersonVocab = new List<PersonVocabulary>
            {
                new PersonVocabulary {   PersonId = 1, VocabularyId = 1 },
                new PersonVocabulary {   PersonId = 1, VocabularyId = 2 },
                new PersonVocabulary {   PersonId = 2, VocabularyId = 1 },
                new PersonVocabulary {   PersonId = 2, VocabularyId = 2 }
            };

            var mockSetPersonVocab = new MockAsyncData<PersonVocabulary>().MockAsyncQueryResult(dataPersonVocab.AsQueryable());
            mockContext.Setup(c => c.PersonVocabulary).Returns(mockSetPersonVocab.Object);
            #endregion

            #region VocabularyWords
            var dataVocabWords = new List<VocabularyWord>
            {
                new VocabularyWord {  WordId = 1, VocabularyId = 1 },
                new VocabularyWord {   WordId = 1, VocabularyId = 2 },
                new VocabularyWord {   WordId = 2, VocabularyId = 1 },
                new VocabularyWord {   WordId = 2, VocabularyId = 2 }
            };

            var mockSetVocabWords = new MockAsyncData<VocabularyWord>().MockAsyncQueryResult(dataVocabWords.AsQueryable());
            mockContext.Setup(c => c.VocabularyWords).Returns(mockSetVocabWords.Object);
            #endregion

            #region Words
            var dataWords = new List<Word>
            {
                new Word { Id = 1, Value = "Hello", LanguageId = 1, WordId = 1 },
                new Word { Id = 2, Value = "Bonjour", LanguageId = 2, WordId = 1 },
                new Word { Id = 3, Value = "Thanks", LanguageId = 1, WordId = 2 },
                new Word { Id = 4, Value = "May", LanguageId = 1, WordId = 3 },
            };

            var mockSetWord = new MockAsyncData<Word>().MockAsyncQueryResult(dataWords.AsQueryable());
            mockContext.Setup(c => c.Words).Returns(mockSetWord.Object);
            #endregion

            #region Languages
            var dataLanguage = new List<Language>
            {
                new Language { Id = 1, Name = "BBB" },
                new Language { Id = 2, Name = "ZZZ" },
                new Language { Id = 3, Name = "AAA" },
            };

            var mockSetLanguage = new MockAsyncData<Language>().MockAsyncQueryResult(dataLanguage.AsQueryable());
            mockContext.Setup(c => c.Languages).Returns(mockSetLanguage.Object);
            #endregion

            #region WordsOfTheDay
            var dataWordsOfTheDay = new List<WordOfTheDay>
            {
                new WordOfTheDay { Id = 1,  PersonId = 1, WordId = 1, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 2,  PersonId = 1, WordId = 2, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 3,  PersonId = 2, WordId = 3, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 4,  PersonId = 3, WordId = 3, AddingDate = DateTime.Today}
            };

            var mockSetWordOfTheDay = new MockAsyncData<WordOfTheDay>().MockAsyncQueryResult(dataWordsOfTheDay.AsQueryable());
            mockContext.Setup(c => c.WordsOfTheDay).Returns(mockSetWordOfTheDay.Object);
            #endregion

        }
    }
}
