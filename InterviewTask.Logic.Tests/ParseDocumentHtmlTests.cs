﻿using InterviewTask.Logic.Parsers;
using NUnit.Framework;
using System.Collections.Generic;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class ParseDocumentHtmlTests
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
            var expectedLinks = new string[] { "https://test.com/#test-one/" };
            string document = "<a href=\"https://test.com/#test-one/\">";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }

        [Test]
        public void ParseDocument_gDocumentNotHaveLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<a class=\"test.cs\">";

            //Act
            var actualLinks = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualLinks);
        }

        [Test]
        public void ParseDocument_DocumentHaveSeveralLink_ReturnList()
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
        public void ParseDocument_BlankDocument_ReturnEmpty()
        {
            //Arrange
            string document = string.Empty;

            //Act
            IEnumerable<string> actualList = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_PassingNullIsParameters_ReturnEmpty()
        {
            //Arrange
            string document = null;

            //Act
            IEnumerable<string> actualList = parseDocumenthtml.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualList);
        }
    }
}
