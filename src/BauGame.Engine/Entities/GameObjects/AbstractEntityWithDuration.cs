namespace Bau.BauEngine.Entities.GameObjects;

/// <summary>
///     Base de los comportamientos de una entidad que se procesa durante determinado tiempo
/// </summary>
public abstract class AbstractEntityWithDuration(float duration)
{
    /// <summary>
    ///     Arranca el comportamiento
    /// </summary>
    public virtual void Start() {}

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (IsActive && !IsComplete)
        {
            // Incrementa el tiempo
            if (Duration > 0)
                Progress = ElapsedTime / Duration;
            else
                Progress = 0;
            // Actualiza el comportamiento
            UpdateSelf(gameContext);
            // Incrementa el tiempo e indica que ya se ha inicializado
            IsInitialized = true;
            ElapsedTime += gameContext.DeltaTime;
            // Comprueba si se ha terminado
            if (Duration > 0 && ElapsedTime >= Duration)
            {
                OnComplete();
                IsComplete = true;
            }
        }
    }

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    protected abstract void UpdateSelf(Managers.GameContext gameContext);

    /// <summary>
    ///     Acciones a ejecutar cuando se termina el comportamiento
    /// </summary>
    protected virtual void OnComplete() {}

    /// <summary>
    ///     Finaliza el comportamiento
    /// </summary>
    public virtual void End(Managers.GameContext gameContext) {}

    /// <summary>
    ///     Id del comportamiento
    /// </summary>
	public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     Indica si se ha inicializado
    /// </summary>
    protected bool IsInitialized { get; private set; }

    /// <summary>
    ///     Duración del efecto
    /// </summary>
    public float Duration { get; set; } = duration;

    /// <summary>
    ///     Tiempo que ha pasado
    /// </summary>
    public float ElapsedTime { get; private set; }

    /// <summary>
    ///     Progreso
    /// </summary>
    public float Progress { get; private set; }

    /// <summary>
    ///     Indica si ha finalizado el comportamiento
    /// </summary>
    public bool IsComplete { get; protected set; }
}
