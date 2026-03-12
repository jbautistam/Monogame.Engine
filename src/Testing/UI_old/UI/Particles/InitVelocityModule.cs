namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class InitVelocityModule : IInitModule
{
    private readonly InitVelocityModuleDefinition _def;
    private readonly ParticleCurve? _inheritCurve;
    
    public InitVelocityModule(InitVelocityModuleDefinition def, ICurveCompiler compiler)
    {
        _def = def;
        _inheritCurve = def.InheritVelocityByDirection != null ? compiler.Compile(def.InheritVelocityByDirection) : null;
    }
    
    public void Initialize(in ParticleRef p, in EmissionPoint emissionPoint, float emitterTime)
    {
        Vector2 direction = _def.DirectionMode switch
        {
            DirectionMode.Random => RandomDirection(p),
            DirectionMode.Directional => DirectionalDirection(),
            DirectionMode.Outward => Vector2.Normalize(p.Position),
            DirectionMode.Inward => -Vector2.Normalize(p.Position),
            _ => emissionPoint.Direction
        };
        
        if (_def.Spread > 0 && _def.DirectionMode != DirectionMode.Random)
        {
            float baseAngle = MathF.Atan2(direction.Y, direction.X);
            float spreadRad = _def.Spread * MathHelper.Pi / 180f;
            float randomAngle = baseAngle + (new Random(p.Index).NextSingle() * 2 - 1) * spreadRad;
            direction = new Vector2(MathF.Cos(randomAngle), MathF.Sin(randomAngle));
        }
        
        float speed = _def.Speed.GetValue(p.Index);
        p.Velocity = direction * speed;
    }
    
    private Vector2 RandomDirection(in ParticleRef p)
    {
        float angle = new Random(p.Index).NextSingle() * MathHelper.TwoPi;
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }
    
    private Vector2 DirectionalDirection()
    {
        float angle = new Random().NextSingle() * (_def.Direction.Max - _def.Direction.Min) + _def.Direction.Min;
        angle *= MathHelper.Pi / 180f;
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }
}
