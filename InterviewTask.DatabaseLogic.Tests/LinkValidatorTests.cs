using InterviewTask.Logic.Services;
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
            var action = _linkValidator.CheckLink(null);

            //Assert
            Assert.False(action.IsValid);
            Assert.Equal(expected, action.Message);
        }
    }
}