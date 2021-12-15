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
        private Mock<ParseDocumentSitemap> _mockParseDocumentSitemap;
        private Mock<LinkHandling> _mockLinkHandling;
        private SitemapCrawler _sitemapCrawler;

        [SetUp]
        public void SetUp()
        {
            _mockParseDocumentSitemap = new Mock<ParseDocumentSitemap>();
            _mockLinkHandling = new Mock<LinkHandling>(It.IsAny<HttpService>());
            _sitemapCrawler = new SitemapCrawler(_mockParseDocumentSitemap.Object, _mockLinkHandling.Object);
        }

        [Test]
        public void Parse_DocumentHaveLinks_ReturnListLinks()
        {
            //Arrange
            var expectedList = new List<Uri>() { new Uri("http://test.com"), new Uri("http://test.com/coffe") };
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            _mockLinkHandling.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);
            _mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualList = _sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public void Parse_DocumentDontHaveLinks_ReturnEmptyListLinks()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> </urlset>";
            _mockLinkHandling.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);
            _mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualList = _sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void Parse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _sitemapCrawler.Parse(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
