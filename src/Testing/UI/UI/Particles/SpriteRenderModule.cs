namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

// ============================================================================
// RUNTIME / MODULES - RENDER
// ============================================================================

internal sealed class SpriteRenderModule : IRenderModule
{
    private readonly SpriteRenderModuleDefinition _def;
    private readonly Texture2D _texture;
    
    public SpriteRenderModule(SpriteRenderModuleDefinition def, Texture2D texture)
    {
        _def = def;
        _texture = texture;
    }
    
    public void Render(ParticleRenderer renderer, in ParticleDataArrays data, int aliveCount)
    {
        for (int i = 0; i < aliveCount; i++)
        {
            if (!data.IsAlive[i]) continue;
            
            Vector2 scale = data.Scales[i];
            float rotation = data.Rotations[i];
            
            if (_def.Billboard == SpriteRenderModuleDefinition.BillboardMode.Stretched)
            {
                float speed = data.Velocities[i].Length();
                scale.Y *= 1f + speed * _def.StretchFactor;
                rotation = MathF.Atan2(data.Velocities[i].Y, data.Velocities[i].X);
            }
            
            renderer.DrawSprite(_texture, data.Positions[i], _def.SourceRect, 
                data.Colors[i], rotation, _def.Pivot, scale, _def.Flip);
        }
    }
}
