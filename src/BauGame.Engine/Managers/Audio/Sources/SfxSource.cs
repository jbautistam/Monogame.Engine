using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

/// <summary>
///     Origen de audio con un efecto de sonido
/// </summary>
public class SfxSource(SoundEffectInstance effect, List<ScheduleEvent>? scheduleEvents) : AbstractAudioSource(scheduleEvents)
{
    /// <summary>
    ///     Actualiza el sonido
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
    {
    }

    /// <summary>
    ///     Configura el volumen
    /// </summary>
    protected override void SetVolumeSelf(float volume)
    {
        if (Effect is not null)
            Effect.Volume = volume;
    }

    /// <summary>
    ///     Cambia el tono
    /// </summary>
    public override void SetPitch(float pitch)
    {
        if (Effect is not null)
            Effect.Pitch = pitch;
    }

    /// <summary>
    ///     Cambia el envolvente
    /// </summary>
    public override void SetPan(float pan)
    {
        if (Effect is not null)
            Effect.Pan = pan;
    }

    /// <summary>
    ///     Ejecuta el sonido
    /// </summary>
    public override void Play()
    {
        Effect?.Play();
    }

    /// <summary>
    ///     Detiene el sonido
    /// </summary>
    public override void Stop() 
    {
        Effect?.Stop();
    }
    
    /// <summary>
    ///     Efecto de sonido
    /// </summary>
    public SoundEffectInstance? Effect { get; } = effect;

    /// <summary>
    ///     Estado del sonido
    /// </summary>
    public override SoundState State => Effect?.State ?? SoundState.Stopped;
}
