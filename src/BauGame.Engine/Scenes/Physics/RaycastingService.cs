using Bau.Libraries.BauGame.Engine.Actors;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics;

/// <summary>
///		Métodos para raycasting
/// </summary>
public class RaycastingService(PhysicsManager physicsManager)
{
	/// <summary>
	///		Obtiene las colisiones con un actor sobre una dirección
	/// </summary>
	public List<KinematicCollisionModel> Raycast(AbstractActor abstractActor, Vector2 direction, float distance, bool stopAtFirst)
	{
		return [];
	}

	/// <summary>
	///		Manager de físicas
	/// </summary>
	public PhysicsManager PhysicsManager { get; } = physicsManager;
}