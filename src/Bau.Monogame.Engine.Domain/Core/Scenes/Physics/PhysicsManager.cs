using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Scenes.Physics;

/// <summary>
///		Managers de físicas
/// </summary>
public class PhysicsManager
{
	public PhysicsManager(AbstractScene scene)
	{
		Scene = scene;
		CollisionSpatialGrid = new CollisionSpatialGrid(this, 100);
	}

	/// <summary>
	///		Actualiza los datos de físicas
	/// </summary>
	public void Update(GameTime gameTime)
	{
	}

	/// <summary>
	///		Escena a la que se asocian las físicas
	/// </summary>
	public AbstractScene Scene { get; }

	/// <summary>
	///		Grid para colisiones
	/// </summary>
	public CollisionSpatialGrid CollisionSpatialGrid { get; }

	/// <summary>
	///		Relaciones entre las capas físicas
	/// </summary>
	public PhysicLayersRelation LayersRelations { get; } = new();
}