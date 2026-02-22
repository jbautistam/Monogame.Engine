namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

/// <summary>
///		Base para los efectos sobre un fondo
/// </summary>
public abstract class AbstractBackgroundEffect(float duration, bool autoRewind) : Entities.Common.Collections.ISecureListItem
{
    /// <summary>
    ///     Arranca el efecto
    /// </summary>
    public virtual void Start() {}

    /// <summary>
    ///     Actualiza el efecto
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        // Incrementa el tiempo
        ElapsedTime += gameContext.DeltaTime;
        // Actualiza el efecto
        UpdateEffect(gameContext);
    }

	/// <summary>
	///		Actualiza el efecto
	/// </summary>
	protected abstract void UpdateEffect(Managers.GameContext gameContext);

    /// <summary>
    ///     Detiene el efecto
    /// </summary>
    public virtual void End(Managers.GameContext gameContext) {}

	/// <summary>
	///		Id del efecto
	/// </summary>
	public string Id { get; } = Guid.NewGuid().ToString();

	/// <summary>
	///		Estado
	/// </summary>
	public BackgroundState State { get; } = new();

    /// <summary>
    ///     Indica si el efecto se debe autorebobinar
    /// </summary>
    public bool AutoRewind { get; } = autoRewind;

    /// <summary>
    ///     Duración del efecto
    /// </summary>
    public float Duration { get; } = duration;

    /// <summary>
    ///     Progreso
    /// </summary>
    public float Progress
    {
        get 
        { 
            if (Duration == 0)
                return 0;
            else
                return ElapsedTime / Duration; 
        }
    }

    /// <summary>
    ///     Tiempo transcurrido
    /// </summary>
    public float ElapsedTime { get; private set; }

    /// <summary>
    ///     Indica si ha terminado el efecto
    /// </summary>
    public bool IsCompleted => ElapsedTime >= Duration;
}