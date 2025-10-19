namespace Bau.Libraries.BauGame.Engine.Pool;

/// <summary>
///     Manager de <see cref="ObjectPool{TypeData}"/>
/// </summary>
public class PoolManager
{
    // Variables privadas
    private Dictionary<string, ObjectPool<IPoolable>> _pools = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Crea un nuevo pool
    /// </summary>
    public void CreatePool(string name)
    {
        if (!_pools.ContainsKey(name))
            _pools.Add(name, new ObjectPool<IPoolable>());
    }

    /// <summary>
    ///     Obtiene un pool en concreto
    /// </summary>
    public ObjectPool<TypeData>? GetPool<TypeData>(string name) where TypeData : IPoolable
    {
        if (_pools.TryGetValue(name, out ObjectPool<TypeData>? pool))
            return pool;
        else
            return null;
    }
}
