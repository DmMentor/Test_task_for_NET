using InterviewTask.Logic.Crawlers;
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
    internal class HtmlCrawlerTests
    {
        private Mock<ParseDocumentHtml> mockParseDocumentHtml;
        private Mock<LinkHandling> mockLinkHandling;
        private Mock<Converter> mockConverter;
        private HtmlCrawler htmlCrawler;

        [SetUp]
        public void SetUp()
        {
            mockParseDocumentHtml = new Mock<ParseDocumentHtml>();
            mockLinkHandling = new Mock<LinkHandling>(It.IsAny<int>());
            mockConverter = new Mock<Converter>();
            htmlCrawler = new HtmlCrawler(mockParseDocumentHtml.Object, mockLinkHandling.Object, mockConverter.Object);
        }

        [Test]
        public void StartParse_AnyLinks_ReturnsForrmattedLinks()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            IEnumerable<Uri> expected = new List<Uri>()
            {
                new Uri("https://test1.com/"),
                new Uri("https://test1.com/chill/arg/buysell"),
                new Uri("https://test1.com/chill"),
                new Uri("https://test1.com/trainee.work/")
            };
            var documentFirst = "<a href=\"https://test1.com/\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a>" +
                " \n\r <a href=\"https://test1.com/chill\" ></a> ";
            var documentSecond = "<a href=\"/trainee.work/\" ></a> \n <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(documentFirst)
                .Returns(documentSecond);
            mockParseDocumentHtml.CallBase = true;
            mockConverter.CallBase = true;

            //Act
            var actual = htmlCrawler.StartParse(baseLink);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StartParse_LinksAnotherHost_ReturnsEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var document = "<a href=\"https://test2.com/\" ></a> \r <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(document);

            //Act
            var actual = htmlCrawler.StartParse(baseLink).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actual);
        }

        [Test]
        public void StartParse_CallMethodParceDocumentNever_ReturnsEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>()))
                .Returns(Enumerable.Empty<string>());

            //Act
            var actual = htmlCrawler.StartParse(baseLink);

            //Assert
            mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void StartParse_CallMethodParceDocumentOnce_ReturnsEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockLinkHandling.Setup(d => d.DownloadDocument(It.IsAny<Uri>())).Returns(" ");
            mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>()))
                .Returns(Enumerable.Empty<string>());

            //Act
            var actual = htmlCrawler.StartParse(baseLink);

            //Assert
            mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void StartParse_CallMethodDownloadOnce_ReturnsForrmattedLinks()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);

            //Act
            var actual = htmlCrawler.StartParse(baseLink);

            //Assert
            mockLinkHandling.Verify(d => d.DownloadDocument(It.IsAny<Uri>()), Times.Once());
        }


        [Test]
        public void ParseDocument_BlankDocument_ReturnEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockLinkHandling.SetupSequence(d => d.DownloadDocument(It.IsAny<Uri>()))
                .Returns(string.Empty);

            //Act
            var actualList = htmlCrawler.StartParse(baseLink).Where(_ => _ != baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }

        [Test]
        public void StartParse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => htmlCrawler.StartParse(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void ConvertDocumentToLinks_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => htmlCrawler.ParseDocumentToLinks(string.Empty, link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void ConvertDocumentToLinks_BlankDocument_ReturnEmpty()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");

            //Act
            var actualList = htmlCrawler.ParseDocumentToLinks(string.Empty, baseLink);

            //Assert
            Assert.IsEmpty(actualList);
        }
    }
}
