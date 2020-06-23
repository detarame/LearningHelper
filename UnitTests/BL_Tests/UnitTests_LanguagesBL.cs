using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.AsyncSetups;

namespace UnitTests
{
    [TestClass]
    public class UnitTests_LanguagesBL
    {
        Mock<IDbContext> mockContext;
        LanguagesBL languagesBL;
        public UnitTests_LanguagesBL()
        {
            var data = new List<Language>
            {
                new Language { Id = 1, Name = "BBB" },
                new Language { Id = 2, Name = "ZZZ" },
                new Language { Id = 3, Name = "AAA" },
            };

            var mockSetLanguage = new MockAsyncData<Language>().MockAsyncQueryResult(data.AsQueryable());
            mockContext = new Mock<IDbContext>();
            mockContext.Setup(c => c.Languages).Returns(mockSetLanguage.Object);

            languagesBL = new LanguagesBL(mockContext.Object);

        }
        [TestMethod]
        public async Task Test_GetLanguage_ById()
        {
            // act
            var language = await languagesBL.GetLanguage(1);

            // assert
            Assert.AreEqual("BBB", language.Name);
        }
        [TestMethod]
        public async Task Test_GetLanguages_ProperCount()
        {
            // act
            var language = await languagesBL.GetLanguages();

            // assert
            Assert.IsTrue(language.Count == 3);
        }
    }
}
