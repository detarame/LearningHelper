using DataLayer;
using DataLayer.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.AsyncSetups
{
    public static class MockSetup
    {
        public static Mock<IDbContext> GetMock()
        {
            var mockContext = new Mock<IDbContext>();

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
                new VocabularyWord {  WordId = 1, VocabularyId = 1, Word = new Word { Id = 1 } },
                new VocabularyWord {   WordId = 1, VocabularyId = 2, Word = new Word { Id = 1 }  },
                new VocabularyWord {   WordId = 2, VocabularyId = 1, Word = new Word { Id = 2 }  },
                new VocabularyWord {   WordId = 2, VocabularyId = 2, Word = new Word { Id = 2 }  }
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

            return mockContext;
        }
    }
}
