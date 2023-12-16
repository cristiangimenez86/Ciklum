namespace Sportradar.Services.Exceptions
{
    public class ScoreBoardException : Exception
    {
        public ScoreBoardException() { }

        public ScoreBoardException(string message)
            : base(message) { }

        public ScoreBoardException(string message, Exception inner)
            : base(message, inner) { }
    }
}
