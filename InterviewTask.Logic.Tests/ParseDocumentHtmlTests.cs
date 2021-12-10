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
            string expectedLink = "https://test.com/#test-one/";
            string document = "<a href=\"https://test.com/#test-one/\">";

            //Act
            var actualLink = parseDocumenthtml.ParseDocument(document).ToList();

            //Assert
            Assert.AreEqual(expectedLink, actualLink[0]);
        }

        [Test]
        public void ParseDocument_ParsingDocumentWithoutLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<a class=\"test.cs\">";

            //Act
            var actualLink = parseDocumenthtml.ParseDocument(document).ToList();

            //Assert
            Assert.Zero(actualLink.Count);
        }

        [Test]
        public void ParseDocument_ParsingSeveralLink_ReturnList()
        {
            //Arrange
            var expectedLinks = new List<string>(2) { "/test.com/#test-one/", "https://test.com/#test-two/" };
            string document = "<a class=\"test.cs\" href=\"/test.com/#test-one/\"> </p></div></article><article class=\"blog - card blog - card--small blog-card--short\"> \n <a class=\"test.cs\" href=\"https://test.com/#test-two/\"> ";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document).ToList();

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }

        [Test]
        public void ParseDocument_DocumentNotExist_ReturnNull()
        {
            //Act
            IEnumerable<string> actualList = parseDocumenthtml.ParseDocument(null);

            //Assert
            Assert.IsNull(actualList);
        }
    }
}
