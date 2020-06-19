using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Language>>();
            mockSet.As<IQueryable<Language>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Language>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Language>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Language>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext = new Mock<IDbContext>();
            mockContext.Setup(c => c.Languages).Returns(mockSet.Object);

            languagesBL = new LanguagesBL(mockContext.Object);

        }
        [TestMethod]
        public void Test_GetLanguage_ById()
        {
            // act
            var language = languagesBL.GetLanguage(1);

            // assert
            Assert.AreEqual("BBB", language.Name);
        }
        [TestMethod]
        public void Test_GetLanguages_ProperCount()
        {
            // act
            var language = languagesBL.GetLanguages();

            // assert
            Assert.IsTrue(language.Count == 3);
        }
    }
}
