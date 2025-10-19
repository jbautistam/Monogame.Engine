using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Bau.Libraries.BauGame.Engine.Scenes.Audio;

/// <summary>
///     Manager para efectos de sonido
/// </summary>
internal class SoundManager(AudioManager audioManager) : AbstractAudioManager(audioManager)
{
    // Variables privadas
    private Dictionary<string, SoundEffectInstance> _soundsPool = new(StringComparer.CurrentCultureIgnoreCase);

    /// <summary>
    ///     Inicializa el manager
    /// </summary>
    internal void Initialize()
    {
    }

    /// <summary>
    ///     Reproduce un efecto de sonido una sola vez.
    /// </summary>
    public void Play(string name, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
    {
        if (_soundsPool.TryGetValue(name, out SoundEffectInstance? cached))
        {
            if (cached.State == SoundState.Stopped)
                PlayEffect(cached, volume, pitch, pan);
        }
        else
        {
            SoundEffect? effect = GameEngine.Instance.ResourcesManager.GlobalContentManager.LoadAsset<SoundEffect>(name);

                if (effect is not null)
                {
                    SoundEffectInstance instance = effect.CreateInstance();

                        // Añade el efecto al diccionario
                        _soundsPool.Add(name, instance);
                        // Y lo reproduce
                        PlayEffect(instance, volume, pitch, pan);
                }
        }
    }

    /// <summary>
    ///    Reproduce un efecto de sonido
    /// </summary>
	private void PlayEffect(SoundEffectInstance soundEffectInstance, float volume, float pitch, float pan)
	{
        soundEffectInstance.Volume = MathHelper.Clamp(volume * MasterVolume, 0f, 1f);
        soundEffectInstance.Pitch = MathHelper.Clamp(pitch, -1f, 1f);
        soundEffectInstance.Pan = MathHelper.Clamp(pan, -1f, 1f);
        soundEffectInstance.Play();
	}

	/// <summary>
	///     Actualiza el audio
	/// </summary>
	internal override void UpdateAudio(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Detiene todos los efectos de sonido en reproducción
    /// </summary>
    internal void Stop()
    {
        foreach (SoundEffectInstance instance in _soundsPool.Values)
            instance.Stop();
    }

    /// <summary>
    ///     Detiene momentáneamente la reproducción de efectos
    /// </summary>
    internal void Pause()
    {
        foreach (SoundEffectInstance instance in _soundsPool.Values)
            instance.Pause();
    }

    /// <summary>
    ///     Continúa la reproducción de efectos
    /// </summary>
    internal void Resume()
    {
        foreach (SoundEffectInstance instance in _soundsPool.Values)
            instance.Resume();
    }
}