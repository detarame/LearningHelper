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
        public void Test_GetPersons_All()
        {
            //act
            var temp = personBL.GetPeople();

            //assert
            Assert.AreEqual(4, temp.Count);
        }
        [TestMethod]
        public void Test_GetPersons_All_Ordered()
        {
            //act
            var temp = personBL.GetOrderedPeople<string>(s => s.Name);

            //assert
            Assert.AreEqual(2, temp.First().Id);
        }
        [TestMethod]
        public void Test_GetPerson_ById()
        {
            //act
            var temp = personBL.GetPerson(1);

            //assert
            Assert.AreEqual("Paul", temp.Name);
        }
        [TestMethod]
        public void Test_GetPersons_ByName()
        {
            var name = "Ev";
            //act
            var temp = personBL.GetPeople(s => s.Name.StartsWith(name));

            //assert
            Assert.AreEqual(2, temp.Count);
            Assert.IsTrue(temp.All(a => a.Name.StartsWith(name)));
        }
        [TestMethod]
        public void Test_GetPersonWords_ByPersonId()
        {
            //act
            var temp = personBL.GetPersonWords(1);

            //assert
            Assert.AreEqual(4, temp.Count);
        }
        [TestMethod]
        public void Test_GetPersonVocabularies_ByPersonId()
        {
            //act
            var temp = personBL.GetPersonVocabularies(1);

            //assert
            Assert.AreEqual(2, temp.Count);
        }
       
        [TestMethod]
        public void GetWordOfTheDay_ByPersonId()
        {
            // act
            var temp = personBL.GetWordOfTheDay(1);
            // assert
            Assert.IsNotNull(temp);
            Assert.IsTrue(temp.WordId == 1 || temp.WordId == 2);
            mockContext.Verify(m => m.GetWordOfTheDay(It.IsAny<Int16>()), Times.Once());
        }
        
        [TestMethod]
        public void AddPerson()
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
        public void AddVocabularyToPerson()
        {
            // act
            var temp = personBL.AddVocabularyToPerson(3, 3);
            // assert
            Assert.IsTrue(temp);
        }
        
        [TestMethod]
        public void DeletePerson()
        {
            // act
            var temp = personBL.DeletePerson(1);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsTrue(temp);
        }
        
        [TestMethod]
        public void DeletePersonVocab()
        {
            // act
            var temp = personBL.DeletePersonVocab(3, 3);

            // assert
            Assert.IsFalse(temp);
        }
        
        [TestMethod]
        public void Update()
        {
            // arrange
            var temp = new Person();
            temp.Id = 1;
            temp.Name = "NotPaul";
             // act
            personBL.Update(temp);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(mockContext.Object.Persons.Where(w => w.Id == 1).FirstOrDefault().Name, temp.Name);
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
            }.AsQueryable();

            var mockSetPerson = new Mock<DbSet<Person>>();
            mockSetPerson.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(dataPerson.Provider);
            mockSetPerson.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(dataPerson.Expression);
            mockSetPerson.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(dataPerson.ElementType);
            mockSetPerson.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(dataPerson.GetEnumerator());
            mockContext.Setup(c => c.Persons).Returns(mockSetPerson.Object);
            #endregion

            #region Vocabulary
            var dataVocab = new List<Vocabulary>
            {
                new Vocabulary {  Id = 1, Name = "MyVocab", LanguageId = 1, Theme = "Random", CreationDate = DateTime.Today },
                new Vocabulary { Id = 2, Name = "Favorite", LanguageId = 4, Theme = "Flowers", CreationDate = DateTime.Today },
                new Vocabulary { Id = 3, Name = "Ori", LanguageId = 3, Theme = "Games", CreationDate = DateTime.Today},
                new Vocabulary { Id = 4, Name = "Blabla", LanguageId = 2, Theme = "Speech", CreationDate = DateTime.Today},
            }.AsQueryable();

            var mockSetVocab = new Mock<DbSet<Vocabulary>>();
            mockSetVocab.As<IQueryable<Vocabulary>>().Setup(m => m.Provider).Returns(dataVocab.Provider);
            mockSetVocab.As<IQueryable<Vocabulary>>().Setup(m => m.Expression).Returns(dataVocab.Expression);
            mockSetVocab.As<IQueryable<Vocabulary>>().Setup(m => m.ElementType).Returns(dataVocab.ElementType);
            mockSetVocab.As<IQueryable<Vocabulary>>().Setup(m => m.GetEnumerator()).Returns(dataVocab.GetEnumerator());

            mockContext.Setup(c => c.Vocabularies).Returns(mockSetVocab.Object);
            #endregion

            #region PersonVocabulary
            var dataPersonVocab = new List<PersonVocabulary>
            {
                new PersonVocabulary {   PersonId = 1, VocabularyId = 1 },
                new PersonVocabulary {   PersonId = 1, VocabularyId = 2 },
                new PersonVocabulary {   PersonId = 2, VocabularyId = 1 },
                new PersonVocabulary {   PersonId = 2, VocabularyId = 2 }
            }.AsQueryable();

            var mockSetPersonVocab = new Mock<DbSet<PersonVocabulary>>();
            mockSetPersonVocab.As<IQueryable<PersonVocabulary>>().Setup(m => m.Provider).Returns(dataPersonVocab.Provider);
            mockSetPersonVocab.As<IQueryable<PersonVocabulary>>().Setup(m => m.Expression).Returns(dataPersonVocab.Expression);
            mockSetPersonVocab.As<IQueryable<PersonVocabulary>>().Setup(m => m.ElementType).Returns(dataPersonVocab.ElementType);
            mockSetPersonVocab.As<IQueryable<PersonVocabulary>>().Setup(m => m.GetEnumerator()).Returns(dataPersonVocab.GetEnumerator());

            mockContext.Setup(c => c.PersonVocabulary).Returns(mockSetPersonVocab.Object);
            #endregion

            #region VocabularyWords
            var dataVocabWords = new List<VocabularyWord>
            {
                new VocabularyWord {  WordId = 1, VocabularyId = 1 },
                new VocabularyWord {   WordId = 1, VocabularyId = 2 },
                new VocabularyWord {   WordId = 2, VocabularyId = 1 },
                new VocabularyWord {   WordId = 2, VocabularyId = 2 }
            }.AsQueryable();

            var mockSetVocabWords = new Mock<DbSet<VocabularyWord>>();
            mockSetVocabWords.As<IQueryable<VocabularyWord>>().Setup(m => m.Provider).Returns(dataVocabWords.Provider);
            mockSetVocabWords.As<IQueryable<VocabularyWord>>().Setup(m => m.Expression).Returns(dataVocabWords.Expression);
            mockSetVocabWords.As<IQueryable<VocabularyWord>>().Setup(m => m.ElementType).Returns(dataVocabWords.ElementType);
            mockSetVocabWords.As<IQueryable<VocabularyWord>>().Setup(m => m.GetEnumerator()).Returns(dataVocabWords.GetEnumerator());

            mockContext.Setup(c => c.VocabularyWords).Returns(mockSetVocabWords.Object);
            #endregion

            #region Words
            var dataWords = new List<Word>
            {
                new Word { Id = 1, Value = "Hello", LanguageId = 1, WordId = 1 },
                new Word { Id = 2, Value = "Bonjour", LanguageId = 2, WordId = 1 },
                new Word { Id = 3, Value = "Thanks", LanguageId = 1, WordId = 2 },
                new Word { Id = 4, Value = "May", LanguageId = 1, WordId = 3 },
            }.AsQueryable();

            var mockSetWord = new Mock<DbSet<Word>>();
            mockSetWord.As<IQueryable<Word>>().Setup(m => m.Provider).Returns(dataWords.Provider);
            mockSetWord.As<IQueryable<Word>>().Setup(m => m.Expression).Returns(dataWords.Expression);
            mockSetWord.As<IQueryable<Word>>().Setup(m => m.ElementType).Returns(dataWords.ElementType);
            mockSetWord.As<IQueryable<Word>>().Setup(m => m.GetEnumerator()).Returns(dataWords.GetEnumerator());
            mockContext.Setup(c => c.Words).Returns(mockSetWord.Object);
            #endregion

            #region Languages
            var dataLanguage = new List<Language>
            {
                new Language { Id = 1, Name = "BBB" },
                new Language { Id = 2, Name = "ZZZ" },
                new Language { Id = 3, Name = "AAA" },
            }.AsQueryable();

            var mockSetLanguage = new Mock<DbSet<Language>>();
            mockSetLanguage.As<IQueryable<Language>>().Setup(m => m.Provider).Returns(dataLanguage.Provider);
            mockSetLanguage.As<IQueryable<Language>>().Setup(m => m.Expression).Returns(dataLanguage.Expression);
            mockSetLanguage.As<IQueryable<Language>>().Setup(m => m.ElementType).Returns(dataLanguage.ElementType);
            mockSetLanguage.As<IQueryable<Language>>().Setup(m => m.GetEnumerator()).Returns(dataLanguage.GetEnumerator());

            mockContext.Setup(c => c.Languages).Returns(mockSetLanguage.Object);
            #endregion

            #region WordsOfTheDay
            var dataWordsOfTheDay = new List<WordOfTheDay>
            {
                new WordOfTheDay { Id = 1,  PersonId = 1, WordId = 1, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 2,  PersonId = 1, WordId = 2, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 3,  PersonId = 2, WordId = 3, AddingDate = DateTime.Today},
                new WordOfTheDay { Id = 4,  PersonId = 3, WordId = 3, AddingDate = DateTime.Today}
            }.AsQueryable();

            var mockSetWordOfTheDay = new Mock<DbSet<WordOfTheDay>>();
            mockSetWordOfTheDay.As<IQueryable<WordOfTheDay>>().Setup(m => m.Provider).Returns(dataWordsOfTheDay.Provider);
            mockSetWordOfTheDay.As<IQueryable<WordOfTheDay>>().Setup(m => m.Expression).Returns(dataWordsOfTheDay.Expression);
            mockSetWordOfTheDay.As<IQueryable<WordOfTheDay>>().Setup(m => m.ElementType).Returns(dataWordsOfTheDay.ElementType);
            mockSetWordOfTheDay.As<IQueryable<WordOfTheDay>>().Setup(m => m.GetEnumerator()).Returns(dataWordsOfTheDay.GetEnumerator());
            mockContext.Setup(c => c.WordsOfTheDay).Returns(mockSetWordOfTheDay.Object);
            #endregion

        }
    }
}
