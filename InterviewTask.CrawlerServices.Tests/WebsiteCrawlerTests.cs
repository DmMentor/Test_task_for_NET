using InterviewTask.CrawlerServices.Crawlers;
using InterviewTask.CrawlerServices.Models;
using InterviewTask.CrawlerServices.Parsers;
using InterviewTask.CrawlerServices.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.CrawlerServices.Tests
{
    [TestFixture]
    internal class WebsiteCrawlerTests
    {
        private Mock<HtmlCrawler> _mockHtmlCrawler;
        private Mock<SitemapCrawler> _mockSitemapCrawler;
        private WebsiteCrawler _websiteCrawler;

        [SetUp]
        public void SetUp()
        {
            _mockHtmlCrawler = new Mock<HtmlCrawler>(It.IsAny<ParseDocumentHtml>(), It.IsAny<LinkHandling>(), It.IsAny<Converter>());
            _mockSitemapCrawler = new Mock<SitemapCrawler>(It.IsAny<ParseDocumentSitemap>(), It.IsAny<LinkHandling>());

            _websiteCrawler = new WebsiteCrawler(_mockHtmlCrawler.Object, _mockSitemapCrawler.Object);
        }

        [Test]
        public void Start_HtmlIsEmpty_ReturnsLinksOnlySitemap()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var expectedList = new List<Uri>()
            {
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };
            var listSitemap = new List<Uri>()
            {
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };
            _mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());
            _mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(listSitemap);

            //Act
            var actualList = _websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList.Select(_ => _.Url));
        }

        [Test]
        public void Start_SitemapIsEmpty_ReturnsListWithLinksOnlyHtml()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var expectedList = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill")
            };
            var listHtml = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill")
            };
            _mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(listHtml);
            _mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());

            //Act
            var actualList = _websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList.Select(_ => _.Url));
        }

        [Test]
        public void Start_DocumentsEmpty_ReturnEmptyList()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());
            _mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());

            //Act
            var actualList = _websiteCrawler.Start(baseLink).Where(_ => _.Url != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void Start_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _websiteCrawler.Start(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void ConcatLists_CombiningListsWithLinks_ReturnsListAllLinks()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var expectedList = new List<Link>()
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
            _mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(listHtml);
            _mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(listSitemap);

            //Act
            var actualList = _websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expectedList.Select(_ => _.Url), actualList.Select(_ => _.Url));
            Assert.AreEqual(expectedList.Select(_ => _.IsLinkFromHtml), actualList.Select(_ => _.IsLinkFromHtml));
            Assert.AreEqual(expectedList.Select(_ => _.IsLinkFromSitemap), actualList.Select(_ => _.IsLinkFromSitemap));
        }
    }
}
