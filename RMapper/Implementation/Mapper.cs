namespace RMapper.Implementation;

using Adapters;
using Configurations;
using Exceptions;
using Helpers;
using Interfaces;

/// <summary>
/// Clase mapper encargada de ejecutar las configuraciones de mapeo.
/// </summary>
public class Mapper : IMapper
{
    /// <summary>
    /// Metodo para mapear de una entidad a otra entidad.
    /// </summary>
    /// <param name="source">Entidad o datos de origen.</param>
    /// <typeparam name="TSource">Tipo de la entidad de origen.</typeparam>
    /// <typeparam name="TDestination">Tipo de la entidad destino.</typeparam>
    /// <returns>Retorna una nueva instancia de la entidad destino.</returns>
    public TDestination? Map<TSource, TDestination>(TSource source)
        where TDestination : class, new() => CustomMap<TSource, TDestination>(source);
    
    private void MapComplexProperties<TSource, TDestination>(
        TSource source,
        TDestination destination,
        TypeAdapterConfig<TSource, TDestination> config) {
        var complexProperties =
            from d in typeof(TDestination).GetProperties()
            join s in typeof(TSource).GetProperties() on d.Name equals s.Name
            where !d.PropertyType.IsAssignableFrom(Nullable.GetUnderlyingType(s.PropertyType) ?? s.PropertyType) && 
                  !config.IgnoreProperties.Contains(s.Name) && 
                  !config.CustomMapMembers.Select(e => e.Key.Item1.Name).Contains(s.Name)
            select new { Source = s, Dest = d };
        
        foreach (var complexProperty in complexProperties)
        {
            if (PropertyHelper.TryGetEnumerableElementType(complexProperty.Source.PropertyType,
                    out var enumerableElementTypeSource) && PropertyHelper.TryGetEnumerableElementType(complexProperty.Dest.PropertyType,
                    out var enumerableElementTypeDest))
            {
                var methodList = typeof(Mapper)
                    .GetMethod(nameof(MapCollection))!
                    .MakeGenericMethod(enumerableElementTypeSource, enumerableElementTypeDest);

                var valueSourceList = typeof(TSource).GetProperty(complexProperty.Source.Name)!.GetValue(source);
                var responseList = methodList.Invoke(this, [valueSourceList]);
                typeof(TDestination).GetProperty(complexProperty.Dest.Name)!.SetValue(destination, responseList);
                continue;
            }

            var method = typeof(Mapper).GetMethod(nameof(Map))!
                .MakeGenericMethod(complexProperty.Source.PropertyType, complexProperty.Dest.PropertyType);

            var valueSourceComplex = typeof(TSource).GetProperty(complexProperty.Source.Name)!.GetValue(source);
            var response = method.Invoke(this, [valueSourceComplex]);
            typeof(TDestination).GetProperty(complexProperty.Dest.Name)!.SetValue(destination, response);
        }
    }

    public List<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource>? source)
        where TDestination : class, new()
    {
        if (source is null)
            return [];
        var enumerable = source.ToList();
        return enumerable.ToList().Count == 0 ? [] 
            : enumerable.Select(e => Map<TSource, TDestination>(e)!).ToList();
    }
    
    private object MapCollection(object source, Type sourceItemType, Type destItemType)
    {
        var enumerable = (System.Collections.IEnumerable)source;

        var listType = typeof(List<>).MakeGenericType(destItemType);
        var list = (System.Collections.IList)Activator.CreateInstance(listType)!;
        
        var mapMethod = typeof(Mapper) 
            .GetMethod(nameof(Map), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)!;

        var genericMapMethod = mapMethod.MakeGenericMethod(sourceItemType, destItemType);

        foreach (var item in enumerable)
        {
            var mappedItem = genericMapMethod.Invoke(this, new[] { item });
            list.Add(mappedItem);
        }
        return list;
    }

    private TDestination? CustomMap<TSource, TDestination>(TSource source, TDestination? destination = null) 
        where TDestination : class
    {
        destination ??= Activator.CreateInstance<TDestination>();
        
        if (source is null)
            return null;
        
        if (PropertyHelper.TryGetEnumerableElementType(typeof(TSource), out var sourceItemType) &&
            PropertyHelper.TryGetEnumerableElementType(typeof(TDestination), out var destItemType))
        {
            return (TDestination)MapCollection(source, sourceItemType, destItemType);
        }
        var config = Profile.TypeConfigs.FirstOrDefault(e => e.Key == (typeof(TSource), typeof(TDestination))).Value 
            as TypeAdapterConfig<TSource, TDestination>;
        
        if(config is null)
            throw new NotFoundMappingConfigException(
                $"No se encontró una configuración de mapeo para {typeof(TSource)} -> {typeof(TDestination)}. \n\n" +
                $"Si el mapeo corresponde a una propiedad puede solucionar el problema con las siguientes opciones: \n\n" +
                $"1. Agregando el CreateMap<{typeof(TSource)}, {typeof(TDestination)}> a uno de sus Profile's \n" +
                $"2. Usando el ForMember(e => e.MiPropertyObject, e => (...value))" +
                $"3. ignorando la propiedad desde el Ignore(e => e.MiPropertyObject))");
        
        var allCustomConfigs = config.CustomMapMembers;

        MapComplexProperties(source, destination, config);
        config.MapBasicProperties!(source, destination);

        if (allCustomConfigs.Count == 0) return destination;
        
        foreach (var customMap in allCustomConfigs)
            customMap.Value(destination, source, customMap.Key.Item1);

        return destination;
    }
    
    public TDestination? Adapt<TSource, TDestination>(TSource source, TDestination destination)
        where TDestination : class,new() => CustomMap(source, destination);
}