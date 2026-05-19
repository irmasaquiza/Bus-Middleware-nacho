namespace Middleware.Vuelos.Business.Exceptions;

public class UnauthorizedBusinessException : Exception
{
    public UnauthorizedBusinessException(string message) : base(message) { }
}