namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences.EventArguments;

/// <summary>
///		Argumentos de inicio o fin de un <see cref="CinematicSequenceList"/>
/// </summary>
public class SequenceProgressEventArgs(CinematicSequenceList sequence) : EventArgs
{
	/// <summary>
	///		Secuencia
	/// </summary>
	public CinematicSequenceList Sequence { get; } = sequence;
}