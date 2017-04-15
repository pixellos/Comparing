namespace DataContainersBenchmark
{
    public class SucceededResult : Result
    {
        public string Result { get; }
        public SucceededResult(string result)
        {
            this.Result = result;
        }
    }
}
