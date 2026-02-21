using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.Sequences.Commands;

/// <summary>
///     Comando de una secuencia
/// </summary>
public abstract class AbstractSequenceCommand(string actorId, float startTime, float duration)
{
    /// <summary>
    ///     Indica si el comando está activo en un momento
    /// </summary>
    public bool IsActive(float elapsedTime) => elapsedTime > StartTime && elapsedTime <= StartTime + Duration;

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    public abstract void Apply(Actor actor, float sequenceTime);

    /// <summary>
    ///     Obtiene el progreso
    /// </summary>
    protected float GetProgress(float sequenceTime)
    {
        if (Duration == 0 || !IsActive(sequenceTime))
            return 1;
        else
            return (sequenceTime - StartTime) / Duration;
    }

    /// <summary>
    ///     Interpola un valor
    /// </summary>
    protected float Interpolate(float start, float end, float sequenceTime)
    {
        return MathHelper.Lerp(start, end, 
                               MathTools.Easing.EasingFunctionsHelper.Apply(GetProgress(sequenceTime), Easing));
    }

    /// <summary>
    ///     Interpola un color
    /// </summary>
    protected Color Interpolate(Color start, Color end, float sequenceTime)
    {
        return Color.Lerp(start, end, 
                          MathTools.Easing.EasingFunctionsHelper.Apply(GetProgress(sequenceTime), Easing));
    }

    /// <summary>
    ///     Interpola un vector
    /// </summary>
    protected Vector2 Interpolate(Vector2 start, Vector2 end, float sequenceTime)
    {
        return Vector2.Lerp(start, end, 
                            MathTools.Easing.EasingFunctionsHelper.Apply(GetProgress(sequenceTime), Easing));
    }

    /// <summary>
    ///     Clave del actor sobre el que se aplica el comando
    /// </summary>
    public string ActorId { get; } = actorId;

    /// <summary>
    ///     Momento de inicio del comando
    /// </summary>
    public float StartTime { get; } = startTime;

    /// <summary>
    ///     Duración del comando
    /// </summary>
    public float Duration { get; } = duration;

    /// <summary>
    ///     Tipo de función de suavizado
    /// </summary>
    public MathTools.Easing.EasingFunctionsHelper.EasingType Easing { get; set; } = MathTools.Easing.EasingFunctionsHelper.EasingType.Linear;

    /// <summary>
    ///     Momento final del comando
    /// </summary>
    public float EndTime => StartTime + Duration;
}
