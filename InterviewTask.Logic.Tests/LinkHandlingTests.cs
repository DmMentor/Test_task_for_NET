using InterviewTask.Logic.Services;
using NUnit.Framework;
using System;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class LinkHandlingTests
    {
        [Test]
        public void DownloadDocument_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var linkHandling = new LinkHandling();

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => linkHandling.DownloadDocument(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }

        [Test]
        public void GetLinkResponse_LinkIsNotAbsolute_ThrowException()
        {
            //Arrange
            string expectedString = "Link must be absolute";
            var link = new Uri("ukad-group.com", UriKind.Relative);
            var linkHandling = new LinkHandling();

            //Act
            var actualException = Assert.Throws<ArgumentException>(() => linkHandling.GetLinkResponse(link));

            //Assert
            Assert.AreEqual(expectedString, actualException.Message);
        }
    }
}
