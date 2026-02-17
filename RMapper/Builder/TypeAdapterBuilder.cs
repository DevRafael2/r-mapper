namespace RMapper.Builder;

using System.Linq.Expressions;
using Adapters;

/// <summary>
/// Clase principal del TypeAdapterBuilder para construir configuraciones de mapeo.
/// </summary>
/// <typeparam name="TSource">Tipo de entidad de origen.</typeparam>
/// <typeparam name="TDestination">Tipo de entidad de destino.</typeparam>
public partial class TypeAdapterBuilder<TSource, TDestination> : TypeAdapterBuilder
{
    /// <summary>
    /// Configuración por instancia.
    /// </summary>
    private readonly TypeAdapterConfig<TSource, TDestination> _config = new();
    
    /// <summary>
    /// Lista de expresiones de asignación básica.
    /// </summary>
    private readonly List<Expression> _assignments = new();

    /// <summary>
    /// Propiedades ignoradas.
    /// </summary>
    private readonly HashSet<string> _ignoreFields = new();
    
    /// <summary>
    /// Metodo que entrega el objeto creado.
    /// </summary>
    /// <returns>Retorna objeto creado.</returns>
    public override TypeAdapterConfig<TSource, TDestination> Build()
    {
        _config.IgnoreProperties = _ignoreFields;
        MapBasicProperties();
        return _config;
    }
}

/// <summary>
/// Builder abstracrt.
/// </summary>
public abstract class TypeAdapterBuilder
{
    public abstract TypeAdapterConfig Build();
}