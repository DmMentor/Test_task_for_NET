using InterviewTask.Logic.Parser;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class ParseDocumentHtmlTests
    {
        private ParseDocumentHtml parseDocumenthtml;

        [SetUp]
        public void SetUp()
        {
            parseDocumenthtml = new ParseDocumentHtml();
        }

        [Test]
        public void ParseDocument_ParsingSingleLink_ReturnListWithOnceLink()
        {
            //Arrange
            string[] expectedLinks = new[] { "https://test.com/#test-one/" };
            string document = "<a href=\"https://test.com/#test-one/\">";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }

        [Test]
        public void ParseDocument_ParsingDocumentWithoutLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<a class=\"test.cs\">";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualLinks);
        }

        [Test]
        public void ParseDocument_ParsingSeveralLink_ReturnList()
        {
            //Arrange
            var expectedLinks = new List<string>(2) { "/test.com/#test-one/", "https://test.com/#test-two/" };
            string document = "<a class=\"test.cs\" href=\"/test.com/#test-one/\"> </p><" +
                "/div></article><article class=\"blog - card blog - card--small blog-card--short\"> \n " +
                "<a class=\"test.cs\" href=\"https://test.com/#test-two/\"> ";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }

        [Test]
        public void ParseDocument_DocumentEmpty_ReturnEmpty()
        {
            //Act
            IEnumerable<string> actualList = parseDocumenthtml.ParseDocument(string.Empty);

            //Assert
            Assert.IsEmpty(actualList);
        }
    }
}
