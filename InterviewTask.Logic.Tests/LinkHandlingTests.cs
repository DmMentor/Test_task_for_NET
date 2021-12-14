using InterviewTask.Logic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class LinkHandlingTests
    {
        private Mock<HttpService> _mockClass1;
        private LinkHandling _linkHandling;

        [SetUp]
        public void SetUp()
        {
            const int timeout = 1;
            _mockClass1 = new Mock<HttpService>(timeout);
            _linkHandling = new LinkHandling(_mockClass1.Object);
        }

        [Test]
        public void DownloadDocument_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _linkHandling.DownloadDocument(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void GetLinkResponse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _linkHandling.GetLinkResponse(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void GetLinkResponse_HttpResponseStatusCodeOk_ReturnResponseTime()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var returnsResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK };
            _mockClass1.Setup(m => m.GetResponseMessage(It.IsAny<Uri>())).Returns(returnsResponseMessage);

            //Act
            var actual = _linkHandling.GetLinkResponse(baseLink);

            //Assert
            Assert.IsTrue(actual >= 0);
        }

        [Test]
        public void GetLinkResponse_HttpResponseStatusCodeRequestTimeout_ReturnNegativeNumber()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var returnsResponseMessage = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.RequestTimeout };
            _mockClass1.Setup(m => m.GetResponseMessage(It.IsAny<Uri>())).Returns(returnsResponseMessage);

            //Act
            var actual = _linkHandling.GetLinkResponse(baseLink);

            //Assert
            Assert.IsTrue(actual == -1);
        }
    }
}
