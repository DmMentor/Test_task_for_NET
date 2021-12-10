using InterviewTask.Logic.Parser;
using NUnit.Framework;
using System;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class DownloadDocumentTests
    {
        [Test]
        public void Download_UseDontAbsoluteLink_ThrowException()
        {
            //Arrange
            string expectedString = "Link must absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var downloadDocument = new DownloadDocument();
            TestDelegate actual = () => downloadDocument.Download(link);

            //Act
            var exception = Assert.Throws<ArgumentException>(actual);

            //Assert
            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
