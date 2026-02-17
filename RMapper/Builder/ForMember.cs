namespace RMapper.Builder;

using System.Linq.Expressions;
using System.Reflection;
using Helpers;

public partial class TypeAdapterBuilder<TSource, TDestination>
{
    /// <summary>
    /// Metodo para mapear propiedades de manera personalizada.
    /// </summary>
    /// <param name="member">MemberExpression.</param>
    /// <param name="mapValueMember">Delegado para setear el valor.</param>
    /// <typeparam name="TSource">Tipo de entidad origen.</typeparam>
    /// <typeparam name="TDestination">Tipo de entidad destino.</typeparam>
    /// <typeparam name="TProperty">Tipo de propiedad a mapear.</typeparam>
    public TypeAdapterBuilder<TSource, TDestination> ForMember<TProperty>(
        Expression<Func<TDestination, TProperty?>> member,
        Func<TSource, object> mapValueMember)
    {
        var propertyDestination = PropertyHelper.GetPropertyInfo<TDestination, TProperty>(member!);

        void ActionSetter(object entity, TSource source, PropertyInfo property)
        {
            var value = mapValueMember(source);
            property.SetValue(entity, value);
        }

        _config.AddCustomMapMember(propertyDestination, mapValueMember, ActionSetter);
        return this;
    }
}