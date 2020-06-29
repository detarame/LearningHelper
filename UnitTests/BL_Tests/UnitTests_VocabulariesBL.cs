using BusinessLayer;
using DataLayer;
using DataLayer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnitTests.AsyncSetups;

namespace UnitTests
{
    [TestClass]
    public class UnitTests_VocabulariesBL
    {
        Mock<IDbContext> mockContext;
        VocabulariesBL vocabBL;
        public UnitTests_VocabulariesBL()
        {
            mockContext = MockSetup.GetMock();

            vocabBL = new VocabulariesBL(mockContext.Object);
        }
        [TestMethod]
        public async Task GetVocabularies_All()
        {
            //act 
            var temp = await vocabBL.GetVocabulariesAsync();
            // assert
            Assert.AreEqual(4, temp.Count);
        }
        
        [TestMethod]
        public async Task GetVocabulary_PassId2_ReturnsBack()
        {
            short expectedId = 2;
            //act 
            var temp = await vocabBL.GetVocabularyAsync(expectedId);
            // assert
            Assert.AreEqual(expectedId, temp.Id);
        }
       
        [TestMethod]
        public async Task GetVocabularyWords_PassId2_ReturnsAllWords()
        {
            //act 
            var temp = await vocabBL.GetVocabularyWordsAsync(2);
            // assert
            Assert.IsTrue(temp.All(w => w.Id == 1 || w.Id == 2));
            Assert.AreEqual(2, temp.Count);
        }

        [TestMethod]
        public void AddVocabulary_PassVocab_ReturnsBack()
        {
            // arrange
            var temp = new Vocabulary();
            temp.Id = 1;
            temp.LanguageId = 3;
            // act
            var result = vocabBL.AddVocabulary(temp);
            // assert
            Assert.AreEqual(result.Id, temp.Id);
            Assert.AreEqual(result.LanguageId, temp.LanguageId);
            Assert.IsNotNull(result);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
       
        [TestMethod]
        public async Task AddWord_PassExistingWord_ReturnsFalse()
        {
            // act
            var temp = await vocabBL.AddWordAsync(1, 1);
            // assert
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
            Assert.IsFalse(temp);
        }
        
        [TestMethod]
        public async Task Delete_ByID_ReturnsFalse()
        {
            // act
            var temp = await vocabBL.DeleteAsync(10);
            // assert
            Assert.IsFalse(temp);
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }
        [TestMethod]
        public async Task DeleteWordFromVoc_PassIds_ReturnsTrue()
        {
            // act
            var temp = await vocabBL.DeleteFromVocAsync(1, 1);
            // assert
            Assert.IsTrue(temp);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [TestMethod]
        public async Task UpdateVocab_PassNew_ProperReturn()
        {
            // arrange
            var temp = new Vocabulary();
            temp.Id = 1;
            temp.Name = "Changed";
            // act
            var result = await vocabBL.UpdateAsync(temp);
            // assert
            Assert.AreEqual(temp.Name, result.Name);
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
