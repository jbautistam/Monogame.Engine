using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences;

/// <summary>
///     Secuencia cinemática
/// </summary>
public class CinematicSequence(CinematicSequenceList cinematicSequenceList) : Bau.BauEngine.Entities.Common.Collections.ISecureListItem
{
    // Variables privadas
    private float _elapsedTime = 0;

    /// <summary>
    ///     Arranca la lista de comandos
    /// </summary>
	public void Start()
	{
        _elapsedTime = 0;
        Enabled = true;
	}

    /// <summary>
    ///     Actualiza la lista de comandos
    /// </summary>
    public void Update(GameContext gameContext)
    {
        // Ejecuta los comandos
        if (Enabled)
            foreach (Commands.AbstractSequenceCommand command in Commands)
            {
                // Activa el comando si es necesario
                if (command.MustStart(_elapsedTime))
                {
                    AbstractActor? actor = CinematicSequenceList.CharacterManager.GetActor(command.ActorId);

                        // Inicia el comando si encuentra el actor o lo desactiva
                        if (actor is AbstractActorDrawable character)
                            command.Start(character);
                        else
                            command.Enabled = false;
                }
                // Ejecuta el comando si es necesario
                if (command.Enabled && command.IsActive())
                    command.Apply(gameContext);
            }
        // Actualiza el tiempo pasado
        _elapsedTime += gameContext.DeltaTime;
        // Si ha terminado, lo marca como inactivo
        Enabled = !HasEnd(_elapsedTime);
    }

    /// <summary>
    ///     Comprueba si se ha terminado la ejecución de todos los comandos
    /// </summary>
    private bool HasEnd(float elapsedTime)
    {
        float totalTime = 0;

            // Calcula el tiempo total de todos los comandos
            foreach (Commands.AbstractSequenceCommand command in Commands)
                totalTime += command.StartTime + command.Duration;
            // Se ha terminado si se ha superado el tiempo de ejecución de los comandos
            return elapsedTime > totalTime;
    }

    /// <summary>
    ///     Finaliza la secuencia
    /// </summary>
	public void End(GameContext gameContext)
	{
        Enabled = false;
	}

    /// <summary>
    ///     Id de la secuencia
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    ///     Lista de secuencias cinemáticas
    /// </summary>
    public CinematicSequenceList CinematicSequenceList { get; } = cinematicSequenceList;

    /// <summary>
    ///     Comandos
    /// </summary>
    public List<Commands.AbstractSequenceCommand> Commands { get; } = [];

    /// <summary>
    ///     Indica si la secuencia está activa
    /// </summary>
    public bool Enabled { get; private set; }
}
