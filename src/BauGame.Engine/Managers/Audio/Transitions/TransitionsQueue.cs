using Bau.Libraries.BauGame.Engine.Managers.Audio.Sources;

namespace Bau.Libraries.BauGame.Engine.Managers.Audio.Transitions;

/// <summary>
///		Cola de transiciones
/// </summary>
public class TransitionsQueue(AudioSystems.AudioSystem audioSystem)
{
	/// <summary>
	///		Encola una transición
	/// </summary>
	public void Enqueue(AudioManager.TransitionType type, AbstractAudioSource? source, AbstractAudioSource? target, float volume, float duration)
	{
		switch (type)
		{
			case AudioManager.TransitionType.None:
					EnqueueCut(target, volume, 0.01f);
				break;
			case AudioManager.TransitionType.Cut:
					EnqueueCut(target, volume, duration);
				break;
			case AudioManager.TransitionType.Fade:
					EnqueueFadeInOut(target, volume, duration);
				break;
			case AudioManager.TransitionType.CrossFade:
					EnqueueCrossFade(target, volume, duration);
				break;
		}
	}

	/// <summary>
	///		Añade una transición de corte
	/// </summary>
	public void EnqueueCut(AbstractAudioSource? target, float volume, float duration)
	{
		Transitions.Add(new CutAudioTransition(this, target, volume, duration));
	}

	/// <summary>
	///		Añade una transición fade de entrada de un sonido
	/// </summary>
	public void EnqueueFadeIn(AbstractAudioSource target, float volume, float duration)
	{
		Transitions.Add(new FadeInTransition(this, target, volume, duration));
	}

	/// <summary>
	///		Añade una transición fade de salida de un sonido
	/// </summary>
	public void EnqueueFadeOut(float volume, float duration)
	{
		if (AudioSystem.Current is not null)
			Transitions.Add(new FadeOutTransition(this, volume, duration));
	}

	/// <summary>
	///		Añade una transición fade de entrada de un sonido
	/// </summary>
	public void EnqueueFadeInOut(AbstractAudioSource? target, float volume, float duration)
	{
		if (AudioSystem.Current is not null)
			EnqueueFadeOut(volume, duration);
		if (target is not null)
			EnqueueFadeIn(target, volume, duration);
	}

	/// <summary>
	///		Añade una transición de entrada / salida de dos sonidos
	/// </summary>
	public void EnqueueCrossFade(AbstractAudioSource? target, float volume, float duration)
	{
		Transitions.Add(new CrossFadeTransition(this, target, volume, duration));
	}

	/// <summary>
	///		Actualiza la transición
	/// </summary>
	public void Update(GameContext gameContext)
	{
		if (Transitions.Count > 0)
		{
			// Actualiza la transición
			Transitions[0].Update(gameContext);
			// Elimina la transición si ha terminado
			if (!Transitions[0].IsActive)
				Transitions.RemoveAt(0);
		}
	}

	/// <summary>
	///		Sistema de audio
	/// </summary>
	public AudioSystems.AudioSystem AudioSystem { get; } = audioSystem;

	/// <summary>
	///		Transiciones
	/// </summary>
	public List<AbstractAudioTransition> Transitions { get; } = [];
}
