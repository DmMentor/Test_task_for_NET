using InterviewTask.Logic.Crawlers;
using InterviewTask.Logic.Models;
using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class WebsiteCrawlerTests
    {
        private Mock<LinkHandling> mockLinkHandlingHtml;
        private Mock<HtmlCrawler> mockHtmlCrawler;

        private Mock<LinkHandling> mockLinkHandlingSitemap;
        private Mock<SitemapCrawler> mockSitemapCrawler;

        private WebsiteCrawler websiteCrawler;

        [SetUp]
        public void SetUp()
        {
            var parseDocumentHtml = new ParseDocumentHtml();
            var convertLink = new Converter();
            mockLinkHandlingHtml = new Mock<LinkHandling>(It.IsAny<int>());
            mockHtmlCrawler = new Mock<HtmlCrawler>(parseDocumentHtml, mockLinkHandlingHtml.Object, convertLink);

            var parseDocumentSitemap = new ParseDocumentSitemap();
            mockLinkHandlingSitemap = new Mock<LinkHandling>(It.IsAny<int>());
            mockSitemapCrawler = new Mock<SitemapCrawler>(parseDocumentSitemap, mockLinkHandlingSitemap.Object);

            websiteCrawler = new WebsiteCrawler(mockHtmlCrawler.Object, mockSitemapCrawler.Object);
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
            mockLinkHandlingHtml.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>())).Returns(documentHtml);
            mockHtmlCrawler.CallBase = true;
            string documentSitemap = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"> " +
                "<url> <loc>https://test1.com/chill</loc> </url> <url> <loc>https://test.com/coffe</loc> </url> </urlset>";
            mockLinkHandlingSitemap.Setup(d => d.DownloadDocument(It.IsAny<Uri>())).Returns(documentSitemap);
            mockSitemapCrawler.CallBase = true;

            //Act
            var actual = websiteCrawler.Start(baseLink).ToList();

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Url));
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
            mockLinkHandlingHtml.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);
            mockHtmlCrawler.CallBase = true;
            string documentSitemap = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml\"> " +
                "<url> <loc>https://test1.com/chill</loc> </url> <url> " +
                "<loc>https://test.com/coffe</loc> </url> </urlset>";
            mockLinkHandlingSitemap.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(documentSitemap);
            mockSitemapCrawler.CallBase = true;

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Url));
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
            mockLinkHandlingHtml.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(documentHtml);
            mockHtmlCrawler.CallBase = true;
            mockLinkHandlingSitemap.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);
            mockSitemapCrawler.CallBase = true;

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Url));
        }

        [Test]
        public void Start_DocumentsEmpty_ReturnEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockLinkHandlingHtml.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);
            mockHtmlCrawler.CallBase = true;
            mockLinkHandlingSitemap.Setup(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);
            mockSitemapCrawler.CallBase = true;

            //Act
            var actual = websiteCrawler.Start(baseLink).Where(_ => _.Url != baseLink);

            //Assert
            Assert.IsEmpty(actual);
        }

        [Test]
        public void Start_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var downloadDocument = new LinkHandling();
            
            //Act
            var actualException = Assert.Throws<ArgumentException>(() => websiteCrawler.Start(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void ConcatLists_CombiningListsWithLinks_ReturnsListAllLinks()
        {
            //Arrange
            var expected = new List<Link>()
            {
                new Link(){Url = new Uri("https://test1.com/chill"), IsLinkFromHtml = true, IsLinkFromSitemap = true},
                new Link(){Url = new Uri("https://test1.com"), IsLinkFromHtml = true, IsLinkFromSitemap = false},
                new Link(){Url = new Uri("https://test1.com/chill/arg/buysell"), IsLinkFromHtml = false, IsLinkFromSitemap = true}
            };
            var listHtml = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill")
            };
            var listSitemap = new List<Uri>()
            {
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };

            //Act
            var actual = websiteCrawler.ConcatLists(listHtml, listSitemap);

            //Assert
            Assert.AreEqual(expected.Select(_ => _.Url), actual.Select(_ => _.Url));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromHtml), actual.Select(_ => _.IsLinkFromHtml));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromSitemap), actual.Select(_ => _.IsLinkFromSitemap));
        }
    }
}
