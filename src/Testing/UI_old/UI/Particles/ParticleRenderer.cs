namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

public sealed class ParticleRenderer
{
    private readonly GraphicsDevice _device;
    private readonly SpriteBatch _spriteBatch;
    
    public ParticleRenderer(GraphicsDevice device)
    {
        _device = device;
        _spriteBatch = new SpriteBatch(device);
    }
    
    public void Begin(Matrix viewProjection, ParticleSortMode sortMode = ParticleSortMode.ByDistance)
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            SamplerState.LinearClamp, DepthStencilState.None, 
            RasterizerState.CullNone, null, viewProjection);
    }
    
    public void DrawSprite(Texture2D texture, Vector2 position, Rectangle? sourceRect,
        Color color, float rotation, Vector2 pivot, Vector2 scale, SpriteEffects flip)
    {
        Vector2 origin = pivot * new Vector2(
            sourceRect?.Width ?? texture.Width,
            sourceRect?.Height ?? texture.Height);
        
        _spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, flip, 0);
    }
    
    public void End() => _spriteBatch.End();
    
    public void Dispose() => _spriteBatch.Dispose();
}
