namespace Bau.Libraries.BauGame.Engine.Scenes.Timers;

/// <summary>
///     Datos de un temporizador
/// </summary>
public class TimerModel(float duration, Action onComplete, bool repeat)
{
    /// <summary>
    ///     Arranca el temporizador
    /// </summary>
    public void Start()
    {
        IsRunning = true;
        Elapsed = 0;
    }

    /// <summary>
    ///     Detiene el temporizador
    /// </summary>
    public void Stop()
    {
        IsRunning = false;
    }

    /// <summary>
    ///     Actualiza el temporizador
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (IsRunning)
        {
            // Acumula el tiempo
            Elapsed += gameContext.DeltaTime;
            // Si se ha llegado al final ...
            if (Elapsed >= Duration)
            {
                // Llama a la acción asociada
                OnComplete?.Invoke();
                // Repite el proceso si es necesario
                if (Repeat)
                    Start();
                else
                    Stop();
            }
        }
    }

    /// <summary>
    ///     Duración del temporizador
    /// </summary>
    public float Duration { get; } = duration;

    /// <summary>
    ///     Indica si se debe repetir cuando se acabe su tiempo
    /// </summary>
    public bool Repeat { get; } = repeat;

    /// <summary>
    ///     Acción que se ejecuta cuando se termina el tiempo
    /// </summary>
    public Action? OnComplete { get; } = onComplete;

    /// <summary>
    ///     Tiempo que ha pasado desde el inicio
    /// </summary>
    public float Elapsed { get; private set; }

    /// <summary>
    ///     Indica si el temporizador se está ejecutando
    /// </summary>
    public bool IsRunning { get; private set; }

    /// <summary>
    ///     Indica si el temporizador ha finalizado
    /// </summary>
    public bool IsFinished => Elapsed >= Duration && !Repeat;
}
