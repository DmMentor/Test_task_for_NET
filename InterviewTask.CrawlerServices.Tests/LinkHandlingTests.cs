using InterviewTask.CrawlerLogic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace InterviewTask.CrawlerLogic.Tests
{
    [TestFixture]
    internal class LinkHandlingTests
    {
        private Mock<HttpService> _mockHttpService;
        private LinkHandling _linkHandling;

        [SetUp]
        public void SetUp()
        {
            const int timeout = 1;
            _mockHttpService = new Mock<HttpService>(timeout);
            _linkHandling = new LinkHandling(_mockHttpService.Object);
        }

        [Test]
        public void DownloadDocumentAsync_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.ThrowsAsync<ArgumentException>(() => _linkHandling.DownloadDocumentAsync(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void GetLinkResponseAsync_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-2.com", UriKind.Relative);

            //Act
            var actualException = Assert.ThrowsAsync<ArgumentException>(() => _linkHandling.GetLinkResponseAsync(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public async Task GetLinkResponseAsync_HttpResponseStatusCodeOk_ReturnResponseTimeAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var returnsResponseMessage = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
            _mockHttpService.Setup(m => m.GetResponseMessageAsync(It.IsAny<Uri>()))
                .ReturnsAsync(returnsResponseMessage);

            //Act
            var actualResponseTime = await _linkHandling.GetLinkResponseAsync(baseLink);

            //Assert
            Assert.IsTrue(actualResponseTime >= 0);
        }

        [Test]
        public async Task GetLinkResponseAsync_HttpResponseStatusCodeRequestTimeout_ReturnNegativeNumberAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var returnsResponseMessage = new HttpResponseMessage() { StatusCode = HttpStatusCode.RequestTimeout };
            _mockHttpService.Setup(m => m.GetResponseMessageAsync(It.IsAny<Uri>()))
                .ReturnsAsync(returnsResponseMessage);

            //Act
            var actualResponseTime = await _linkHandling.GetLinkResponseAsync(baseLink);

            //Assert
            Assert.IsTrue(actualResponseTime == int.MaxValue);
        }
    }
}
