using System;

namespace InterviewTask.Logic.Exceptions
{
    public class InputLinkInvalidException : Exception
    {
        public InputLinkInvalidException() : base("Link is invalid") { }
        public InputLinkInvalidException(string message) : base(message) { }
    }
}
