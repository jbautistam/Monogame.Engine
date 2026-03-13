using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Cambia la opacidad del actor
/// </summary>
public class FadeCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private float _startOpacity;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.Libraries.BauGame.Engine.Actors.AbstractActorDrawable actor)
    {
        // Obtiene la opacidad inicial del actor
        if (!_initialized)
        {
            _startOpacity = actor.Renderer.Opacity;
            _initialized = true;
        }
        // Cambia el valor del actor
        actor.Renderer.Opacity = EasingFunctionsHelper.Interpolate(_startOpacity, TargetOpacity, Progress, Easing);
    }

    /// <summary>
    ///     Opacidad final
    /// </summary>
    public required float TargetOpacity { get; init; }
}
