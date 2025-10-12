namespace Bau.Monogame.Engine.Domain.Core.Pool;

public class PoolManager
{
    private Dictionary<string, object> pools;
    
    public PoolManager()
    {
        pools = new Dictionary<string, object>();
    }

    public void AddPool<T>(string name, GenericObjectPool<T> pool) where T : class, new()
    {
        pools[name] = pool;
    }

    public GenericObjectPool<T> GetPool<T>(string name) where T : class, new()
    {
        if (pools.ContainsKey(name) && pools[name] is GenericObjectPool<T> pool)
        {
            return pool;
        }
        return null;
    }

    public T GetObject<T>(string poolName) where T : class, new()
    {
        var pool = GetPool<T>(poolName);
        return pool?.GetObject();
    }

    public void ReturnObject<T>(string poolName, T obj) where T : class, new()
    {
        var pool = GetPool<T>(poolName);
        pool?.ReturnObject(obj);
    }

    public void UpdateAllPools(GameTime gameTime)
    {
        // Aquí podrías actualizar lógica común si es necesario
    }

    public void ClearAllPools()
    {
        foreach (var pool in pools.Values)
        {
            if (pool is GenericObjectPool<Projectile> projectilePool)
                projectilePool.ReturnAllObjects();
            else if (pool is GenericObjectPool<ExplosionComponent> explosionPool)
                explosionPool.ReturnAllObjects();
            // Agregar más tipos según sea necesario
        }
    }
}
