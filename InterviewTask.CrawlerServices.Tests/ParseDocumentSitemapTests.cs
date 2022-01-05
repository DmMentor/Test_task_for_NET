using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InterviewTask.CrawlerLogic.Tests
{
    [TestFixture]
    internal class ParseDocumentSitemapTests
    {
        private ParseDocumentSitemap _parserDocumentSitemap;

        [SetUp]
        public void SetUp()
        {
            var convert = new Converter();
            _parserDocumentSitemap = new ParseDocumentSitemap(convert);
        }

        [Test]
        public void ParseDocument_BlankDocument_ReturnEmptyList()
        {
            //Arrange
            var baseLink = new Uri("http://example.com");
            string document = string.Empty;

            //Act
            var actualList = _parserDocumentSitemap.ParseDocument(document, baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_DocumentNotHaveLinks_ReturnEmptyList()
        {
            //Arrange
            var baseLink = new Uri("http://www.w3.org");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"></urlset>";

            //Act
            var actualList = _parserDocumentSitemap.ParseDocument(document, baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_DocumentHaveSeveralLink_ReturnList()
        {
            //Arrange
            var baseLink = new Uri("https://test2.com");
            var expectedList = new List<Uri>() { new Uri("https://test2.com/tea/"), new Uri("https://test2.com/coffe/"), new Uri("https://test2.com/") };
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"https://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>https://test2.com/tea/</loc> </url> <url> <loc>https://test2.com/coffe/</loc> </url> " +
                "<url> <loc>https://test2.com/</loc> </url> </urlset>";

            //Act
            var actualList = _parserDocumentSitemap.ParseDocument(document, baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public void ParseDocument_PassingNullIsParameters_ReturnListLinks()
        {
            //Arrange
            string document = null;
            var baseLink = new Uri("http://example.com");

            //Act
            var actualList = _parserDocumentSitemap.ParseDocument(document, baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseDocument_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"></urlset>";
            var baseLink = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _parserDocumentSitemap.ParseDocument(document, baseLink));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
