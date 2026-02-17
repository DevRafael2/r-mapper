namespace RMapper.Builder;

using System.Linq.Expressions;
using RMapper.Helpers;

public partial class TypeAdapterBuilder<TSource, TDestination>
{
    /// <summary>
    /// Metodo estatico para ignorar propiedades.
    /// </summary>
    /// <param name="expression">Expresion para obtener propiedad a ignorar.</param>
    /// <typeparam name="TSource">Tipo de entrada.</typeparam>
    /// <typeparam name="TDestination">Tipo de destino.</typeparam>
    /// <typeparam name="TProperty">Tipo de la propiedad de destino.</typeparam>
    public TypeAdapterBuilder<TSource, TDestination> Ignore<TProperty>(
        Expression<Func<TSource, TProperty>> expression)
    {
        var propertyInfo = PropertyHelper.GetPropertyInfo(expression);
        _ignoreFields.Add(propertyInfo.Name);
        return this;
    }
}