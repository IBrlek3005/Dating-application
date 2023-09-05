namespace DatingAPI.Exceptions
{
    public class DatingException : Exception
    {
        public DatingException() { }
        public DatingException(string message) : base(message) { }
        public DatingException(string message,  Exception innerException) : base(message, innerException) { }
        public DatingException(string message, params object[] args) : base(string.Format(message, args)) { }
    }
}
