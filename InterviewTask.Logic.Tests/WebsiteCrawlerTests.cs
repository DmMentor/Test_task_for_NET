using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Parser;
using InterviewTask.Logic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class WebsiteCrawlerTests
    {
        private Mock<DownloadDocument> mockDownloadDocumentHtml;
        private Mock<HtmlCrawler> mockHtmlCrawler;

        private Mock<DownloadDocument> mockDownloadDocumentSitemap;
        private Mock<SitemapCrawler> mockSitemapCrawler;

        private Mock<LinkRequest> mockLinkRequest;
        private WebsiteCrawler websiteCrawler;

        [SetUp]
        public void SetUp()
        {
            var parseDocumentHtml = new ParseDocumentHtml();
            var convertLink = new ConvertLink();
            mockDownloadDocumentHtml = new Mock<DownloadDocument>();
            mockHtmlCrawler = new Mock<HtmlCrawler>(parseDocumentHtml, mockDownloadDocumentHtml.Object, convertLink);

            var parseDocumentSitemap = new ParseDocumentSitemap();
            mockDownloadDocumentSitemap = new Mock<DownloadDocument>();
            mockSitemapCrawler = new Mock<SitemapCrawler>(parseDocumentSitemap, mockDownloadDocumentSitemap.Object);

            mockLinkRequest = new Mock<LinkRequest>();
            websiteCrawler = new WebsiteCrawler(mockHtmlCrawler.Object, mockSitemapCrawler.Object, mockLinkRequest.Object);
        }

        [Test]
        public void Start_DocumentsHaveLinks_ReturnsAllLinks()
        {
            //Arrange
            var expected = new List<Uri>()
            {
                new Uri("https://test1.com/chill"),
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test.com/coffe")
            };
            var baseLink = new Uri("https://test1.com");
            var documentHtml = "<a href=\"https://test1.com\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a> " +
                "\n\r <a href=\"https://test1.com/chill\" ></a> ";
            mockDownloadDocumentHtml.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(documentHtml);
            mockHtmlCrawler.CallBase = true;
            string documentSitemap = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"> " +
                "<url> <loc>https://test1.com/chill</loc> </url> <url> <loc>https://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocumentSitemap.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(documentSitemap);
            mockSitemapCrawler.CallBase = true;
            mockLinkRequest.Setup(l => l.LinkResponseTime(It.IsAny<Uri>(), It.IsAny<int>())).Returns(0);

            //Act
            var actual = websiteCrawler.Start(baseLink).ToList();

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Link));
        }

        [Test] 
        public void Start_HtmlIsEmpty_ReturnsLinksOnlySitemap()
        {
            //Arrange
            var expected = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill"),
                new Uri("https://test.com/coffe")
            };
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocumentHtml.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(string.Empty);
            mockHtmlCrawler.CallBase = true;
            string documentSitemap = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"> " +
                "<url> <loc>https://test1.com/chill</loc> </url> <url> " +
                "<loc>https://test.com/coffe</loc> </url> </urlset>";
            mockDownloadDocumentSitemap.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(documentSitemap);
            mockSitemapCrawler.CallBase = true;
            mockLinkRequest.Setup(l => l.LinkResponseTime(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(0);

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Link));
        }

        [Test]
        public void Start_SitemapIsEmpty_ReturnsLinksOnlyHtml()
        {
            //Arrange
            var expected = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };
            var baseLink = new Uri("https://test1.com");
            var documentHtml = "<a href=\"https://test1.com/chill/arg/buysell\" ></a> \n <a class=\"info\"  >\n</a> \n\r <a href=\"https://test1.com/chill\" ></a> ";
            mockDownloadDocumentHtml.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(documentHtml);
            mockHtmlCrawler.CallBase = true;
            mockDownloadDocumentSitemap.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(string.Empty);
            mockSitemapCrawler.CallBase = true;
            mockLinkRequest.Setup(l => l.LinkResponseTime(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(0);

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Link));
        }

        [Test]
        public void Start_DocumentsEmpty_ReturnsOnlyBaseLink()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocumentHtml.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(string.Empty);
            mockHtmlCrawler.CallBase = true;
            mockDownloadDocumentSitemap.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(string.Empty);
            mockSitemapCrawler.CallBase = true;
            mockLinkRequest.Setup(l => l.LinkResponseTime(It.IsAny<Uri>(), It.IsAny<int>()))
                .Returns(0);

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.IsTrue(actual.Count() == 1);
            Assert.AreEqual(baseLink, actual.First().Link);
        }

        [Test]
        public void Start_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var downloadDocument = new DownloadDocument();
            TestDelegate actual = () => websiteCrawler.Start(link);

            //Act
            var exception = Assert.Throws<ArgumentException>(actual);

            //Assert
            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
