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
        private Mock<HtmlCrawler> mockHtmlCrawler;
        private Mock<SitemapCrawler> mockSitemapCrawler;
        private WebsiteCrawler websiteCrawler;

        [SetUp]
        public void SetUp()
        {
            mockHtmlCrawler = new Mock<HtmlCrawler>(It.IsAny<ParseDocumentHtml>(), It.IsAny<LinkHandling>(), It.IsAny<Converter>());
            mockSitemapCrawler = new Mock<SitemapCrawler>(It.IsAny<ParseDocumentSitemap>(), It.IsAny<LinkHandling>());

            websiteCrawler = new WebsiteCrawler(mockHtmlCrawler.Object, mockSitemapCrawler.Object);
        }

        [Test]
        public void Start_HtmlIsEmpty_ReturnsLinksOnlySitemap()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var expected = new List<Uri>()
            {
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };
            var listSitemap = new List<Uri>()
            {
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill")
            };
            mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());
            mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(listSitemap);

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected, actual.Select(_ => _.Url));
        }

        [Test]
        public void Start_SitemapIsEmpty_ReturnsLinksOnlyHtml()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var expected = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill")
            };
            var listHtml = new List<Uri>()
            {
                new Uri("https://test1.com"),
                new Uri("https://test1.com/chill")
            };
            mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(listHtml);
            mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());

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
            mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());
            mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(Enumerable.Empty<Uri>());

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
            var baseLink = new Uri("https://test1.com");
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
            mockHtmlCrawler.Setup(l => l.StartParse(It.IsAny<Uri>())).Returns(listHtml);
            mockSitemapCrawler.Setup(l => l.Parse(It.IsAny<Uri>())).Returns(listSitemap);

            //Act
            var actual = websiteCrawler.Start(baseLink);

            //Assert
            Assert.AreEqual(expected.Select(_ => _.Url), actual.Select(_ => _.Url));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromHtml), actual.Select(_ => _.IsLinkFromHtml));
            Assert.AreEqual(expected.Select(_ => _.IsLinkFromSitemap), actual.Select(_ => _.IsLinkFromSitemap));
        }
    }
}
