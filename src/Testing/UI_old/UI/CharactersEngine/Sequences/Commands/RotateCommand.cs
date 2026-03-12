namespace UI.CharactersEngine.Sequences.Commands;

// Rotación - solo valor final
public class RotateCommand : AbstractSequenceCommand
{
    private readonly float _targetRotation;
    private readonly EasingType _easing;
    private readonly OriginPoint _originPoint;
    private readonly Vector2? _customOrigin;
    
    private float _startRotation;
    private Vector2 _startOrigin;
    private bool _initialized;
    
    // Constructor con OriginPoint predefinido
    public RotateCommand(string actorId, float startTime, float duration,
        float targetDegrees, 
        OriginPoint originPoint = OriginPoint.None,
        EasingType easing = EasingType.QuadInOut)
        : base(actorId, startTime, duration)
    {
        _targetRotation = MathHelper.ToRadians(targetDegrees);
        _easing = easing;
        _originPoint = originPoint;
        _customOrigin = null;
    }
    
    // Constructor con origen personalizado (0-1, 0-1)
    public RotateCommand(string actorId, float startTime, float duration,
        float targetDegrees,
        Vector2 customOrigin,
        EasingType easing = EasingType.QuadInOut)
        : base(actorId, startTime, duration)
    {
        _targetRotation = MathHelper.ToRadians(targetDegrees);
        _easing = easing;
        _originPoint = OriginPoint.None;
        _customOrigin = customOrigin;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (!_initialized)
        {
            _startRotation = actor.Transform.Rotation;
            _startOrigin = actor.Transform.Origin;
            
            // Aplicar nuevo origen si se especificó
            if (_originPoint != OriginPoint.None || _customOrigin.HasValue)
            {
                actor.Transform.Origin = _originPoint.ToVector2(_customOrigin);
            }
            
            _initialized = true;
        }
        
        if (sequenceTime < StartTime) return;
        if (sequenceTime >= EndTime)
        {
            actor.Transform.Rotation = _targetRotation;
            return;
        }
        
        float t = (sequenceTime - StartTime) / Duration;
        float easedT = EasingFunctions.Apply(_easing, t);
        actor.Transform.Rotation = MathHelper.Lerp(_startRotation, _targetRotation, easedT);
    }
}