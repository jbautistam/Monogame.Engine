namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Máquina de estados
/// </summary>
public class StatesMachineManager(BrainComponent brain)
{
    /// <summary>
    ///     Arranca un estado
    /// </summary>
    public void Start(string? newState)
    {
        // Finaliza el estado actual
        if (Current is not null)
            Current.End();
        // Guarda el estado nuevo
        if (!string.IsNullOrWhiteSpace(newState))
            Current = States.FirstOrDefault(item => item.Name.Equals(newState, StringComparison.CurrentCultureIgnoreCase));
        else if (States.Count > 0)
            Current = States[0];
        else
            Current = null;
        // Entra en el nuevo estado
        if (Current is not null)
            Current.Start(this);
    }

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (Current is not null)
        {
            string? newState = Current.Update(gameContext);

                if (string.IsNullOrWhiteSpace(newState) || !newState.Equals(Current.Name, StringComparison.CurrentCultureIgnoreCase))
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

    /// <summary>
    ///     Estado actual
    /// </summary>
    public AbstractState? Current { get; private set; }
}