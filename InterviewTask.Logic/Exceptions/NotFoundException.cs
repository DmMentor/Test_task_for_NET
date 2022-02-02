using System;

namespace InterviewTask.Logic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("No information about the test was found") { }
        public NotFoundException(string message) : base(message) { }
    }
}
