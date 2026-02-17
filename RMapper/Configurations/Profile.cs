namespace RMapper.Configurations;

using Adapters;
using Builder;

/// <summary>
/// Profile para crear configuraciones.
/// </summary>
public abstract class Profile
{
    /// <summary>
    /// Adaptadores o configuraciones de entidades. (Source, Destination).
    /// </summary>
    internal static Dictionary<(Type, Type), TypeAdapterBuilder> TypeConfigsBuilders = new();
    
    /// <summary>
    /// Configuraciones de mapeos. (Source, Destination).
    /// </summary>
    internal static Dictionary<(Type, Type), TypeAdapterConfig> TypeConfigs = new();

    /// <summary>
    /// Metodo para crear una configuración de mapeo.
    /// </summary>
    /// <typeparam name="TSource">Tipo de entidad origen.</typeparam>
    /// <typeparam name="TDestination">Tipo de entidad destino.</typeparam>
    /// <returns>Retorna una instancia de la configuración.</returns>
    public TypeAdapterBuilder<TSource, TDestination> CreateMap<TSource, TDestination>()
        where  TSource : class
        where TDestination : class
    {
        var newConfig = new TypeAdapterBuilder<TSource, TDestination>();

        TypeConfigsBuilders.Add((typeof(TSource), typeof(TDestination)), newConfig);
        return newConfig;
    }

    /// <summary>
    /// Metodo que ejecuta los buildes construyendo las configuraciones.
    /// </summary>
    internal static void ExecuteBuilders()
    {
        foreach (var typeConfigsBuilder in TypeConfigsBuilders)
        {
            var configurationMapper = typeConfigsBuilder.Value.Build();
            TypeConfigs.Add(typeConfigsBuilder.Key, configurationMapper);
        }
    }
}