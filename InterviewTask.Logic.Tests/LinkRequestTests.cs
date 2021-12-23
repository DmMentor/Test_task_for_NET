using InterviewTask.LogicCrawler.Models;
using InterviewTask.LogicCrawler.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InterviewTask.LogicCrawler.Tests
{
    [TestFixture]
    internal class LinkRequestTests
    {
        private Mock<LinkHandling> _mockLinkHandling;
        private LinkRequest _linkRequest;

        [SetUp]
        public void Setup()
        {
            _mockLinkHandling = new Mock<LinkHandling>(It.IsAny<HttpService>());
            _linkRequest = new LinkRequest(_mockLinkHandling.Object);
        }

        [Test]
        public void GetListWithLinksResponseTime_ConvertListOtherType_ReturnsListLinkWithResponse()
        {
            //Arrange
            var expected = new List<LinkWithResponse>()
            {
               new LinkWithResponse() { Url = new Uri("https://test1.com/coffe"), ResponseTime = 0 },
               new LinkWithResponse() { Url = new Uri("https://test5.com/tea"), ResponseTime = 0 },
               new LinkWithResponse() { Url = new Uri("https://test2-beta.com/account/12345"), ResponseTime = 0 }
            };
            var inputList = new List<Link>()
            {
               new Link() { Url = new Uri("https://test1.com/coffe"), IsLinkFromHtml = true, IsLinkFromSitemap = false },
               new Link() { Url = new Uri("https://test5.com/tea"), IsLinkFromHtml = false, IsLinkFromSitemap = true },
               new Link() { Url = new Uri("https://test2-beta.com/account/12345"), IsLinkFromHtml = true, IsLinkFromSitemap = true }
            };
            _mockLinkHandling.Setup(m => m.GetLinkResponse(It.IsAny<Uri>())).Returns<HttpResponseMessage>(null);

            //Act
            var actualList = _linkRequest.GetListWithLinksResponseTime(inputList);

            //Assert
            Assert.AreEqual(expected.Select(_ => _.Url), actualList.Select(_ => _.Url));
        }

        [Test]
        public void ToLinkWithResponse_PassingNullIsParameters_ThrowException()
        {
            //Arrange
            string expectedMessage = "List is null (Parameter 'inputListLinks')";

            //Act
            var actualException = Assert.Throws<ArgumentNullException>(() => _linkRequest.GetListWithLinksResponseTime(null));

            //Assert
            Assert.AreEqual(expectedMessage, actualException.Message);
        }
    }
}
