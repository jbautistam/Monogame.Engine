namespace UI.CharactersEngine.Sequences;

/// <summary>
///		Manager de las secuencias cinemáticas
/// </summary>
public class CinematicManager(CharacterLayer characterLayer)
{
    // Eventos públicos
    public event EventHandler<EventArguments.SequenceProgressEventArgs>? StartSequence;
	public event EventHandler<EventArguments.SequenceProgressEventArgs>? EndSequence;
	// Variables privadas
	private List<CinematicSequence> _sequences = [];
	private List<CinematicSequence> _sequencesToRemove = [];

	/// <summary>
	///		Añade una secuencia
	/// </summary>
	public void Add(CinematicSequence sequence)
	{
		// Añade la secuencia
		_sequences.Add(sequence);
		// La inicializa
		sequence.Start(this);
		// y lanza el evento de inicio
		StartSequence?.Invoke(this, new EventArguments.SequenceProgressEventArgs(sequence));
	}

	/// <summary>
	///		Actualiza las secuencias
	/// </summary>
	public void Update(float deltaTime)
	{
		// Elimina las secuencias antiguas
		foreach (CinematicSequence sequence in _sequencesToRemove)
		{
			// Lanza el evento de fin de la secuencia
			EndSequence?.Invoke(this, new EventArguments.SequenceProgressEventArgs(sequence));
			// Elimina la secuencia de la lista
			_sequences.Remove(sequence);
		}
		// Elimina la lista de secuencias que se deben eliminar
		_sequencesToRemove.Clear();
		// Actualiza las secuencias
		foreach (CinematicSequence sequence in _sequences)
			if (sequence.Enabled)
				sequence.Update(deltaTime);
			else
				_sequencesToRemove.Add(sequence);
	}

	/// <summary>
	///		Capa de los personajes
	/// </summary>
	public CharacterLayer CharacterLayer { get; } = characterLayer;
}