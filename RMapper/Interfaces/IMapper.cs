namespace RMapper.Interfaces;

/// <summary>
/// Interfáz mapper. 
/// </summary>
public interface IMapper
{
    /// <summary>
    /// Metodo para mapear de una entidad a un tipo nuevo.
    /// </summary>
    /// <param name="source">Origen de los datos.</param>
    /// <typeparam name="TSource">Tipo de origen de los datos.</typeparam>
    /// <typeparam name="TDestination">Tipo de destino</typeparam>
    /// <returns>Entrega un objeto <typeparamref name="TDestination"/></returns>
    public TDestination? Map<TSource, TDestination>(TSource source)
        where TDestination : class, new();

    /// <summary>
    /// Método para mapear una de un listado a otro.
    /// </summary>
    /// <param name="source">Origen de los datos.</param>
    /// <typeparam name="TSource">Tipo del listado de origen.</typeparam>
    /// <typeparam name="TDestination">Tipo del listado de destino.</typeparam>
    /// <returns>Retorna un listado de datos de tipo <typeparam name="TDestination" /></returns>
    public List<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> source)
        where TDestination : class, new();

    /// <summary>
    /// Metodo para adaptar de una entidad a otra existente.
    /// </summary>
    /// <param name="source">Origen de los datos.</param>
    /// <param name="destination">Objeto destino.</param>
    /// <typeparam name="TSource">Tipo de origen de los datos.</typeparam>
    /// <typeparam name="TDestination">Tipo de destino</typeparam>
    /// <returns>Entrega un objeto <typeparamref name="TDestination"/></returns>
    public TDestination? Adapt<TSource, TDestination>(TSource source, TDestination destination)
        where TDestination : class, new();
}