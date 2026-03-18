using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.Transitions;

public class RenderTargetPool
{
    private readonly GraphicsDevice _graphics;
    private readonly Dictionary<Point, Queue<RenderTarget2D>> _pool = new();
    private readonly List<RenderTarget2D> _inUse = new();
        
    public RenderTargetPool(GraphicsDevice graphics)
    {
        _graphics = graphics;
    }
        
    public RenderTarget2D Acquire(int width, int height)
    {
        var size = new Point(width, height);
            
        if (_pool.TryGetValue(size, out var queue) && queue.Count > 0)
        {
            var target = queue.Dequeue();
            _inUse.Add(target);
            return target;
        }
            
        var newTarget = new RenderTarget2D(_graphics, width, height, false, 
            SurfaceFormat.Color, DepthFormat.None);
        _inUse.Add(newTarget);
        return newTarget;
    }
        
    public void Release(RenderTarget2D target)
    {
        if (target == null || target.IsDisposed) return;
            
        _inUse.Remove(target);
        var size = new Point(target.Width, target.Height);
            
        if (!_pool.ContainsKey(size))
            _pool[size] = new Queue<RenderTarget2D>();
                
        _pool[size].Enqueue(target);
    }
        
    public void Clear()
    {
        foreach (var queue in _pool.Values)
            while (queue.Count > 0)
                queue.Dequeue().Dispose();
                    
        _pool.Clear();
        _inUse.Clear();
    }
}
