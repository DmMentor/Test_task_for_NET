using InterviewTask.CrawlerLogic.Crawlers;
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
    internal class HtmlCrawlerTests
    {
        private Mock<ParseDocumentHtml> _mockParseDocumentHtml;
        private Mock<LinkHandling> _mockLinkHandling;
        private Mock<Converter> _mockConverter;
        private HtmlCrawler _htmlCrawler;

        [SetUp]
        public void SetUp()
        {
            _mockParseDocumentHtml = new Mock<ParseDocumentHtml>();
            _mockLinkHandling = new Mock<LinkHandling>(It.IsAny<HttpService>());
            _mockConverter = new Mock<Converter>();
            _htmlCrawler = new HtmlCrawler(_mockParseDocumentHtml.Object, _mockLinkHandling.Object, _mockConverter.Object);
        }

        [Test]
        public async Task StartParseAsync_AnyLinks_ReturnsForrmattedLinksAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            IEnumerable<Uri> expectedList = new List<Uri>()
            {
                new Uri("https://test1.com/"),
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill"),
                new Uri("https://test1.com/trainee.work/")
            };
            var documentFirst = "<a href=\"https://test1.com/\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a>" +
                " \n\r <a href=\"https://test1.com/chill\" ></a> ";
            var documentSecond = "<a href=\"/trainee.work/\" ></a> \n <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            _mockLinkHandling.SetupSequence(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(documentFirst)
                .ReturnsAsync(documentSecond);
            _mockParseDocumentHtml.CallBase = true;
            _mockConverter.CallBase = true;

            //Act
            var actualList = await _htmlCrawler.StartParseAsync(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public async Task StartParseAsync_LinksAnotherHost_ReturnsEmptyAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var document = "<a href=\"https://test2.com/\" ></a> \r <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            _mockLinkHandling.SetupSequence(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(document);

            //Act
            var actualList = (await _htmlCrawler.StartParseAsync(baseLink)).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public async Task StartParseAsync_CallMethodParceDocumentNever_ReturnsEmptyAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>()))
                .Returns(Enumerable.Empty<string>());

            //Act
            var actualList = await _htmlCrawler.StartParseAsync(baseLink);

            //Assert
            _mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public async Task StartParseAsync_CallMethodDownloadOnce_ReturnsForrmatedLinksAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockLinkHandling.SetupSequence(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(string.Empty);

            //Act
            await _htmlCrawler.StartParseAsync(baseLink);

            //Assert
            _mockLinkHandling.Verify(d => d.DownloadDocumentAsync(It.IsAny<Uri>()), Times.Once());
        }


        [Test]
        public async Task StartParseAsync_BlankDocument_ReturnEmptyAsync()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockLinkHandling.SetupSequence(d => d.DownloadDocumentAsync(It.IsAny<Uri>()))
                .ReturnsAsync(string.Empty);

            //Act
            var actualList = (await _htmlCrawler.StartParseAsync(baseLink)).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void StartParseAsync_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.ThrowsAsync<ArgumentException>(() => _htmlCrawler.StartParseAsync(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
