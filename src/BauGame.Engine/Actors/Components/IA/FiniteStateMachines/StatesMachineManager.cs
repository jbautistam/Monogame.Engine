namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Máquina de estados
/// </summary>
public class StatesMachineManager(BrainComponent brain)
{
    // Vairables privadas
    private AbstractState? _current;

    /// <summary>
    ///     Arranca un estado
    /// </summary>
    public void Start(string? newState)
    {
        // Finaliza el estado actual
        if (_current is not null)
            _current.End();
        // Guarda el estado nuevo
        if (!string.IsNullOrWhiteSpace(newState))
            _current = States.FirstOrDefault(item => item.Name.Equals(newState, StringComparison.CurrentCultureIgnoreCase));
        else
            _current = null;
        // Entra en el nuevo estado
        if (_current is not null)
            _current.Start(this);
    }

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (_current is not null)
        {
            string? newState = _current.Update(gameContext);

                if (string.IsNullOrWhiteSpace(newState) || !newState.Equals(_current.Name, StringComparison.CurrentCultureIgnoreCase))
                    Start(newState);
        }
    }

    /// <summary>
    ///     Componente con los métodos de IA
    /// </summary>
    public BrainComponent Brain { get; } = brain;

    /// <summary>
    ///     Estados definidos en la máquina
    /// </summary>
    public List<AbstractState> States { get; } = [];
}