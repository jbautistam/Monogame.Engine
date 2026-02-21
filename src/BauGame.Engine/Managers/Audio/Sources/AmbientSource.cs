using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

/// <summary>
///     Ambiente procedural (lluvia, viento, etc.)
/// </summary>
public class AmbientSource(List<SfxSource> variations, float minInterval, float maxInterval, List<ScheduleEvent>? scheduleEvents) : AbstractAudioSource(scheduleEvents)
{
    // Variables privadas
    private Random _random = new();
    private double _nextPlayTime;    // Cuándo tocar el siguiente
    private SfxSource? _current;

    /// <summary>
    ///     Actualiza el sonido
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
    {
        if (State != SoundState.Stopped)
        {
            // Si tiene que reproducir la siguiente variación
            if (Elapsed >= _nextPlayTime || _current is null)
            {
                // Ejecuta una variación aleatoriamente
                PlayRandomVariation();
                // Calcular siguiente tiempo (aleatorio entre min y max) e inicializa el tiempo pasado
                _nextPlayTime = MinInterval + (float) (_random.NextDouble() * (MaxInterval - MinInterval));
                Elapsed = -gameContext.DeltaTime;
            }
        }
    }
    
    /// <summary>
    ///     Ejecuta una variación aleatoriamente
    /// </summary>
    private void PlayRandomVariation()
    {
        // Detiene el sonido actual
        _current?.Stop();
        // Obtiene el siguiente sonido
        _current = Variations[_random.Next(Variations.Count)];
        // Inicializa los valores de la instancia
        _current.SetVolume(Volume);
        _current.SetPitch((float) (_random.NextDouble() * 0.2f - 0.1f)); // -10% a + 10%
        _current.SetPan((float) (_random.NextDouble() * 2f - 1f)); // posición del sonido estéreo
        // Ejecuta el efecto
        _current.Play();
    }

    /// <summary>
    ///     Cambia el volumen
    /// </summary>
    protected override void SetVolumeSelf(float volume)
    {
        _current?.SetVolume(volume);
    }

    /// <summary>
    ///     Cambia el tono
    /// </summary>
    public override void SetPitch(float pitch)
    {
        _current?.SetPitch(pitch);
    }

    /// <summary>
    ///     Cambia el envolvente
    /// </summary>
    public override void SetPan(float pan)
    {
        _current?.SetPan(pan);
    }
    
    /// <summary>
    ///     Toca la música
    /// </summary>
    public override void Play()
    {
        _current?.Play();
    }

    /// <summary>
    ///     Detiene la música
    /// </summary>
    public override void Stop()
    {
        _current?.Stop();
    }

    /// <summary>
    ///     Variaciones de sonido ambiente
    /// </summary>
    private List<SfxSource> Variations { get; } = variations;

    /// <summary>
    ///     Tiempo mínimo que se debe ejecutar un sonido ambiente
    /// </summary>
    private float MinInterval { get; } = minInterval;

    /// <summary>
    ///     Tiempo máximo para pasar al siguiente sonido ambiente
    /// </summary>
    private float MaxInterval { get; } = maxInterval;

    /// <summary>
    ///     Estado del sonido
    /// </summary>
    public override SoundState State => _current?.State ?? SoundState.Stopped;
}