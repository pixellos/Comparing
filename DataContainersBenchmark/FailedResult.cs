namespace DataContainersBenchmark
{
    public partial class Program
    {
        public class FailedResult : Result
        {
            public string FailureReason { get; }
            public FailedResult(string message)
            {
                this.FailureReason = message;
            }
        }
    }
}
