namespace UI.CharactersEngine.Sequences.EventArguments;

/// <summary>
///		Argumentos de inicio o fin de un <see cref="CinematicSequence"/>
/// </summary>
public class SequenceProgressEventArgs(CinematicSequence sequence) : EventArgs
{
	/// <summary>
	///		Secuencia
	/// </summary>
	public CinematicSequence Sequence { get; } = sequence;
}