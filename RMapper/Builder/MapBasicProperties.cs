using RMapper.Configurations;
using RMapper.Exceptions;

namespace RMapper.Builder;

using System.Linq.Expressions;
using System.Reflection;
using Helpers;

/// <summary>
/// Clase parcial con metodo de creación de mapeo de propiedades basicas.
/// </summary>
/// <typeparam name="TSource">Tipo de entidad de entrada.</typeparam>
/// <typeparam name="TDestination">Tipo de entidad de destino.</typeparam>
public partial class TypeAdapterBuilder<TSource, TDestination>
{
    /// <summary>
    /// Metodo que crea el listado de expresiones para propiedades basicas.
    /// </summary>
    private void MapBasicProperties()
    {
        ParameterExpression sourceParam = Expression.Parameter(typeof(TSource), "source");
        ParameterExpression destinationParam = Expression.Parameter(typeof(TDestination), "destination");
        
        var equalProperties =
            from d in typeof(TDestination).GetProperties()
            join s in typeof(TSource).GetProperties() on d.Name equals s.Name
            where d.PropertyType.IsAssignableFrom(Nullable.GetUnderlyingType(s.PropertyType) ?? s.PropertyType) && 
                  !_ignoreFields.Contains(s.Name) && 
                  !Enumerable.Select<KeyValuePair<(PropertyInfo, Func<TSource, object>), Action<object, TSource, PropertyInfo>>, string>(_config.CustomMapMembers, e => e.Key.Item1.Name).Contains(s.Name)
            select new { Source = s, Dest = d };
        
        foreach (var equalProperty in equalProperties)
        {
            MemberExpression sourceProperty = Expression.Property(sourceParam, equalProperty.Source);
            MemberExpression destinationProperty = Expression.Property(destinationParam, equalProperty.Dest);

            var isCollectionSource = PropertyHelper.TryGetEnumerableElementType(equalProperty.Source.PropertyType, out var sourceCollectionType);
            var isCollectionDest = PropertyHelper.TryGetEnumerableElementType(equalProperty.Dest.PropertyType,
                out var destinationCollectionType);
            if ((isCollectionSource && isCollectionDest) ||
                (PropertyHelper.IsSimpleType(equalProperty.Source.PropertyType) &&
                 PropertyHelper.IsSimpleType(equalProperty.Dest.PropertyType)) 
                )
            {
                if ((equalProperty.Source.PropertyType == equalProperty.Dest.PropertyType) ||
                    (sourceCollectionType is not null && destinationCollectionType is not null &&
                     (sourceCollectionType == destinationCollectionType)))
                {
                    BinaryExpression assign = Expression.Assign(destinationProperty, sourceProperty);
                    _assignments.Add(assign);
                }
                else
                {
                    var convertedSource = Expression.Convert(sourceProperty, equalProperty.Dest.PropertyType);
                    var throwExpression = Expression.Throw(Expression.Constant(
                        new ValueCannotBeAssignedException(
                            $"No se pudo asignar el valor de la propiedad de tipo {equalProperty.Source.PropertyType} a {equalProperty.Dest.PropertyType}. \n\n" +
                            $"Propiedad: {equalProperty.Source.Name} -> {equalProperty.Dest.Name} \n" +
                            $"Objeto: {typeof(TSource)} -> {typeof(TDestination)}"
                        )
                    ));
                    Expression.Empty();
                    BinaryExpression assign = Expression.Assign(destinationProperty, convertedSource);
                    var tryConverExpression = Expression
                        .TryCatch(
                            Expression.Block(typeof(void), convertedSource, assign),
                            Expression.Catch(Expression.Parameter(typeof(Exception), "ex"), MapperSettings.IgnoreBasicPropertiesFailedCast ? 
                                Expression.Default(typeof(void)) : 
                                throwExpression));
                    
                    _assignments.Add(tryConverExpression);
                }
                    
            }
        }
    
        BlockExpression block = Expression.Block((IEnumerable<Expression>)_assignments);
        var lambda = Expression.Lambda<Action<TSource, TDestination>>(block, sourceParam, destinationParam);
        _config.MapBasicProperties = lambda.Compile();
    }
}