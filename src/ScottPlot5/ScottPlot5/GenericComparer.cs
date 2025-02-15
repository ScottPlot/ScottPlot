namespace ScottPlot;

/// <summary>
/// Generic helper used to provide <see cref="IComparer{T}"/> on supported types.
/// </summary>
/// <typeparam name="T"><inheritdoc cref="Default" path="/remarks"/></typeparam>
public static class GenericComparer<T>
{
    ///<summary>
    /// An appropriate <see cref="IComparer{T}"/> for this type.
    ///</summary> 
    ///<remarks>
    /// If the type is supported, this will be either <see cref="BinarySearchComparer.Instance"/> or <see cref="Comparer{T}.Default"/>
    /// <br/> If the type is unsupported, throws <see cref="TypeInitializationException"/>
    /// <para/> Supported Types :
    /// <br/> - All primitive types
    /// <br/> - Types comparable via <see cref="BinarySearchComparer.Instance"/>
    /// <br/> - Types that implement <see cref="IComparable{T}"/>
    ///</remarks>
    public static readonly IComparer<T> Default = GetComparer();
    private static IComparer<T> GetComparer()
    {
        if (typeof(T).IsPrimitive) return Comparer<T>.Default;
        if (BinarySearchComparer.Instance is IComparer<T> custom) return custom;
        if (typeof(IComparable<>).MakeGenericType(typeof(T)).IsAssignableFrom(typeof(T))) return Comparer<T>.Default;
        throw new ArgumentException($"{typeof(T)} is not supported. Must be a primitive type, handled by BinarySearchComparer, or implemenent IComparable<{typeof(T).Name}>.");
    }
}
