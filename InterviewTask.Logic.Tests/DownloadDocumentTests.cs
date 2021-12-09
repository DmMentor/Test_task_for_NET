using InterviewTask.Logic.Parser;
using NUnit.Framework;
using System;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    class DownloadDocumentTests
    {
        [Test]
        public void Download_UseDontWorkLink_ThrowException()
        {
            string expectedString = "Link must absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var downloadDocument = new DownloadDocument();

            TestDelegate actual = () => downloadDocument.Download(link);
            var exception = Assert.Throws<ArgumentException>(actual);

            Assert.AreEqual(expectedString, exception.Message);
        }
    }
}
