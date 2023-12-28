namespace UsersService.Exceptions
{
    public class InvalidAccesTokenException : Exception
    {
        public InvalidAccesTokenException(string message)
            : base(message) { }
    }
}
