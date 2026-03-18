namespace Bau.BauEngine.Managers.Audio.EventArguments;

/// <summary>
///		Argumentos de los eventos de la transición
/// </summary>
public class AudioTransitionEventArgs(AudioTransitionEventArgs.State status, Sources.AbstractAudioSource? nextSource) : EventArgs
{
	/// <summary>
	///		Estado de la transición
	/// </summary>
	public enum State
	{
		/// <summary>Inicio de la transición</summary>
		Start,
		/// <summary>Punto medio de la transición</summary>
		MidPoint,
		/// <summary>Final de la transición</summary>
		Complete
	}

	/// <summary>
	///		Estado de la transición
	/// </summary>
	public State Status { get; } = status;

	/// <summary>
	///		Audio a ejecutar cuando se termina la transición
	/// </summary>
	public Sources.AbstractAudioSource? NextSong { get; } = nextSource;
}