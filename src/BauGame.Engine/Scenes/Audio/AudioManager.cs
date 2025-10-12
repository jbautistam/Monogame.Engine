using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Audio;

/// <summary>
///     Manager de audio
/// </summary>
public class AudioManager
{
    /// <summary>
    ///     Canal de audio
    /// </summary>
    public enum AudioChannelType
    {
        /// <summary>Música</summary>
        Music,
        /// <summary>Efectos de sonido</summary>
        Effects
    }

    public AudioManager(AbstractScene scene)
    {
        Scene = scene;
        SoundManager = new SoundManager(this);
        MusicManager = new MusicManager(this);
    }

    /// <summary>
    ///     Inicializa el audio
    /// </summary>
    public void Initialize()
    {
        SoundManager.Initialize();
        MusicManager.Initialize();
    }

    /// <summary>
    ///     Actualiza el audio
    /// </summary>
    public void Update(GameTime gameTime)
    {
        SoundManager.Update(gameTime);
        MusicManager.Update(gameTime);
    }

    /// <summary>
    ///     Actualiza la configuración
    /// </summary>
    public void UpdateSettings(float masterMusicVolume, float masterSoundVolume)
    {
        MusicManager.MasterVolume = masterMusicVolume;
        SoundManager.MasterVolume = masterSoundVolume;
    }

    /// <summary>
    ///     Reproduce una canción
    /// </summary>
    public void PlaySong(string name)
    {
        MusicManager.PlaySong(name);
    }

    /// <summary>
    ///     Reproduce un efecto de sonido
    /// </summary>
	public void PlayEffect(string name, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
	{
        SoundManager.Play(name, volume, pitch, pan);
	}

    /// <summary>
    ///     Detiene momentáneamente la reproducción de música
    /// </summary>
    internal void Pause()
    {
        MusicManager.Pause();
        SoundManager.Pause();
    }

    /// <summary>
    ///     Continúa la reproducción de música
    /// </summary>
    internal void Resume()
    {
        MusicManager.Resume();
        SoundManager.Resume();
    }

    /// <summary>
    ///     Detiene la música
    /// </summary>
    public void Stop()
    {
        MusicManager.Stop();
        SoundManager.Stop();
    }

    /// <summary>
    ///     Comprueba si se está reproduciendo música
    /// </summary>
	public bool IsPlayingMusic() => MusicManager.IsPlaying;

	/// <summary>
	///     Escena a la que se asocia el audio
	/// </summary>
	public AbstractScene Scene { get; }

    /// <summary>
    ///     Manager de efectos de sonido
    /// </summary>
    private SoundManager SoundManager { get; }

    /// <summary>
    ///     Manager de música
    /// </summary>
    private MusicManager MusicManager { get; }
}