using InterviewTask.Logic.Parser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class ParseDocumentSitemapTests
    {
        private ParseDocumentSitemap parserDocumentSitemap;

        [SetUp]
        public void SetUp()
        {
            parserDocumentSitemap = new ParseDocumentSitemap();
        }

        [Test]
        public void ParseDocument_ParsingSingleLink_ReturnListWithOnceLink()
        {
            //Arrange
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> <url> <loc>http://test.com</loc> </url> </urlset>";
            var expectedLink = new Uri("http://test.com/");

            //Act
            var actualList = parserDocumentSitemap.ParseDocument(document).ToList();

            //Assert
            Assert.AreEqual(expectedLink, actualList[0]);
        }

        [Test]
        public void ParseDocument_DocumentNotExist_ReturnNull()
        {
            //Act
            IEnumerable<Uri> actualList = parserDocumentSitemap.ParseDocument(null);

            //Assert
            Assert.IsNull(actualList);
        }

        [Test]
        public void ParseDocument_ParsingDocumentWithoutLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"></urlset>";

            //Act
            var actualLink = parserDocumentSitemap.ParseDocument(document).ToList();

            //Assert
            Assert.Zero(actualLink.Count);
        }

        [Test]
        public void ParseDocument_ParsingSeveralLink_ReturnList()
        {
            //Arrange
            var expectedLinks = new List<Uri>(3) {  new Uri("https://test1.com/tea/"), new Uri("https://test2.com/coffe/"), new Uri("https://test3.com/") };
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"https://www.w3.org/1999/xhtml/\"> <url> <loc>https://test1.com/tea/</loc> </url> <url> <loc>https://test2.com/coffe/</loc> </url> <url> <loc>https://test3.com/</loc> </url> </urlset>";

            //Act
            var actualLinks = parserDocumentSitemap.ParseDocument(document).ToList();

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }
    }
}
