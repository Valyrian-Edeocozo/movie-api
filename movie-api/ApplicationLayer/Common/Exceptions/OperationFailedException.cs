namespace movie_api.ApplicationLayer.Exceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {
            
        }
    }
}
