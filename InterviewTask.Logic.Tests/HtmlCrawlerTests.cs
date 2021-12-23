using InterviewTask.LogicCrawler.Crawlers;
using InterviewTask.LogicCrawler.Parsers;
using InterviewTask.LogicCrawler.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.LogicCrawler.Tests
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
        public void StartParse_AnyLinks_ReturnsForrmattedLinks()
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
            _mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(documentFirst)
                .Returns(documentSecond);
            _mockParseDocumentHtml.CallBase = true;
            _mockConverter.CallBase = true;

            //Act
            var actualList = _htmlCrawler.StartParse(baseLink);

            //Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public void StartParse_LinksAnotherHost_ReturnsEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var document = "<a href=\"https://test2.com/\" ></a> \r <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            _mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);

            //Act
            var actualList = _htmlCrawler.StartParse(baseLink).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void StartParse_CallMethodParceDocumentNever_ReturnsEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>()))
                .Returns(Enumerable.Empty<string>());

            //Act
            var actualList = _htmlCrawler.StartParse(baseLink);

            //Assert
            _mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void StartParse_CallMethodDownloadOnce_ReturnsForrmatedLinks()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);

            //Act
            _htmlCrawler.StartParse(baseLink);

            //Assert
            _mockLinkHandling.Verify(d => d.DownloadDocument(It.IsAny<Uri>()), Times.Once());
        }


        [Test]
        public void ParseDocument_BlankDocument_ReturnEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            _mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);

            //Act
            var actualList = _htmlCrawler.StartParse(baseLink).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void StartParse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("example-1.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => _htmlCrawler.StartParse(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
