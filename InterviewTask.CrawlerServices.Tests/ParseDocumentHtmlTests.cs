using InterviewTask.CrawlerLogic.Parsers;
using NUnit.Framework;
using System.Collections.Generic;

namespace InterviewTask.CrawlerLogic.Tests
{
    [TestFixture]
    internal class ParseDocumentHtmlTests
    {
        private ParseDocumentHtml _parseDocumenthtml;

        [SetUp]
        public void SetUp()
        {
            _parseDocumenthtml = new ParseDocumentHtml();
        }

        [Test]
        public void ParseDocument_DocumentNotHaveLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<a class=\"test.cs\">";

            //Act
            var actualLinks = _parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualLinks);
        }

        [Test]
        public void ParseDocument_BlankDocument_ReturnEmpty()
        {
            //Arrange
            string document = string.Empty;

            //Act
            var actualList = _parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_PassingNullIsParameters_ReturnEmpty()
        {
            //Arrange
            string document = null;

            //Act
            var actualList = _parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_DocumentHaveSeveralLink_ReturnList()
        {
            //Arrange
            var expectedLinks = new List<string>() { "/test.com/#test-one/", "https://test.com/#test-two/" };
            string document = "<a class=\"test.cs\" href=\"/test.com/#test-one/\"> </p><" +
                "/div></article><article class=\"blog - card blog - card--small blog-card--short\"> \n " +
                "<a class=\"test.cs\" href=\"https://test.com/#test-two/\"> ";

            //Act
            var actualLinks = _parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }
    }
}
