using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Mueve un actor
/// </summary>
public class MoveCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _start;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Guarda la posición inicial
        if (!_initialized)
        {
            _start = ToRelative(actor.Transform.Bounds.TopLeft);
            _initialized = true;
        }
        // Calcula la posición interplada
        actor.Transform.Bounds.TopLeft = ToWorld(EasingFunctionsHelper.Interpolate(_start, Target, Progress, Easing));
    }

	/// <summary>
	///     Posición final
	/// </summary>
	public required Vector2 Target { get; init; }
}
