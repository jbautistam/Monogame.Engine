using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands;

/// <summary>
///     Comando para escalar un actor
/// </summary>
public class ScaleCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _startScale;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Guarda la escala inicial
        if (!_initialized)
        {
            _startScale = actor.Renderer.Scale;
            _initialized = true;
        }
        // Asigna la escala actual
        actor.Renderer.Scale = EasingFunctionsHelper.Interpolate(_startScale, Scale, Progress, Easing);
    }

    /// <summary>
    ///     Escala final
    /// </summary>
    public required Vector2 Scale { get; init; }
}
