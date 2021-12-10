using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Parser;
using Moq;
using NUnit.Framework;
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
        public void Parse_DocumentHaveLinks_ReturnsLinks()
        {
            //Arrange
            IEnumerable<Uri> expected = new List<Uri>() { new Uri("http://test.com"), new Uri("http://test.com/coffe") };
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actual = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Parse_DocumentDontHaveLinks_ReturnEmpty()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            var actualResult = sitemapCrawler.Parse(baseLink);

            //Assert
            Assert.IsEmpty(actualResult);
        }

        [Test]
        public void Parse_CallMethodDocumentOnce_ReturnsLinks()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> <url> <loc>http://test.com</loc> </url> <url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            sitemapCrawler.Parse(baseLink);

            //Assert
            mockDownloadDocument.Verify(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void Parse_CallMethodParceDocumentOnce_ReturnsLinks()
        {
            //Arrange
            var baseLink = new Uri("http://test.com");
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>http://test.com</loc> </url> " +
                "<url> <loc>http://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(document);
            mockParseDocumentSitemap.CallBase = true;

            //Act
            sitemapCrawler.Parse(baseLink);

            //Assert
            mockParseDocumentSitemap.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Once());
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
