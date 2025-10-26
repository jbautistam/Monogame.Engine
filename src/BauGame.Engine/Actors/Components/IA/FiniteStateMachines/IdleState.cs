using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Estado inactivo
/// </summary>
public class IdleState(string name, PropertiesState properties) : AbstractState(name, properties)
{
    // Variables privadas
    private float _elapsedTime;

    /// <summary>
    ///     Arranca el estado
    /// </summary>
	protected override void StartState()
	{
        // Inicializa el tiempo
        _elapsedTime = 0;
        // Arranca la animación
       StartAnimation(Properties);
	}

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
	public override string? Update(GameContext gameContext)
	{
        // Incrementa el tiempo
        _elapsedTime += gameContext.DeltaTime;
        // Si se ha pasado la duración establecida
        if (Properties.Duration != 0 && _elapsedTime > Properties.Duration)
            return Properties.NextState;
        else
            return Name;
	}

    /// <summary>
    ///     Finaliza el estado
    /// </summary>
	public override void End()
	{
        // ... en este caso no hace falta nada
	}
}