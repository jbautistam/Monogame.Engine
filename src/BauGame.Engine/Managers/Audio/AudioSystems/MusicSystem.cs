using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.AudioSystems;

/// <summary>
///     Sistema de audio para música
/// </summary>
public class MusicSystem(AudioManager audioEngine) : AbstractAudioSystem(audioEngine)
{
    // Variables privadas
    private Sources.MusicSource? _currentSource;
    
    /// <summary>
    ///     Inicializa el sistema
    /// </summary>
	public override void Initialize()
	{
	}

    /// <summary>
    ///     Encola música con una transición
    /// </summary>
    public void Enqueue(Song song, AudioManager.TransitionType transition, float duration)
    {
        float volume = AudioManager.GetVolume(AudioManager.AudioDefinitionType.Music, 1f);

            // Encola la nueva música
            TransitionsQueue.Enqueue(transition, _currentSource, new Sources.MusicSource(song, null), volume, duration);
    }

    /// <summary>
    ///     Actualiza el sistema
    /// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
	}

    /// <summary>
    ///     Detine momentáneamente el sonido
    /// </summary>
	public override void Pause()
	{
		_currentSource?.Stop();
	}

    /// <summary>
    ///     Continúa con el sonido
    /// </summary>
	public override void Resume()
	{
		_currentSource?.Play();
	}

    /// <summary>
    ///     Detiene el sonido
    /// </summary>
	public override void Stop()
	{
		_currentSource?.Stop();
	}
    
    /// <summary>
    ///     Estado del sonido
    /// </summary>
	public override SoundState State => _currentSource?.State ?? SoundState.Stopped;
}
