namespace WebTimetableApi.Handlers.Exceptions
{
    public class OutagesNotLoadedException : Exception
    {
        private readonly string _outagesSource;

        public OutagesNotLoadedException(string message, string outagesSource) : base(message)
        {
            _outagesSource = outagesSource;
        }

        public OutagesNotLoadedException(Exception innerExpection, string outagesSource, string message) : base(message, innerExpection)
        {
            _outagesSource = outagesSource;
        }

        public override string ToString()
        {
            return "Error occured during retrieving outages. Outages source: " + _outagesSource + "\n" + base.ToString();
        }
    }
}