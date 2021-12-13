using InterviewTask.Logic.Models;
using InterviewTask.Logic.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Tests
{
    internal class ConverterTests
    {
        private Converter convertLink;

        [SetUp]
        public void SetUp()
        {
            convertLink = new Converter();
        }

        [Test]
        public void ToUri_RelativeLink_ReturnAbsoluteLinks()
        {
            //Arrange
            var expectedLink = new Uri("https://example.com/coffebreak");
            var baseLink = new Uri("https://example.com");
            string testLink = "/coffebreak";

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }

        [Test]
        public void ToUri_OtherHost_ReturnsNull()
        {
            //Arrange
            var baseLink = new Uri("https://example.com");
            string testLink = "https://new-example.com/coffebreak";

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.IsNull(actualLink);
        }

        [Test]
        public void ToUri_NoLink_ReturnNull()
        {
            //Arrange
            string testLink = "skype:myskype234";
            var baseLink = new Uri("https://example.com");

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.IsNull(actualLink);
        }

        [Test]
        public void ToUri_AbsoluteLink_ReturnAbsoluteLink()
        {
            //Arrange
            var baseLink = new Uri("https://example.com");
            string testLink = "https://example.com/mytea/see";
            var expectedLink = new Uri("https://example.com/mytea/see");

            //Act
            Uri actualLink = convertLink.ToUri(testLink, baseLink);

            //Assert
            Assert.AreEqual(expectedLink, actualLink);
        }
    }
}