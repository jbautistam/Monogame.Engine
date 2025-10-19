using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Audio;

/// <summary>
///		Clase abstracta para los managers de audio
/// </summary>
internal abstract class AbstractAudioManager(AudioManager audioManager)
{
	// Variables privadas
    private float _masterVolume = 1.0f;

	/// <summary>
	///		Actualiza el audio
	/// </summary>
	internal void Update(Managers.GameContext gameContext)
	{
		UpdateAudio(gameContext);
	}

	/// <summary>
	///		Actualiza los datos de audio
	/// </summary>
	internal abstract void UpdateAudio(Managers.GameContext gameContext);

	/// <summary>
	///		Manager principal de audio
	/// </summary>
	internal AudioManager AudioManager { get; } = audioManager;

	/// <summary>
	///		Volumen principal
	/// </summary>
    public float MasterVolume
    {
        get { return _masterVolume; }
        set 
		{ 
			_masterVolume = MathHelper.Clamp(value, 0f, 1f); 
		}
    }
}