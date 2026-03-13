namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

public class CurveMoveCommand : Command
{
    private readonly ICurveDefinition _curveDef;
    private readonly CurveTransform _transform;
    private readonly IEasingDefinition _easing;
    private bool _initialized;
    
    public CurveMoveCommand(
        string actorId,
        float startTime,
        float duration,
        ICurveDefinition curveDef,
        CurveTransform transform,
        IEasingDefinition easing = null)
        : base(actorId, startTime, duration)
    {
        _curveDef = curveDef;
        _transform = transform;
        _easing = easing ?? new LambdaDefinition(Easing.Linear);
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime < StartTime) return;
        
        // Inicializar transform con posición actual del actor
        if (!_initialized)
        {
            _transform.Start = actor.Transform.Position;
            _initialized = true;
        }
        
        if (sequenceTime >= EndTime)
        {
            actor.Transform.Position = _transform.Transform(_curveDef.Evaluate(1));
            return;
        }
        
        float t = (sequenceTime - StartTime) / Duration;
        float easedT = _easing.Evaluate(t);
        
        Vector2 uv = _curveDef.Evaluate(easedT);
        actor.Transform.Position = _transform.TransformFree(uv);
    }
}
/*
```


public class SequenceBuilder
{
    // Curva con transformación automática desde posición actual
    public SequenceBuilder Curve(string actor, ICurveDefinition curveDef, 
        Vector2? end = null, float? scale = null, float? rotation = null,
        float duration = 1f, IEasingDefinition ease = null)
    {
        var transform = new CurveTransform();
        if (end.HasValue) transform.End = end.Value;
        if (scale.HasValue) transform.Scale = scale.Value;
        if (rotation.HasValue) transform.Rotation = rotation.Value;
        
        _sequence.Add(new CurveMoveCommand(actor, _time, duration, curveDef, transform, ease));
        return this;
    }
    
    // Curvas predefinidas comunes
    
    // Salto en arco: altura relativa al desplazamiento
    public SequenceBuilder Jump(string actor, Vector2 to, float height = 0.3f, 
        float duration = 1f, IEasingDefinition ease = null)
    {
        var curve = new QuadraticBezierDef(height); // height en UV (0.5 = semicírculo)
        var transform = new CurveTransform { End = to };
        return Curve(actor, curve, to, null, null, duration, ease);
    }
    
    // Entrada suave con overshoot
    public SequenceBuilder Enter(string actor, Vector2 from, Vector2 to,
        float overshoot = 0.2f, float duration = 0.8f)
    {
        // Curva que se pasa y vuelve
        var curve = new CubicBezierDef(
            tensionIn: 0, heightIn: 0,           // Entrada recta
            tensionOut: 0.5f, heightOut: -overshoot // Overshoot al final
        );
        return Curve(actor, curve, to, null, null, duration, Ease.QuadOut);
    }
    
    // Trayectoria S (esquiva)
    public SequenceBuilder Sway(string actor, Vector2 to, float width = 0.2f,
        float duration = 1f)
    {
        var curve = new CubicBezierDef(
            tensionIn: 0.3f, heightIn: width,
            tensionOut: 0.7f, heightOut: -width
        );
        return Curve(actor, curve, to, null, null, duration, Ease.Linear);
    }
    
    // Espiral desde centro
    public SequenceBuilder SpiralIn(string actor, Vector2 center, float startScale = 500f,
        float turns = 2, float duration = 2f)
    {
        var curve = new SpiralDef(turns);
        var transform = new CurveTransform 
        { 
            Start = center,
            Scale = startScale 
        };
        _sequence.Add(new CurveMoveCommand(actor, _time, duration, curve, transform, Ease.ExpoOut));
        return this;
    }
    
    // Spline por puntos relativos (0-1)
    public SequenceBuilder Path(string actor, float duration, params Vector2[] uvPoints)
    {
        var curve = new CatmullRomDef(uvPoints);
        return Curve(actor, curve, null, null, null, duration);
    }
}
*/