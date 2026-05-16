namespace Bau.BauEngine.Entities.Common.Collections;

/// <summary>
///     Lista concurrente
/// </summary>
public class ConcurrentList<TypeData>
{
    // Variables privadas
    private object _lock = new();
    private List<TypeData> _items = [];

    /// <summary>
    ///     Añade un elemento
    /// </summary>
    public void Add(TypeData item)
    {
        lock (_lock)
        {
            _items.Add(item);
        }
    }

    /// <summary>
    ///     Añade una serie de elementos
    /// </summary>
    public void AddRange(List<TypeData> items)
    {
        lock (_lock)
        {
            foreach (TypeData item in items)
                _items.Add(item);
        }
    }

    /// <summary>
    ///     Limpia los elementos de la lista
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _items.Clear();
        }
    }

    /// <summary>
    ///     Elimina un elemento
    /// </summary>
    public void Remove(TypeData item)
    {
        lock (_lock)
        {
            _items.Remove(item);
        }
    }

    /// <summary>
    ///     Elimina un elemento de una posición
    /// </summary>
    public void RemoveAt(int index)
    {
        lock (_lock)
        {
            if (index >= 0 && index < Count)
                _items.RemoveAt(index);
        }
    }

    /// <summary>
    ///     Enumera los elementos
    /// </summary>
    public IEnumerable<TypeData> Enumerate()
    {
        if (Count > 0)
            lock (_lock)
            {  
                foreach (TypeData item in _items)
                    yield return item;
            }
    }

    /// <summary>
    ///     Número de elementos
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    ///     Obtiene / asigna el valor de un elemento
    /// </summary>
    public TypeData this[int index]
    {
        get { return _items[index]; }
        set
        {
            lock (_lock)
            {
                _items[index] = value;
            }
        }
    }
}
