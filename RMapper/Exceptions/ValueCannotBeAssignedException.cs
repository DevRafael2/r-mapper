namespace RMapper.Exceptions;

/// <summary>
/// Excepción para casos donde no es posible mapear una propiedad a otra.
/// </summary>
/// <param name="message">Mensaje de excepción.</param>
public class ValueCannotBeAssignedException(string message) : Exception(message);