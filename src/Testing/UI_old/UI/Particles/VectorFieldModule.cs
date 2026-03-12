namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class VectorFieldModule : IUpdateModule
{
    private readonly VectorFieldModuleDefinition _def;
    private readonly VectorFieldRuntime _field;
    
    public VectorFieldModule(VectorFieldModuleDefinition def, VectorFieldRuntime field)
    {
        _def = def;
        _field = field;
    }
    
    public void Update(in ParticleRef p, float deltaTime, float emitterTime)
    {
        var fieldForce = _field.Sample(p.Position);
        
        switch (_def.Mode)
        {
            case VectorFieldModuleDefinition.ApplyMode.Force:
                if (_def.RelativeToVelocity)
                {
                    var desired = Vector2.Normalize(fieldForce) * p.Velocity.Length();
                    var steer = desired - p.Velocity;
                    p.Velocity += Vector2.Clamp(steer, -_def.MaxDelta, _def.MaxDelta) * deltaTime;
                }
                else
                {
                    p.Velocity += Vector2.Clamp(fieldForce, -_def.MaxDelta, _def.MaxDelta) * deltaTime;
                }
                break;
                
            case VectorFieldModuleDefinition.ApplyMode.Velocity:
                p.Velocity = Vector2.Lerp(p.Velocity, fieldForce, 1 - MathF.Exp(-5 * deltaTime));
                break;
                
            case VectorFieldModuleDefinition.ApplyMode.Drag:
                var parallel = Vector2.Dot(p.Velocity, Vector2.Normalize(fieldForce));
                var perp = p.Velocity - Vector2.Normalize(fieldForce) * parallel;
                p.Velocity -= perp * (1 - MathF.Exp(-fieldForce.Length() * deltaTime));
                break;
        }
    }
}
