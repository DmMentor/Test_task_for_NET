using InterviewTask.Logic.Models;
using System;
using System.Net;

namespace InterviewTask.Logic.Services
{
    public class LinkValidator
    {
        public TestResult CheckLink(Uri link)
        {
            var testResult = new TestResult
            {
                Message = "Parsing completed successfully"
            };

            if (link == null)
            {
                testResult.Message = "Input value equal null";
                testResult.IsValid = false;
            }
            else
            {
                try
                {
                    WebRequest.Create(link).GetResponse().Close();
                }
                catch
                {
                    testResult.Message = "Link dont work";
                    testResult.IsValid = false;
                }
            }

            return testResult;
        }
    }
}
