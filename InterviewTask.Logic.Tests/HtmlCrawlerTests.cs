using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Parser;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class HtmlCrawlerTests
    {
        private Mock<ParseDocumentHtml> mockParseDocumentHtml;
        private Mock<DownloadDocument> mockDownloadDocument;
        private Mock<ConvertLink> mockConvertLink;
        private HtmlCrawler htmlCrawler;

        [SetUp]
        public void SetUp()
        {
            mockParseDocumentHtml = new Mock<ParseDocumentHtml>();
            mockDownloadDocument = new Mock<DownloadDocument>();
            mockConvertLink = new Mock<ConvertLink>();
            htmlCrawler = new HtmlCrawler(mockParseDocumentHtml.Object, mockDownloadDocument.Object, mockConvertLink.Object);
        }

        [Test]
        public void StartParse_DocumentHaveNeedLinks_ReturnsForrmattedLinks()
        {
            //Arrange
            IEnumerable<Uri> expectedQuery = new List<Uri>() { new Uri("https://test1.com/"), new Uri("https://test1.com/chill/arg/buysell"), new Uri("https://test1.com/chill"), new Uri("https://test1.com/trainee.work/") };
            var documentFirst = "<a href=\"https://test1.com/\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a> \n\r <a href=\"https://test1.com/chill\" ></a> ";
            var documentSecond = "<a href=\"/trainee.work/\" ></a> \n <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(documentFirst).Returns(documentSecond);
            mockParseDocumentHtml.CallBase = true;
            mockConvertLink.CallBase = true;
            var baseLink = new Uri("https://test1.com");

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink);

            //Assert
            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void StartParse_DocumentWithoutNeedLinks_ReturnsEmptyQuery()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            var document = "<a href=\"https://test2.com/\" ></a> \r <a href=\"skype:gg.com@group\" class=\"offens.cs\"></a>";
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink).ToList();

            //Assert
            Assert.IsTrue(actualQuery.Count() == 1);
            Assert.AreEqual(baseLink, actualQuery.First().AbsoluteUri);
        }

        [Test]
        public void StartParse_CallMethodPrceDocumentNever_ReturnsEmptyQuery()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>())).Returns(Enumerable.Empty<string>());

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink);

            //Assert
            mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void StartParse_CallMethodParceDocumentOnce_ReturnsEmptyQuery()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocument.Setup(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(" ");
            mockParseDocumentHtml.Setup(s => s.ParseDocument(It.IsAny<string>())).Returns(Enumerable.Empty<string>());

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink);

            //Assert
            mockParseDocumentHtml.Verify(p => p.ParseDocument(It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void StartParse_CallMethodConvertStringSeveralTimes_ReturnsForrmattedLinks()
        {
            //Arrange
            var document = "<a href=\"https://test1.com/\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a> \n\r <a href=\"https://test1.com/chill\" ></a> ";
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);
            mockConvertLink.CallBase = true;
            mockParseDocumentHtml.CallBase = true;

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink);

            //Assert
            mockConvertLink.Verify(c => c.ConvertStringToUri(It.IsAny<string>(), It.IsAny<Uri>()), Times.Exactly(3));
        }

        [Test]
        public void StartParse_CallMethodDownloadOnce_ReturnsForrmattedLinks()
        {
            //Arrange
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(string.Empty);

            //Act
            var actualQuery = htmlCrawler.StartParse(baseLink);

            //Assert
            mockDownloadDocument.Verify(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void StartParse_UseDontAbsolutekLink_ThrowException()
        {
            //Arrange
            string expectedString = "Link must absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            TestDelegate actual = () => htmlCrawler.StartParse(link);

            //Act
            var exception = Assert.Throws<ArgumentException>(actual);

            //Assert
            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
