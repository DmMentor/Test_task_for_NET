using InterviewTask.CrawlerLogic.Crawlers;
using InterviewTask.CrawlerLogic.Models;
using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Tests
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
        public async Task StartAsync_HtmlIsEmpty_ReturnsLinksOnlySitemapAsync()
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
            _mockHtmlCrawler.Setup(l => l.StartParseAsync(It.IsAny<Uri>())).ReturnsAsync(Enumerable.Empty<Uri>());
            _mockSitemapCrawler.Setup(l => l.ParseAsync(It.IsAny<Uri>())).ReturnsAsync(listSitemap);

            //Act
            var actualList = await _websiteCrawler.StartAsync(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList.Select(_ => _.Url));
        }

        [Test]
        public async Task StartAsync_SitemapIsEmpty_ReturnsListWithLinksOnlyHtmlAsync()
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
            _mockHtmlCrawler.Setup(l => l.StartParseAsync(It.IsAny<Uri>())).ReturnsAsync(listHtml);
            _mockSitemapCrawler.Setup(l => l.ParseAsync(It.IsAny<Uri>())).ReturnsAsync(Enumerable.Empty<Uri>());

            //Act
            var actualList = await _websiteCrawler.StartAsync(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList.Select(_ => _.Url));
        }

        [Test]
        public async Task StartAsync_DocumentsEmpty_ReturnEmptyListAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockHtmlCrawler.Setup(l => l.StartParseAsync(It.IsAny<Uri>())).ReturnsAsync(Enumerable.Empty<Uri>());
            _mockSitemapCrawler.Setup(l => l.ParseAsync(It.IsAny<Uri>())).ReturnsAsync(Enumerable.Empty<Uri>());

            //Act
            var actualList = (await _websiteCrawler.StartAsync(baseLink)).Where(_ => _.Url != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void StartAsync_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.ThrowsAsync<ArgumentException>(() => _websiteCrawler.StartAsync(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public async Task StartAsync_CombiningListsWithLinks_ReturnsListAllLinksAsync()
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
            _mockHtmlCrawler.Setup(l => l.StartParseAsync(It.IsAny<Uri>())).ReturnsAsync(listHtml);
            _mockSitemapCrawler.Setup(l => l.ParseAsync(It.IsAny<Uri>())).ReturnsAsync(listSitemap);

            //Act
            var actualList = await _websiteCrawler.StartAsync(baseLink);

            //Assert
            Assert.AreEqual(expectedList.Select(_ => _.Url), actualList.Select(_ => _.Url));
            Assert.AreEqual(expectedList.Select(_ => _.IsLinkFromHtml), actualList.Select(_ => _.IsLinkFromHtml));
            Assert.AreEqual(expectedList.Select(_ => _.IsLinkFromSitemap), actualList.Select(_ => _.IsLinkFromSitemap));
        }
    }
}
