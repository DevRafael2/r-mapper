namespace RMapper.Exceptions;

/// <summary>
/// Excepción para casos donde no existe una configuración.
/// </summary>
/// <param name="message">Mensaje de la excepción.</param>
/// <param name="ex">Excepción.</param>
public class NotFoundMappingConfigException(string message, Exception? ex = null) : Exception(message, ex);