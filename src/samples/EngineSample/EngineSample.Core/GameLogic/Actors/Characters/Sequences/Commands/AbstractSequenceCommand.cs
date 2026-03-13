using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Actors;
using Bau.Libraries.BauGame.Engine.Tools.MathTools.Easing;
using Bau.Libraries.BauGame.Engine.Managers;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

/// <summary>
///     Comando de una secuencia
/// </summary>
public abstract class AbstractSequenceCommand(string actorId, float startTime, float duration)
{
    // Variables privadas
    private float _elapsedTime;
    private AbstractActorDrawable? _actor;

    /// <summary>
    ///     Arranca el comando
    /// </summary>
    public void Start(AbstractActorDrawable actor)
    {
        _actor = actor;
        Started = true;
    }

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    public void Apply(GameContext gameContext)
    {
        // Calcula el progreso
        if (Duration <= 0 || !IsActive())
            Progress = 1;
        else
            Progress = _elapsedTime / Duration;
        // Ajusta el progreso
        if (Progress > 0.99f)
            Progress = 1;
        // Ejecuta el comando
        if (_actor is not null)
            ApplySelf(_actor);
        // Incrementa el tiempo
        _elapsedTime += gameContext.DeltaTime;
    }

    /// <summary>
    ///     Aplica el comando sobre el actor
    /// </summary>
    protected abstract void ApplySelf(AbstractActorDrawable actor);

    /// <summary>
    ///     Transforma las coordenadas en coordenadas de mundo
    /// </summary>
    protected Vector2 ToWorld(Vector2 position)
    {
        if (_actor is not null)
            return _actor.Layer.Scene.WorldDefinition.RelativeToWorld(position);
        else
            return Vector2.Zero;
    }

    /// <summary>
    ///     Transforma las coordenadas de mundo en coordenadas relativas 0 .. 1
    /// </summary>
	protected Vector2 ToRelative(Vector2 position)
	{
		if (_actor is not null)
            return _actor.Layer.Scene.WorldDefinition.WorldToRelative(position);
        else
            return position;
	}

    /// <summary>
    ///     Indica si se debe iniciar el comando
    /// </summary>
	public bool MustStart(float elapsedTime) => !Started && elapsedTime > StartTime && elapsedTime <= StartTime + Duration;

    /// <summary>
    ///     Indica si el comando está activo en este momento
    /// </summary>
    public bool IsActive() => Started && Progress < 1;

	/// <summary>
	///     Clave del actor sobre el que se aplica el comando
	/// </summary>
	public string ActorId { get; } = actorId;

    /// <summary>
    ///     Indica si se ha iniciado la ejecución del comando
    /// </summary>
    public bool Started { get; private set; }

    /// <summary>
    ///     Indica si el comando está activo (se desactiva desde fuera si por ejemplo no se encuentra el actor especificado)
    /// </summary>
    public bool Enabled { get; set; } = true;

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
    public EasingFunctionsHelper.EasingType Easing { get; set; } = EasingFunctionsHelper.EasingType.Linear;

    /// <summary>
    ///     Progreso del comando
    /// </summary>
    protected float Progress { get; private set; }
}
