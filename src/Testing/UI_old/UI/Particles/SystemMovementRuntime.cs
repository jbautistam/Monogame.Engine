namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class SystemMovementRuntime
{
    private readonly SystemMovementDefinition _def;
    private readonly ParticleCurve? _posXCurve;
    private readonly ParticleCurve? _posYCurve;
    private float _orbitAngle;
    
    public SystemMovementRuntime(SystemMovementDefinition def, ICurveCompiler compiler)
    {
        _def = def;
        _posXCurve = def.PositionXCurve != null ? compiler.Compile(def.PositionXCurve) : null;
        _posYCurve = def.PositionYCurve != null ? compiler.Compile(def.PositionYCurve) : null;
    }
    
    public void Update(Transform2D transform, float time, Vector2 spawnPosition, float deltaTime)
    {
        Vector2 targetPosition = spawnPosition;
        
        if (_posXCurve != null || _posYCurve != null)
        {
            float x = _posXCurve?.Evaluate(time % 1f) ?? 0;
            float y = _posYCurve?.Evaluate(time % 1f) ?? 0;
            targetPosition += new Vector2(x, y);
        }
        
        if (_def.Orbit != null)
        {
            _orbitAngle += _def.Orbit.AngularSpeed * deltaTime;
            float ox = MathF.Cos(_orbitAngle) * _def.Orbit.Radius;
            float oy = MathF.Sin(_orbitAngle) * _def.Orbit.Radius * _def.Orbit.EllipseRatio;
            targetPosition += new Vector2(ox, oy);
        }
        
        if (_def.ConstantVelocity.HasValue)
            transform.Velocity = _def.ConstantVelocity.Value;
        
        if (_def.Acceleration.HasValue)
            transform.Acceleration = _def.Acceleration.Value;
        
        if (_posXCurve != null || _posYCurve != null || _def.Orbit != null)
        {
            transform.Position = targetPosition - transform.AnimatedOffset;
        }
        else
        {
            transform.PhysicsUpdate(deltaTime);
        }
    }
}
