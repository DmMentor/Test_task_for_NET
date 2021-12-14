using InterviewTask.Logic.Crawlers;
using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class SitemapCrawlerTests
    {
        private Mock<ParseDocumentSitemap> mockParseDocumentSitemap;
        private Mock<LinkHandling> mockLinkHandling;
        private SitemapCrawler sitemapCrawler;

        [SetUp]
        public void SetUp()
        {
            mockParseDocumentSitemap = new Mock<ParseDocumentSitemap>();
            mockLinkHandling = new Mock<LinkHandling>(It.IsAny<int>());
            sitemapCrawler = new SitemapCrawler(mockParseDocumentSitemap.Object, mockLinkHandling.Object);
        }

        [Test]
        public void Parse_DocumentHaveLinks_ReturnListLinks()
        {
            //Arrange
            IEnumerable<Uri> expected = new List<Uri>() { new Uri("http://test.com"), new Uri("http://test.com/coffe") };
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockLinkHandling.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actual = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Parse_DocumentDontHaveLinks_ReturnEmptyListLinks()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> </urlset>";
            mockLinkHandling.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualResult = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void Parse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);

            //Act
            var exception = Assert.Throws<ArgumentException>(() => sitemapCrawler.Parse(link));

            //Assert
            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
