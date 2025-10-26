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
        // Inicializa el tiempo
        _elapsedTime = 0;
        // Arranca la animación
       StartAnimation(Properties);
	}

    /// <summary>
    ///     Actualiza el estado
    /// </summary>
	public override string? Update(Managers.GameContext gameContext)
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
            //TODO: aquí debería mover al NPC
            //npc.Velocity = _direction * npc.Speed;
            // Devuelve el nombre actual
            return Name;
        }
	}

    /// <summary>
    ///     Calcula la siguiente dirección
    /// </summary>
    private void ComputeNewDirection()
    {
        // Si ha superado el tiempo máximo de caminar en una dirección, la cambia
        if (_walkingTime > _maxWalkToDirectionTime)
        {
            float angle = MathHelper.ToRadians(Random.Shared.Next(0, 360));

                // Cambia la dirección
                _direction = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
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