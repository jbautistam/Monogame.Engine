using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

/// <summary>
///		Base para los comportamientos de motor
/// </summary>
public abstract class AbstractSteeringBehavior
{
	/// <summary>
	///		Calcula el movimiento
	/// </summary>
	public abstract Vector2 Calculate(AgentSteeringManager agentSteeringManager);

	/// <summary>
	///		Busca un punto destino
	/// </summary>
    protected Vector2 AddressTo(AgentSteeringManager agentSteeringManager, Vector2 target, float minimuDistance)
    {
        Vector2 desired = target - agentSteeringManager.Position;

			// Si está cerca, no cambia la fuerza
			if (desired.Length() < minimuDistance) 
				return Vector2.Zero;
			else
			{
				// Normaliza la velocidad
				desired = Vector2.Normalize(desired) * agentSteeringManager.MaxSpeed;
				// Devuelve el valor que nos acerca al destino
				return desired - agentSteeringManager.Velocity;
			}
    }

	/// <summary>
	///		Obtiene el vector adecuado para separarse / huir de un punto
	/// </summary>
    protected Vector2 FleeFrom(AgentSteeringManager agentSteeringManager, Vector2 target, float maximumDistance) 
	{
		return AddressTo(agentSteeringManager, agentSteeringManager.Position - target, maximumDistance);
	}
}
