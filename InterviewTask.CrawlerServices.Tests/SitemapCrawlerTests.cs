using InterviewTask.CrawlerLogic.Crawlers;
using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Tests
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
            var converter = new Converter();
            _mockParseDocumentSitemap = new Mock<ParseDocumentSitemap>(converter);
            _mockLinkHandling = new Mock<LinkHandling>(It.IsAny<HttpService>());
            _sitemapCrawler = new SitemapCrawler(_mockParseDocumentSitemap.Object, _mockLinkHandling.Object);
        }

        [Test]
        public async Task ParseAsync_DocumentHaveLinks_ReturnListLinksAsync()
        {
            //Arrange
            var expectedList = new List<Uri>() { new Uri("http://test.com"), new Uri("http://test.com/coffe") };
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            _mockLinkHandling.Setup(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(document);
            _mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualList = await _sitemapCrawler.ParseAsync(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public async Task ParseAsync_DocumentDontHaveLinks_ReturnEmptyListLinksAsync()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> </urlset>";
            _mockLinkHandling.Setup(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(document);
            _mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualList = await _sitemapCrawler.ParseAsync(baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void ParseAsync_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.ThrowsAsync<ArgumentException>(() => _sitemapCrawler.ParseAsync(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
