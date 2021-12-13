using InterviewTask.Logic.Models;
using InterviewTask.Logic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class LinkRequestTests
    {
        private Mock<LinkHandling> mockLinkHandling;
        private LinkRequest linkRequest;

        [SetUp]
        public void Setup()
        {
            mockLinkHandling = new Mock<LinkHandling>(10000);
            linkRequest = new LinkRequest(mockLinkHandling.Object);
        }

        [Test]
        public void GetListWithLinksResponseTime_ConvertListOtherType_ReturnsLinkWithResponse()
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
            mockLinkHandling.Setup(m => m.GetLinkResponse(It.IsAny<Uri>())).Returns<HttpResponseMessage>(null);

            //Act
            var actual = linkRequest.GetListWithLinksResponseTime(inputList);

            //Assert
            Assert.AreEqual(expected.Select(_ => _.Url), actual.Select(_ => _.Url));
        }

        [Test]
        public void ToLinkWithResponse_PassingNullIsParameters_ThrowException()
        {
            //Arrange
            string expectedMessage = "List is null (Parameter 'inputList')";

            //Act
            var actualException = Assert.Throws<ArgumentNullException>(() => linkRequest.GetListWithLinksResponseTime(null));

            //Assert
            Assert.AreEqual(expectedMessage, actualException.Message);
        }
    }
}
