using InterviewTask.Logic.Validators;
using System;
using Xunit;

namespace InterviewTask.Logic.Tests
{
    public class LinkValidatorTests
    {
        private LinkValidator _linkValidator;

        public LinkValidatorTests()
        {
            _linkValidator = new LinkValidator();
        }

        [Fact]
        public void CheckLink_LinkEqualNull_ReturnErrorMessage()
        {
            //Arrange
            var expected = "Input value equal null";

            //Act
            var action = Assert.Throws<ArgumentException>(() => _linkValidator.CheckLink(null));

            //Assert
            Assert.Equal(expected, action.Message);
        }
    }
}