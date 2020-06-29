using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using UnitTests.AsyncSetups;

namespace UnitTests
{
    [TestClass]
    public class UnitTests_WordsBL
    {
        Mock<IDbContext> mockContext;
        WordsBL wordsBL;
        public UnitTests_WordsBL()
        {
            mockContext = MockSetup.GetMock();

            wordsBL = new WordsBL(mockContext.Object);
        }
        [TestMethod]
        public async Task GetWords_ReturnsProperCount()
        {
            // act
            var temp = await wordsBL.GetWordsAsync();
            // assert
            Assert.IsTrue(temp.Count == 4);
        }
        [TestMethod]
        public async Task GetWords_ByLanguage()
        {
            // act
            var temp = await wordsBL.GetLanguageWordsAsync(1);
            // assert
            Assert.IsTrue(temp.Count == 3);
        }
        [TestMethod]
        public async Task GetWord_ById()
        {
            // act
            var temp = await wordsBL.GetWordByIdAsync(1);
            // assert
            Assert.AreEqual("Hello", temp.Value);
        }

        [TestMethod]
        public async Task AddWord_PassNew_AddsAndReturnsBack()
        {
            // arrange 
            var temp = new Word();
            temp.Id = 10;
            temp.LanguageId = 1;
            temp.WordId = 12;
            temp.Value = "Word";
            // act
            var result = await wordsBL.AddWordAsync(temp);
            // assert
            Assert.AreEqual(temp.Value, result.Value);
        }
        
        [TestMethod]
        public async Task AddWord_PassExisting_ReturnsBack()
        {
            // arrange 
            var temp = new Word();
            temp.Id = 10;
            temp.LanguageId = 1;
            temp.WordId = 1;
            temp.Value = "Word";
            // act
            var result = await wordsBL.AddWordAsync(temp);
            // assert
            Assert.AreNotEqual(temp.Value, result.Value);
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [TestMethod]
        public async Task DeleteWord_PassExistingId_ReturnsTrue()
        {
            // act
            var temp = await wordsBL.DeleteAsync(1);
            // assert
            Assert.IsTrue(temp);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public async Task DeleteWord_PassId_ReturnsFalse()
        {
            // act
            var temp = await wordsBL.DeleteAsync(10);
            // assert
            Assert.IsFalse(temp);
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [TestMethod]
        public async Task Update_PassWord_ReturnsItUpdated()
        {
            // arrange 
            var temp = new Word();
            temp.Id = 1;
            temp.Value = "New";
            // act
            var result = await wordsBL.UpdateAsync(temp);
            // assert
            Assert.AreEqual(temp.Value, result.Value);
        }

        [TestMethod]
        public async Task SwitchLanguage_PassId_ReturnsSwitched()
        {
            // act
            var temp = await wordsBL.SwitchLanguageAsync(1, 2);
            // assert
            Assert.IsTrue(temp.LanguageId == 2);
        }
    }
}
