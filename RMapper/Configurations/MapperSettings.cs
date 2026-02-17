namespace RMapper.Configurations;

/// <summary>
/// Clase estatica para configurar opciones del mapper.
/// </summary>
internal static class MapperSettings
{
    /// <summary>
    /// Indica si deben ignorarse las propiedades simples cuando no pueden ser casteadas.
    /// </summary>
    internal static bool IgnoreBasicPropertiesFailedCast { get; set; } = false;
}