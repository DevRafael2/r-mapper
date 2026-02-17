namespace RMapper.Adapters;

using System.Reflection;

/// <summary>
/// Implementación de configuración de adaptador.
/// </summary>
/// <typeparam name="TSource">Tipo del objeto origen.</typeparam>
/// <typeparam name="TDestination">Tipo del objeto destino.</typeparam>
public sealed class TypeAdapterConfig<TSource, TDestination> : TypeAdapterConfig
{
    /// <summary>
    /// Diccionario con expresiones complejas.
    /// </summary>
    internal Dictionary<
        (PropertyInfo, Func<TSource, object>), Action<object, TSource, PropertyInfo>> CustomMapMembers { get; set; } = new();

    /// <summary>
    /// Nombres de propiedades ignoradas.
    /// </summary>
    internal HashSet<string> IgnoreProperties { get; set; } = new();

    /// <summary>
    /// Metodo para agregar una configuración personalizada de mapeo de propiedad.
    /// </summary>
    /// <param name="property">Tipo de la propiedad.</param>
    /// <param name="func">Delegado que entrega el valor de la propiedad.</param>
    /// <param name="action">Función que obtienen la instancia y asigna el valor.</param>
    public void AddCustomMapMember(PropertyInfo property, Func<TSource, object> func, Action<object, TSource, PropertyInfo> action) =>
        CustomMapMembers.Add((property, func), action);

    /// <summary>
    /// Expresiones para propiedades sin configuraciones.
    /// </summary>
    public Action<TSource, TDestination>? MapBasicProperties { get; set; }
    
}

/// <summary>
/// Clase abstracta de configuración de adaptador.
/// </summary>
public abstract class TypeAdapterConfig;

