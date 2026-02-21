namespace ParticleEngine.Core;

// ============================================================================
// DEFINITIONS / CURVES
// ============================================================================

namespace ParticleEngine.Definitions;

// ============================================================================
// RUNTIME / CURVES
// ============================================================================

namespace ParticleEngine.Runtime;

internal sealed class CircleShape : EmissionShape
{
    private readonly CircleShapeDefinition _def;
    private float _currentArcRotation;
    
    public CircleShape(CircleShapeDefinition def) : base(def)
    {
        _def = def;
        _currentArcRotation = def.ArcStart;
    }
    
    public override bool IsAnimated => _def.ArcRotates;
    
    public override void Update(float deltaTime, float totalTime)
    {
        if (_def.ArcRotates)
        {
            _currentArcRotation += _def.ArcRotationSpeed * deltaTime;
            _currentArcRotation %= 360f;
        }
    }
    
    public override EmissionPoint Sample(ParticleRandom random)
    {
        float angleRad = SampleAngle(random);
        float radius;
        
        if (_def.EdgeOnly)
        {
            radius = _def.Radius;
            if (_def.EdgeThickness > 0)
                radius += random.Range(-_def.EdgeThickness / 2, _def.EdgeThickness / 2);
        }
        else
        {
            float rNorm = MathF.Sqrt(random.NextFloat());
            radius = MathHelper.Lerp(_def.InnerRadius, _def.Radius, rNorm);
            
            if (Definition.Distribution == EmissionShapeDefinition.DistributionMode.Gaussian)
                radius = _def.InnerRadius + (_def.Radius - _def.InnerRadius) * (1 - MathF.Abs(random.NextFloat() * 2 - 1));
        }
        
        var localPos = new Vector2(MathF.Cos(angleRad) * radius, MathF.Sin(angleRad) * radius);
        var direction = new Vector2(MathF.Cos(angleRad), MathF.Sin(angleRad));
        
        return new EmissionPoint(TransformPoint(localPos), TransformDirection(direction));
    }
    
    private float SampleAngle(ParticleRandom random)
    {
        float arcStart = _currentArcRotation * MathHelper.Pi / 180f;
        float arcSpan = _def.ArcSpan * MathHelper.Pi / 180f;
        return arcStart + random.NextFloat() * arcSpan;
    }
    
    public override float GetApproximateArea()
    {
        float ringArea = MathF.PI * (_def.Radius * _def.Radius - _def.InnerRadius * _def.InnerRadius);
        return ringArea * (_def.ArcSpan / 360f);
    }
    
    public override BoundingBox2D GetBounds()
    {
        var r = _def.Radius;
        return new BoundingBox2D(TransformPoint(new Vector2(-r, -r)), TransformPoint(new Vector2(r, r)));
    }
}
