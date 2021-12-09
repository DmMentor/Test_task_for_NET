using NUnit.Framework;
using System;
using InterviewTask.Logic.Parser;

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
        public void ConvertStringToUri_RelativeLink_Return()
        {
            var baseLink = new Uri("https://example.com");

            string testLink = "/coffebreak";
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            var expectedLink = new Uri("https://example.com/coffebreak");

            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_OtherHost_ReturnsNull()
        {
            var baseLink = new Uri("https://example.com");

            string testLink = "https://new-example.com/coffebreak";
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            Uri expectedLink = null;

            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_NoLink_ReturnNull()
        {
            var baseLink = new Uri("https://example.com");

            string testLink = "skype:myskype234";
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            Uri expectedLink = null;

            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ConvertStringToUri_AbsoluteLink_Return()
        {
            var baseLink = new Uri("https://example.com");

            string testLink = "https://example.com/mytea/see";
            Uri actualLink = convertLink.ConvertStringToUri(testLink, baseLink);

            var expectedLink = new Uri("https://example.com/mytea/see");

            Assert.AreEqual(expectedLink, actualLink);
        }
    }
}