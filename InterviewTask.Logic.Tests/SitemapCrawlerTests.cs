using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Parser;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class SitemapCrawlerTests
    {
        private Mock<ParseDocumentSitemap> mockParseDocumentSitemap;
        private Mock<DownloadDocument> mockDownloadDocument;
        private SitemapCrawler sitemapCrawler;

        [SetUp]
        public void SetUp()
        {
            mockParseDocumentSitemap = new Mock<ParseDocumentSitemap>();
            mockDownloadDocument = new Mock<DownloadDocument>();
            sitemapCrawler = new SitemapCrawler(mockParseDocumentSitemap.Object, mockDownloadDocument.Object);
        }

        [Test]
        public void Parse_DocumentHaveLinks_ReturnsLinksInQuery()
        {
            //Arrange
            IEnumerable<Uri> expectedQuery = new List<Uri>() { new Uri("http://test.com"), new Uri("http://test.com/coffe") };
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> <url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualQuery = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void Parse_DocumentDontHaveLinks_ReturnEmptyQuery()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualQuery = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.IsEmpty(actualQuery);
        }

        [Test]
        public void Parse_CallMethodDocumentOnce_ReturnsLinksInQuery()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> <url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualQuery = sitemapCrawler.Parse(baseLink);

            //Assert
            mockDownloadDocument.Verify(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Parse_CallMethodParceDocumentOnce_ReturnsLinksInQuery()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> <url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualQuery = sitemapCrawler.Parse(baseLink);

            //Assert
            mockParseDocumentSitemap.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void Parse_UseDontAbsoluteLink_ThrowException()
        {
            //Arrange
            string expectedString = "Link must absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            TestDelegate actual = () => sitemapCrawler.Parse(link);

            //Act
            var exception = Assert.Throws<ArgumentException>(actual);

            //Assert
            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
