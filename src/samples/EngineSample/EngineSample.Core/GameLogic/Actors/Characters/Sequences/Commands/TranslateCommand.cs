using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Mueve un actor automáticamente a una posición
/// </summary>
public class TranslateCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Modo de movimiento
    /// </summary>
    public enum MovementMode
    {
        /// <summary>Movimiento sin interpolación: directamente a un punto</summary>
        ToPointInmediate,
        /// <summary>Movimiento relativo al punto inicial</summary>
        Relative,
        /// <summary>Movimiento absoluto a un punto</summary>
        To
    }
    // Variables privadas
    private Vector2 _start, _target;
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
            _target = Target;
            if (Mode == MovementMode.Relative)
                _target = _start + Target;
            _initialized = true;
        }
        // Calcula la posición interplada
        if (Mode == MovementMode.ToPointInmediate)
            actor.Transform.Bounds.TopLeft = _target;
        else
            actor.Transform.Bounds.TopLeft = ToWorld(EasingFunctionsHelper.Interpolate(_start, _target, Progress, Easing));
    }

    /// <summary>
    ///     Modo de movimiento
    /// </summary>
    public required MovementMode Mode { get; init; }

	/// <summary>
	///     Posición final
	/// </summary>
	public required Vector2 Target { get; init; }
}
