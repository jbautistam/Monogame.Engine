using Microsoft.Xna.Framework.Graphics;
using UI.CharactersEngine.Transitions;

namespace GameEngine.Rendering.PostProcess;

public class PostProcessStack
{
    private readonly List<IPostProcessEffect> _effects = new List<IPostProcessEffect>();
    private readonly RenderTargetPool _targetPool;
    private readonly GraphicsDevice _device;

    public PostProcessStack(GraphicsDevice device, RenderTargetPool targetPool)
    {
        _device = device;
        _targetPool = targetPool;
    }

    public void AddEffect(IPostProcessEffect effect)
    {
        _effects.Add(effect);
    }

    public void RemoveEffect(IPostProcessEffect effect)
    {
        _effects.Remove(effect);
    }

    public RenderTarget2D Process(SpriteBatch spriteBatch, RenderTarget2D source, int targetWidth, int targetHeight)
    {
        if (_effects.Count == 0) return source;

        RenderTarget2D current = source;
        RenderTarget2D next = null;

        for (int i = 0; i < _effects.Count; i++)
        {
            var effect = _effects[i];
                
            if (effect.IsEnabled == false) continue;

            bool isLast = (i == _effects.Count - 1);
                
            if (isLast == false)
            {
                next = _targetPool.Acquire(targetWidth, targetHeight);
            }

            _device.SetRenderTarget(isLast ? null : next);
            effect.Apply(spriteBatch, current, isLast ? null : next);

            if (current != source)
            {
                _targetPool.Release(current);
            }

            current = next;
        }

        return current;
    }
}
