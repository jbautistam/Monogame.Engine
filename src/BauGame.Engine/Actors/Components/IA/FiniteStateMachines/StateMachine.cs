namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Máquina de estados
/// </summary>
public class StateMachine(BrainComponent brain)
{
    // Vairables privadas
    private AbstractState? _current;

    /// <summary>
    ///     Arranca un estado
    /// </summary>
    public void Start(AbstractState newState)
    {
        // Finaliza el estado actual
        if (_current is not null)
            _current.End();
        // Guarda el estado nuevo
        _current = newState;
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
            AbstractState newState = _current.Update(gameContext);

                if (newState != _current)
                    Start(newState);
        }
    }

    /// <summary>
    ///     Componente con los métodos de IA
    /// </summary>
    public BrainComponent Brain { get; } = brain;
}