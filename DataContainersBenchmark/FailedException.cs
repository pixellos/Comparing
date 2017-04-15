using System;

namespace DataContainersBenchmark
{
    public class FailedException : Exception
    {
        public string FailureReason { get; }
        public FailedException(string failureReason)
        {
            this.FailureReason = failureReason;
        }
    }
}
