namespace RMapper.Helpers;

using System.Linq.Expressions;
using System.Reflection;

/// <summary>
/// Helper para propiedades.
/// </summary>
public static class PropertyHelper
{
    /// <summary>
    /// Metodo para validar que la expresión sea una MemberExpression.
    /// </summary>
    /// <param name="propertyLambda">Expresión lambda.</param>
    /// <typeparam name="TDestination">Tipo de objeto destino.</typeparam>
    /// <typeparam name="TProperty">Tipo de la propiedad.</typeparam>
    /// <returns>Retorna un PropertyInfo del objeto de destino.</returns>
    /// <exception cref="ArgumentException">Excepción en caso de no entregar un MemberExpression.</exception>
    internal static PropertyInfo GetPropertyInfo<TDestination, TProperty>(
        Expression<Func<TDestination, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is not MemberExpression member)
            throw new ArgumentException("La expresión debe ser una propiedad", nameof(propertyLambda));

        if (member.Member is not PropertyInfo propInfo)
            throw new ArgumentException("El miembro no es una propiedad", nameof(propertyLambda));

        return propInfo;
    }
    
    /// <summary>
    /// Metodo que valída sí se solicitó una colección.
    /// </summary>
    /// <param name="type">Tipo solicitado.</param>
    /// <param name="entityCollectionType">Tipo generico (entidad) de la colección si aplica.</param>
    internal static bool TryGetEnumerableElementType(Type type, out Type entityCollectionType)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            entityCollectionType = type.GetGenericArguments()[0];
            return true;
        }

        var isSubEnumerable = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (isSubEnumerable is not null)
        {
            entityCollectionType = isSubEnumerable.GetGenericArguments()[0];
            return true;
        }

        entityCollectionType = null!;
        return false;
    }
    
    internal static bool IsSimpleType(Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        return type.IsPrimitive
               || type.IsEnum
               || type == typeof(string)
               || type == typeof(decimal)
               || type == typeof(DateTime)
               || type == typeof(DateTimeOffset)
               || type == typeof(TimeSpan)
               || type == typeof(DateOnly)
               || type == typeof(Guid);
    }
}