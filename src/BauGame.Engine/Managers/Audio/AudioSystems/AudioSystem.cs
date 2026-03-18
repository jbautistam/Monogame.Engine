namespace Bau.BauEngine.Managers.Audio.AudioSystems;

/// <summary>
///		Clase abstracta para los diferentes sistemas de audio
/// </summary>
public class AudioSystem
{
	public AudioSystem(AudioManager audioManager)
	{
		AudioManager = audioManager;
		TransitionsQueue = new Transitions.TransitionsQueue(this);
	}

	/// <summary>
	///		Actualiza el sistema de audio
	/// </summary>
	public void Update(GameContext gameContext)
	{
		// Actualiza las transiciones
		TransitionsQueue.Update(gameContext);
		// Si no hay ninguna transición, actualiza el sonido actual
		if (TransitionsQueue.Transitions.Count == 0)
			Current?.Update(gameContext);
	}

	/// <summary>
	///		Manager principal de audio
	/// </summary>
	public AudioManager AudioManager { get; }

	/// <summary>
	///		Sonido actual
	/// </summary>
	public Sources.AbstractAudioSource? Current { get; set; }

	/// <summary>
	///		Cola de transiciones
	/// </summary>
	public Transitions.TransitionsQueue TransitionsQueue { get; }
}