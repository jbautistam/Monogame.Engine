using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

/// <summary>
///     Música
/// </summary>
public class MusicSource(Song song, List<ScheduleEvent>? scheduleEvents) : AbstractAudioSource(scheduleEvents)
{
    /// <summary>
    ///     Actualiza el sonido
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
    {
    }

    /// <summary>
    ///     Cambia el volumen
    /// </summary>
    protected override void SetVolumeSelf(float volume)
    {
        MediaPlayer.Volume = volume;
    }

    /// <summary>
    ///     Cambia el tono
    /// </summary>
    public override void SetPitch(float pitch)
    {
        // MediaPlayer.SetPan(pitch);
    }

    /// <summary>
    ///     Cambia el envolvente
    /// </summary>
    public override void SetPan(float pan)
    {
       // MediaPlayer.SetPan(pan);
    }

    /// <summary>
    ///     Toca la música
    /// </summary>
    public override void Play()
    {
        MediaPlayer.Play(Song);
    }

    /// <summary>
    ///     Detiene la música
    /// </summary>
    public override void Stop()
    {
        if (State == SoundState.Playing)
            MediaPlayer.Stop();
    }

    /// <summary>
    ///     Música
    /// </summary>
    public Song? Song { get; } = song;

    /// <summary>
    ///     Estado
    /// </summary>
    public override SoundState State => MediaPlayer.State == MediaState.Playing ? SoundState.Playing : SoundState.Stopped;
}