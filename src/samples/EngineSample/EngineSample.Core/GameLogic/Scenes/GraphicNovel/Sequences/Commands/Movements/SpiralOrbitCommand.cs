using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors;
using Bau.BauEngine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///		Comando para describir una espiral en órbita
/// </summary>
public class SpiralOrbitCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(AbstractActorDrawable actor)
    {
        float interpolated = EasingFunctionsHelper.Apply(Progress, Easing);
        float angle = interpolated * Turns * MathHelper.TwoPi;
        float radius = MathHelper.Lerp(StartRadius, EndRadius, interpolated);
        
            // Cambia el ángulo
            if (!Inward) 
                angle = -angle;
            // Cambia la ubicación y la rotación acumulada
            actor.Transform.Bounds.Location = new Vector2(Center.X + MathF.Cos(angle) * radius, Center.Y + MathF.Sin(angle) * radius);
            actor.Transform.Rotation = angle;
    }

	/// <summary>
	///     Centro de la órbita
	/// </summary>
	public required Vector2 Center { get; init; }

    /// <summary>
    ///     Radio inicial
    /// </summary>
    public required float StartRadius { get; init; }

    /// <summary>
    ///     Radio final
    /// </summary>
    public required float EndRadius { get; init; }

    /// <summary>
    ///     Número de movimientos
    /// </summary>
    public required float Turns { get; init; }

    /// <summary>
    ///     Indica si se va del ángulo inical al final o al contrario
    /// </summary>
    public required bool Inward { get; init; }
}