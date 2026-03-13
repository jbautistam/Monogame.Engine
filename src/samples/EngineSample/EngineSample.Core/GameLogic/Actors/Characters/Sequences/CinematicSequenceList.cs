using Bau.Libraries.BauGame.Engine.Managers;
using EngineSample.Core.GameLogic.Actors.Characters.Sequences.Commands;

namespace EngineSample.Core.GameLogic.Actors.Characters.Sequences;

/// <summary>
///     Secuencia cinemática
/// </summary>
public class CinematicSequenceList(CharacterManager characterManager) : Bau.Libraries.BauGame.Engine.Entities.Common.Collections.SecureList<CinematicSequence>
{
    /// <summary>
    ///     Añade una secuencia con una lista de comandos
    /// </summary>
    public void Add(List<AbstractSequenceCommand> commands)
    {
        CinematicSequence sequence = new(this);

            // Añade los comandos
            sequence.Commands.AddRange(commands);
            // Añade la secuencia a la colección
            Add(sequence);
    }

    /// <summary>
    ///     Arranca la secuencia
    /// </summary>
	public void Start()
	{
	}

    /// <summary>
    ///     Actualiza la secuencia
    /// </summary>
    protected override void UpdateSelf(GameContext gameContext)
    {
        foreach (CinematicSequence sequence in Enumerate())
            if (sequence.Enabled)
                sequence.Update(gameContext);
            else
                MarkToDestroy(sequence, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    ///     Trata la creación de un elemento
    /// </summary>
	protected override void Added(CinematicSequence item)
	{
	}

    /// <summary>
    ///     Trata el borrado de un elemento
    /// </summary>
	protected override void Removed(CinematicSequence item)
	{
	}

    /// <summary>
    ///     Finaliza la secuencia
    /// </summary>
	public void End(GameContext gameContext)
	{
	}

    /// <summary>
    ///     Id de la secuencia
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Comandos
    /// </summary>
    public List<AbstractSequenceCommand> Commands { get; } = [];

    /// <summary>
    ///     Indica si la secuencia está activa
    /// </summary>
    public bool Enabled => Count > 0;

    /// <summary>
    ///     Manager
    /// </summary>
    public CharacterManager CharacterManager { get; } = characterManager;
}
