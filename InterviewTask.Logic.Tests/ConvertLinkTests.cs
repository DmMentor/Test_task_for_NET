using InterviewTask.Logic.Parser;
using NUnit.Framework;
using System;

namespace InterviewTask.Logic.Tests
{
    public class ConvertLinkTests
    {
        private ConvertLink convertLink;

        [SetUp]
        public void SetUp()
        {
            convertLink = new ConvertLink();
        }

        [Test]
        public void ConvertStringToUri_RelativeLink_ReturnAbsoluteLinks()
        {
            //Arrange
            var expectedLink = new Uri("https://example.com/coffebreak");
            var baseLink = new Uri("https://example.com");
            string testLink = "/coffebreak";

            //Act
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_OtherHost_ReturnsNull()
        {
            //Arrange
            Uri expectedLink = null;
            var baseLink = new Uri("https://example.com");
            string testLink = "https://new-example.com/coffebreak";

            //Act
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_NoLink_ReturnNull()
        {
            //Arrange
            Uri expectedLink = null;
            string testLink = "skype:myskype234";
            var baseLink = new Uri("https://example.com");

            //Act
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_AbsoluteLink_ReturnAbsoluteLink()
        {
            //Arrange
            var baseLink = new Uri("https://example.com");
            string testLink = "https://example.com/mytea/see";
            var expectedLink = new Uri("https://example.com/mytea/see");

            //Act
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }
    }
}