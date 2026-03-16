namespace UI.CharactersEngine.Sequences.Commands;

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
    public override void Apply(Actor actor, float sequenceTime)
    {
        // Obtiene la opacidad inicial del actor
        if (!_initialized)
        {
            _startOpacity = actor.Opacity;
            _initialized = true;
        }
        // Cambia el valor del actor
        actor.Opacity = Interpolate(_startOpacity, TargetOpacity, sequenceTime);
    }

    /// <summary>
    ///     Opacidad final
    /// </summary>
    public required float TargetOpacity { get; init; }
}
