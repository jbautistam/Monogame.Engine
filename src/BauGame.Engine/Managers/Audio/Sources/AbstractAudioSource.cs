using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

/// <summary>
///     Clase base para fuentes de audio
/// </summary>
public abstract class AbstractAudioSource(List<ScheduleEvent>? scheduleEvents)
{
    // Eventos públicos
    public event EventHandler<EventArguments.AudioSyncEventArgs>? SyncFound;

    /// <summary>
    ///     Configura el volumen
    /// </summary>
    public void SetVolume(float volume)
    {
        SetVolumeSelf(volume);
    }

    /// <summary>
    ///     Configura el volumen
    /// </summary>
    protected abstract void SetVolumeSelf(float volume);

    /// <summary>
    ///     Configura el tono
    /// </summary>
    public abstract void SetPitch(float pitch);

    /// <summary>
    ///     Configura la posición del sonido estéreo
    /// </summary>
    public abstract void SetPan(float pan);

    /// <summary>
    ///     Comienza la ejecución del sonido
    /// </summary>
    public abstract void Play();

    /// <summary>
    ///     Detiene la ejecución del sonido
    /// </summary>
    public abstract void Stop();

    /// <summary>
    ///     Actualiza el estado del sonido
    /// </summary>
    public void Update(GameContext gameContext)
    {
        // Lanza los eventos
        if (ScheduleEvents is not null)
            for (int index = ScheduleEvents.Count - 1; index >= 0; index--)
                if (ScheduleEvents[index].Start > Elapsed)
                {
                    // Lanza el evento
                    SyncFound?.Invoke(this, new EventArguments.AudioSyncEventArgs(ScheduleEvents[index].Name));
                    // Elimina el evento de la cola
                    ScheduleEvents.RemoveAt(index);
                }
        // Guarda el tiempo pasado
        Elapsed += gameContext.DeltaTime;
    }

    /// <summary>
    ///     Actualiza el sonido
    /// </summary>
    protected abstract void UpdateSelf(GameContext gameContext);

    /// <summary>
    ///     Tiempo pasado desde el origen del sonido
    /// </summary>
    public float Elapsed { get; protected set; }

    /// <summary>
    ///     Volumen
    /// </summary>
    public float Volume { get; private set; }

    /// <summary>
    ///     Eventos
    /// </summary>
    public List<ScheduleEvent>? ScheduleEvents { get; } = scheduleEvents;

	/// <summary>
	///     Estado
	/// </summary>
	public abstract SoundState State { get; }
}