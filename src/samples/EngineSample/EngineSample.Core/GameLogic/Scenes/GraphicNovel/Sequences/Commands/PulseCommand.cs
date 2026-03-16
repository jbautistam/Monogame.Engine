using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Pulso - latido: escala oscilante
/// </summary>
public class PulseCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _startScale;
    private bool _initialized;

    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        float sine = MathF.Sin(Progress * MathHelper.TwoPi * Pulses);
        float pulse = (sine + 1) / 2 * Amplitude * (1 - EasingFunctionsHelper.Apply(Progress, Easing));

            // Recoge los datos iniciales
            if (!_initialized)
            {
                _startScale = actor.Renderer.Scale;
                _initialized = true;
            }
            // Cambia la escala
            actor.Renderer.Scale = _startScale * (1 + pulse);
    }

    /// <summary>
    ///     Cuánto crece el actor. Por ejemplo: 0.2 es un 20% más grande
    /// </summary>
    public float Amplitude { get; init; }

    /// <summary>
    ///     Número de pulsos
    /// </summary>
    public required int Pulses { get; init; }
}
