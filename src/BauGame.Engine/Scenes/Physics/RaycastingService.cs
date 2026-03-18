using Bau.BauEngine.Actors;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Scenes.Physics;

/// <summary>
///		Métodos para raycasting
/// </summary>
public class RaycastingService(PhysicsManager physicsManager)
{
	/// <summary>
	///		Obtiene las colisiones con un actor sobre una dirección
	/// </summary>
	public List<KinematicCollisionModel> Raycast(AbstractActorDrawable abstractActor, Vector2 direction, float distance, bool stopAtFirst)
	{
		return [];
	}

	/// <summary>
	///		Manager de físicas
	/// </summary>
	public PhysicsManager PhysicsManager { get; } = physicsManager;
}