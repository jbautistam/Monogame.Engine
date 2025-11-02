using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Estado de caminar sin rumbo
/// </summary>
public class WalkingState(string name, PropertiesState properties) : AbstractState(name, properties)
{
    // Variables privadas
    private float _elapsedTime, _walkingTime, _maxWalkToDirectionTime;
    private Vector2 _direction;

    /// <summary>
    ///     Arranca el estado
    /// </summary>
	protected override void StartState()
	{
        _elapsedTime = 0;
	}

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
	protected override string? UpdateState(Managers.GameContext gameContext)
	{
        // Incrementa el tiempo
        _elapsedTime += gameContext.DeltaTime;
        _walkingTime += gameContext.DeltaTime;
        // Mueve hacia una nueva dirección si es necesario
        ComputeNewDirection();
        // Si se ha pasado la duración establecida
        if (Properties.Duration != 0 && _elapsedTime > Properties.Duration)
            return Properties.NextState;
        else
        {
            // Cambia la velocidad
            Speed = _direction * Properties.SpeedMaximum;
            // Devuelve el nombre actual
            return Name;
        }
	}

    /// <summary>
    ///     Calcula la siguiente dirección
    /// </summary>
    private void ComputeNewDirection()
    {
        if (_walkingTime > _maxWalkToDirectionTime)
        {
            // Cambia la dirección
            _direction = Tools.Randomizer.GetRandomDirection();
            // Crea un nuevo tiempo máximo
            _maxWalkToDirectionTime = Random.Shared.Next(3, 8);
            // Inicializa el tiempo que lleva caminando
            _walkingTime = 0;
        }
    }

    /// <summary>
    ///     Finaliza el estado
    /// </summary>
	public override void End()
	{
        // ... en este caso no hace falta nada
	}
}