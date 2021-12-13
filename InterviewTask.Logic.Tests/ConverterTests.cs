using InterviewTask.Logic.Models;
using InterviewTask.Logic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    internal class ConverterTests
    {
        private Converter convertLink;

        [SetUp]
        public void SetUp()
        {
            convertLink = new Converter();
        }

        [Test]
        public void ToUri_RelativeLink_ReturnAbsoluteLinks()
        {
            //Arrange
            var expectedLink = new Uri("https://example.com/coffebreak");
            var baseLink = new Uri("https://example.com");
            string testLink = "/coffebreak";

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ToUri_OtherHost_ReturnsNull()
        {
            //Arrange
            var baseLink = new Uri("https://example.com");
            string testLink = "https://new-example.com/coffebreak";

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.IsNull(actualLink);
        }

        [Test]
        public void ToUri_NoLink_ReturnNull()
        {
            //Arrange
            string testLink = "skype:myskype234";
            var baseLink = new Uri("https://example.com");

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.IsNull(actualLink);
        }

        [Test]
        public void ToUri_AbsoluteLink_ReturnAbsoluteLink()
        {
            //Arrange
            var baseLink = new Uri("https://example.com");
            string testLink = "https://example.com/mytea/see";
            var expectedLink = new Uri("https://example.com/mytea/see");

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ToLinkWithResponse_ConvertListOtherType_ReturnsLinkWithResponse()
        {
            //Arrange
            var expected = new List<LinkWithResponse>()
            {
               new LinkWithResponse() { Url = new Uri("https://test1.com/coffe"), IsLinkFromHtml = true, IsLinkFromSitemap = false, ResponseTime = 0 },
               new LinkWithResponse() { Url = new Uri("https://test5.com/tea"), IsLinkFromHtml = false, IsLinkFromSitemap = true, ResponseTime = 0 },
               new LinkWithResponse() { Url = new Uri("https://test2-beta.com/account/12345"), IsLinkFromHtml = true, IsLinkFromSitemap = true, ResponseTime = 0 }
            };
            var inputList = new List<Link>()
            {
               new Link() { Url = new Uri("https://test1.com/coffe"), IsLinkFromHtml = true, IsLinkFromSitemap = false },
               new Link() { Url = new Uri("https://test5.com/tea"), IsLinkFromHtml = false, IsLinkFromSitemap = true },
               new Link() { Url = new Uri("https://test2-beta.com/account/12345"), IsLinkFromHtml = true, IsLinkFromSitemap = true }
            };

            //Act
            var actual = convertLink.ToLinkWithResponse(inputList);

            //Assert
            Assert.AreEqual(expected.Select(_ => _.Url), actual.Select(_ => _.Url));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromHtml), actual.Select(_ => _.IsLinkFromHtml));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromSitemap), actual.Select(_ => _.IsLinkFromSitemap));
        }

        [Test]
        public void ToLinkWithResponse_PassingNullIsParameters_ThrowException()
        {
            //Arrange
            string expectedMessage = "List is null (Parameter 'inputList')";

            //Act
            var actualException = Assert.Throws<ArgumentNullException>(() => convertLink.ToLinkWithResponse(null));

            //Assert
            Assert.AreEqual(expectedMessage, actualException.Message);
        }
    }
}