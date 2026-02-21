using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio;

/// <summary>
///     Manager de audio
/// </summary>
public class AudioManager
{
    /// <summary>
    ///     Tipo de sistema de audio
    /// </summary>
    public enum AudioDefinitionType
    {
        /// <summary>Música</summary>
        Music,
        /// <summary>Efectos</summary>
        Sfx,
        /// <summary>Voz</summary>
        Voice,
        /// <summary>Sonido ambiente</summary>
        Ambience
    }
    /// <summary>
    ///     Tipo de transición
    /// </summary>
    public enum TransitionType
    {
        /// <summary>Sin transición</summary>
        None,
        /// <summary>Corte de la música actual, espera y comienzo de la siguiente</summary>
        Cut,
        /// <summary>Fade de salida / entrada de sonido</summary>
        Fade,
        /// <summary>Fade de salida / entrada de sonido al mismo tiempo</summary>
        CrossFade
    }
    // Registros privados
    private record AudioSystem(AudioDefinitionType Type, AudioSystems.AudioSystem System);
    // Variables privadas
    private List<AudioSystem> _audioSystems = [];

    public AudioManager(EngineManager engineManager)
    {
        EngineManager = engineManager;
        _audioSystems.Add(new AudioSystem(AudioDefinitionType.Music, new AudioSystems.AudioSystem(this)));
        _audioSystems.Add(new AudioSystem(AudioDefinitionType.Sfx, new AudioSystems.AudioSystem(this)));
        _audioSystems.Add(new AudioSystem(AudioDefinitionType.Ambience, new AudioSystems.AudioSystem(this)));
        _audioSystems.Add(new AudioSystem(AudioDefinitionType.Voice, new AudioSystems.AudioSystem(this)));
    }

    /// <summary>
    ///     Inicializa el audio
    /// </summary>
    public void Initialize()
    {
    }

    /// <summary>
    ///     Actualiza el audio
    /// </summary>
    public void Update(GameContext gameContext)
    {
        foreach (AudioSystem audioSystem in _audioSystems)
            audioSystem.System.Update(gameContext);
    }

    /// <summary>
    ///     Obtiene un tipo de motor de sonido
    /// </summary>
    private AudioSystems.AudioSystem? GetAudioSystem(AudioDefinitionType type) => _audioSystems.FirstOrDefault(item => item.Type == type)?.System;

    /// <summary>
    ///     Reproduce una canción
    /// </summary>
    public void PlaySong(string name, TransitionType transition, float duration)
    {
        Song? song = EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<Song>(name);

            if (song is not null)
                Enqueue(AudioDefinitionType.Music, transition, new Sources.MusicSource(song, null), duration);
    }

    /// <summary>
    ///     Reproduce un efecto de sonido
    /// </summary>
	public void PlayEffect(string name, float pitch = 0.0f, float pan = 0.0f)
	{
        SoundEffectInstance? effect = EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<SoundEffectInstance>(name);

            if (effect is not null)
            {
                // Ajusta los valores
                effect.Pitch = pitch;
                effect.Pan = pan;
                // Encola el sonido
                Enqueue(AudioDefinitionType.Sfx, TransitionType.None, new Sources.SfxSource(effect, null), 0);
            }
	}

    /// <summary>
    ///     Reproduce una voz
    /// </summary>
	public void PlayVoice(string name, float pitch = 0.0f, float pan = 0.0f, List<Sources.ScheduleEvent>? scheduleEvents = null)
	{
        SoundEffectInstance? effect = EngineManager.ResourcesManager.GlobalContentManager.LoadAsset<SoundEffectInstance>(name);

            if (effect is not null)
            {
                // Ajusta los valores
                effect.Pitch = pitch;
                effect.Pan = pan;
                // Encola el sonido
                Enqueue(AudioDefinitionType.Sfx, TransitionType.None, new Sources.VoiceSource(effect, scheduleEvents), 0);
            }
	}

    /// <summary>
    ///     Encola un sonido
    /// </summary>
	private void Enqueue(AudioDefinitionType type, TransitionType transition, Sources.AbstractAudioSource source, float duration)
	{
        AudioSystems.AudioSystem? system = GetAudioSystem(type);

            if (system is not null)
                system.TransitionsQueue.Enqueue(TransitionType.None, system.Current, source, GetVolume(type, 1), 0);
	}

    /// <summary>
    ///     Detiene momentáneamente la reproducción de música
    /// </summary>
    public void Pause()
    {
        foreach (AudioSystem audioSystem in _audioSystems)
            audioSystem.System.Current?.Stop();
    }

    /// <summary>
    ///     Continúa la reproducción de música
    /// </summary>
    public void Resume()
    {
        foreach (AudioSystem audioSystem in _audioSystems)
            audioSystem.System.Current?.Play();
    }

    /// <summary>
    ///     Detiene la música
    /// </summary>
    public void Stop()
    {
        foreach (AudioSystem audioSystem in _audioSystems)
            audioSystem.System.Current?.Stop();
    }

    /// <summary>
    ///     Comprueba si se está reproduciendo música
    /// </summary>
	public bool IsPlayingMusic()
    {
        AudioSystems.AudioSystem? system = GetAudioSystem(AudioDefinitionType.Music);

            if (system is not null && system.Current is not null)
                return system.Current.State == SoundState.Playing;
            else
                return false;
    }

    /// <summary>
    ///     Obtiene el volumen aplicando la configuración
    /// </summary>
	internal float GetVolume(AudioDefinitionType music, float baseVolume) => EngineManager.EngineSettings.AudioSettings.CalculateVolume(music, baseVolume);

	/// <summary>
	///     Manager del motor
	/// </summary>
	public EngineManager EngineManager { get; }
}