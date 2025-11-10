namespace Bau.Libraries.BauGame.Engine.Pool;

/// <summary>
///     Pool de objetos
/// </summary>
public class ObjectPool<TypeData>(int? maxSize = null) where TypeData : IPoolable
{
    // Variables privadas
    private List<TypeData> _pool = [];

    /// <summary>
    ///     Añade un elemento al pool
    /// </summary>
    public void Add(TypeData item)
    {
        // Añade un elemento
        if (MaxSize is null || Count < MaxSize)
            _pool.Add(item);
        // Recupera los inactivos
        Shrink();
    }

    /// <summary>
    ///     Obtiene el primer objeto inactivo del pool
    /// </summary>
    public TypeData? GetFirstInactive() => _pool.FirstOrDefault(item => !item.Enabled);

    /// <summary>
    ///     Cuenta los elementos activos
    /// </summary>
	public int CountEnabled()
    {
        int count = 0;

            // Cuenta los elementos activos
            foreach (TypeData item in _pool)
                if (item.Enabled)
                    count++;
            // Devuelve el número de elementos activos
            return count;
    }

    /// <summary>
    ///     Elimina un elemento
    /// </summary>
    public void Remove(TypeData item)
    {
        _pool.Remove(item);
    }

    /// <summary>
    ///     Elimina los elementos inactivos
    /// </summary>
    private void Shrink()
    {
        int count = 0;

            // Cuenta los elementos inactivos
            foreach (IPoolable item in _pool)
                if (!item.Enabled)
                    count++;
            // Borra si se ha superado el número máximo de elementos inactivos
            if (count > Count / 2)
                for (int index = _pool.Count - 1; index >= 0; index--)
                    if (!_pool[index].Enabled)
                        _pool.RemoveAt(index);
    }

    /// <summary>
    ///     Enumera los elementos de la lista
    /// </summary>
    public IEnumerable<TypeData> Enumerate()
    {
        for (int index = 0; index < Count; index++)
            if (_pool[index].Enabled)
                yield return _pool[index];
    }

    /// <summary>
    ///     Limpia la lista
    /// </summary>
    public void Clear()
    {
        _pool.Clear();
    }

    /// <summary>
    ///     Número de elementos del pool
    /// </summary>
    public int Count => _pool.Count;

    /// <summary>
    ///     Tamaño máximo
    /// </summary>
    public int? MaxSize { get; set; } = maxSize;
}
