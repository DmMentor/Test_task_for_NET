using InterviewTask.CrawlerLogic.Models;
using InterviewTask.EntityFramework.Entities;
using InterviewTask.Logic.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InterviewTask.Logic.Tests
{
    public class DatabaseOperationTests
    {
        private Mock<IRepository<Test>> _mockRepositoryTest;
        private Mock<IRepository<CrawlingResult>> _mockRepositoryCrawlingResult;
        private DatabaseOperation _databaseOperation;

        public DatabaseOperationTests()
        {
            _mockRepositoryTest = new Mock<IRepository<Test>>();
            _mockRepositoryCrawlingResult = new Mock<IRepository<CrawlingResult>>();
            _databaseOperation = new DatabaseOperation(_mockRepositoryTest.Object, _mockRepositoryCrawlingResult.Object);
        }

        [Fact]
        public async Task SaveToDatabaseAsync_VerificationDataEntryToDatabase_ShouldCallingOnceAsync()
        {
            //Arrange
            var link = new Uri("https://example.com");

            //Act
            await _databaseOperation.SaveToDatabaseAsync(link, new List<Link>(), new List<LinkWithResponse>());

            //Assert
            _mockRepositoryTest.Verify(v => v.AddAsync(It.Is<Test>(m => m.BaseUrl == link), It.IsAny<CancellationToken>()), Times.Once);
            _mockRepositoryTest.Verify(v => v.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SaveToDatabaseAsync_TransferOfTwoLists_ShouldUnionTwoListsByLinksAsync()
        {
            //Arrange
            const int expectedCount = 2;
            var link = new Uri("https://example.com");
            var listLink = new List<Link>()
            {
                new Link(){Url = new Uri("https://test1.com/chill")},
                new Link(){Url = new Uri("https://test1.com")},
            };
            var listLinkWithResponse = new List<LinkWithResponse>()
            {
                new LinkWithResponse(){Url = new Uri("https://test1.com/chill")},
                new LinkWithResponse(){Url = new Uri("https://test1.com")},
            };

            //Act
            await _databaseOperation.SaveToDatabaseAsync(link, listLink, listLinkWithResponse);

            //Assert
            _mockRepositoryTest.Verify(v => v.AddAsync(It.Is<Test>(t => t.Links.Count == expectedCount), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
