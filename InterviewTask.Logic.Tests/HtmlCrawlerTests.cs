using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Parser;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;

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
            htmlCrawler = CreateHtmlCrawler(mockParseDocumentHtml.Object, mockDownloadDocument.Object, mockConvertLink.Object);
        }

        private HtmlCrawler CreateHtmlCrawler(ParseDocumentHtml parseDocumentHtml, DownloadDocument downloadDocument, ConvertLink convertLink)
        {
            return new HtmlCrawler(parseDocumentHtml, downloadDocument, convertLink);
        }

        [Test]
        public void Test()
        {
            IEnumerable<Uri> expectedQuery = new List<Uri>() { new Uri("https://test1.com/"), new Uri("https://test1.com/chill/arg/buysell"), new Uri("https://test1.com/chill") };
            string document = "<a href=\"https://test1.com/\" ></a> \n <a class=\"info\" href=\"https://test1.com/chill/arg/buysell\" >\n</a> \n\r <a href=\"https://test1.com/chill\" ></a> ";
            var baseLink = new Uri("https://test1.com");
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);

            var actualQuery = htmlCrawler.StartParse(baseLink);

            Assert.AreEqual(expectedQuery, actualQuery);
        }

        [Test]
        public void Test1()
        {
            IEnumerable<Uri> expectedQuery = new List<Uri>() { new Uri("https://test1.com/"), new Uri("https://test1.com/chill/arg/buysell"), new Uri("https://test1.com/chill") };
            var baseLink = new Uri("https://test1.com");
            string document = null;
            mockDownloadDocument.SetupSequence(d => d.Download(It.IsAny<Uri>(), It.IsAny<int>())).Returns(document);

            var actualQuery = htmlCrawler.StartParse(baseLink);

            Assert.AreEqual(expectedQuery, actualQuery);
        }
    }
}
