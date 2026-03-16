namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

// Rotación - solo valor final
public class RotateCommand : AbstractSequenceCommand
{
public enum OriginPoint
{
    None,           // Usar valor personalizado (0-1, 0-1)
    Center,         // (0.5, 0.5)
    TopLeft,        // (0, 0)
    TopCenter,      // (0.5, 0)
    TopRight,       // (1, 0)
    CenterLeft,     // (0, 0.5)
    CenterRight,    // (1, 0.5)
    BottomLeft,     // (0, 1)
    BottomCenter,   // (0.5, 1)
    BottomRight     // (1, 1)
}

    public static Vector2 ToVector2(this OriginPoint point, Vector2? custom = null)
    {
        return point switch
        {
            OriginPoint.None => custom ?? new Vector2(0.5f, 1f), // Default bottom-center para personajes
            OriginPoint.Center => new Vector2(0.5f, 0.5f),
            OriginPoint.TopLeft => new Vector2(0f, 0f),
            OriginPoint.TopCenter => new Vector2(0.5f, 0f),
            OriginPoint.TopRight => new Vector2(1f, 0f),
            OriginPoint.CenterLeft => new Vector2(0f, 0.5f),
            OriginPoint.CenterRight => new Vector2(1f, 0.5f),
            OriginPoint.BottomLeft => new Vector2(0f, 1f),
            OriginPoint.BottomCenter => new Vector2(0.5f, 1f),
            OriginPoint.BottomRight => new Vector2(1f, 1f),
            _ => new Vector2(0.5f, 1f)
        };
    }

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