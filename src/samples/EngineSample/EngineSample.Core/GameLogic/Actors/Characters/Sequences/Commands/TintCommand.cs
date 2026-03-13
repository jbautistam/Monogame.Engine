using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Comando para cambiar el color de un actor
/// </summary>
public class TintCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Color _start;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Guarda el valor inicial
        if (!_initialized)
        {
            _start = actor.Renderer.Color;
            _initialized = true;
        }
        // Aplica el valor interpolado
        actor.Renderer.Color = EasingFunctionsHelper.Interpolate(_start, Target, Progress, Easing);
    }

    /// <summary>
    ///     Color final
    /// </summary>
    public required Color Target { get; init; }
}
