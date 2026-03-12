namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class RectangleShape : EmissionShape
{
    private readonly RectangleShapeDefinition _def;
    private readonly float _halfW, _halfH;
    
    public RectangleShape(RectangleShapeDefinition def) : base(def)
    {
        _def = def;
        _halfW = def.Size.X / 2;
        _halfH = def.Size.Y / 2;
        
        if (!def.Centered)
        {
            // Ajustar transform
        }
    }
    
    public override EmissionPoint Sample(ParticleRandom random)
    {
        float x = random.Range(-_halfW, _halfW);
        float y = random.Range(-_halfH, _halfH);
        
        if (_def.CornerRadius > 0 && IsInCorner(x, y))
        {
            // Reject sampling o clamp
            if (!IsValidCorner(x, y)) return Sample(random);
        }
        
        return new EmissionPoint(TransformPoint(new Vector2(x, y)), Vector2.Zero);
    }
    
    private bool IsInCorner(float x, float y) => 
        MathF.Abs(x) > _halfW - _def.CornerRadius && MathF.Abs(y) > _halfH - _def.CornerRadius;
    
    private bool IsValidCorner(float x, float y)
    {
        float dx = MathF.Abs(x) - (_halfW - _def.CornerRadius);
        float dy = MathF.Abs(y) - (_halfH - _def.CornerRadius);
        return dx * dx + dy * dy <= _def.CornerRadius * _def.CornerRadius;
    }
    
    public override float GetApproximateArea() => _def.Size.X * _def.Size.Y;
    public override BoundingBox2D GetBounds() => 
        new(TransformPoint(new Vector2(-_halfW, -_halfH)), TransformPoint(new Vector2(_halfW, _halfH)));
}
