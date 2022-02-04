using InterviewTask.CrawlerLogic.Services;
using InterviewTask.Logic.Exceptions;
using InterviewTask.Logic.Validators;
using Moq;
using System;
using Xunit;

namespace InterviewTask.Logic.Tests
{
    public class LinkValidatorTests
    {
        private LinkValidator _linkValidator;
        private Mock<HttpService> _mockHttpService;

        public LinkValidatorTests()
        {
            _mockHttpService = new Mock<HttpService>();
            _linkValidator = new LinkValidator(_mockHttpService.Object);
        }

        [Fact]
        public void CheckLink_LinkEqualNull_ReturnErrorMessage()
        {
            //Arrange
            var expected = "Link is invalid";

            //Act
            var action = Assert.Throws<InputLinkInvalidException>(() => _linkValidator.CheckLinkAsync(null).Wait());

            //Assert
            Assert.Equal(expected, action.Message);
        }
    }
}