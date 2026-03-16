using Microsoft.Xna.Framework;
using UI.CharactersEngine.Characters;

namespace UI.CharactersEngine.Sequences.Commands;

public class OriginCommand : AbstractSequenceCommand
{
    private readonly OriginPoint _point;
    private readonly Vector2? _custom;
    
    public OriginCommand(string actorId, float startTime, OriginPoint point, Vector2? custom = null)
        : base(actorId, startTime, 0f)
    {
        _point = point;
        _custom = custom;
    }
    
    public override void Apply(Actor actor, float sequenceTime)
    {
        if (sequenceTime >= StartTime)
        {
            actor.Transform.Origin = _point.ToVector2(_custom);
        }
    }
}

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

public static class OriginPointExtensions
{
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
}
