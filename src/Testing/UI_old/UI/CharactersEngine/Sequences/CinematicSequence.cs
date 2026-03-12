using UI.CharactersEngine.Sequences.Commands;

namespace UI.CharactersEngine.Sequences;

/// <summary>
///     Secuencia cinemática
/// </summary>
public class CinematicSequence
{
    // Variables privadas
    private float _elapsedTime = 0;

    /// <summary>
    ///     Arranca la secuencia
    /// </summary>
    public void Start(CinematicManager manager)
    {
        CinematicManager = manager;
    }

    /// <summary>
    ///     Actualiza la secuencia
    /// </summary>
    public void Update(float deltaTime)
    {
        // Ejecuta los comandos
        if (CinematicManager is not null)
        {
            // Ejecuta los comandos
            foreach (AbstractSequenceCommand command in Commands)
                if (command.IsActive(_elapsedTime))
                {
                    Actor? actor = CinematicManager.CharacterLayer.GetActor(command.ActorId);

                        if (actor is not null)
                            command.Apply(actor, _elapsedTime);
                }
            // Actualiza el tiempo pasado
            _elapsedTime += deltaTime;
        }
        // Desactiva
        Enabled = !HasEnd(_elapsedTime);
    }

    /// <summary>
    ///     Comprueba si se ha terminado la ejecución de todos los comandos
    /// </summary>
    private bool HasEnd(float elapsedTime)
    {
        // Comprueba si alguno de los comandos no ha terminado
        foreach (AbstractSequenceCommand command in Commands)
            if (command.EndTime > elapsedTime)
                return false;
        // Si ha llegado hasta aquí es porque todos han terminado
        return true;
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
    public bool Enabled { get; private set; }

    /// <summary>
    ///     Manager
    /// </summary>
    public CinematicManager? CinematicManager { get; private set; }
}
