namespace Bau.Libraries.BauGame.Engine.Scenes.Physics.Mapping;

/// <summary>
///		Manager del mapa
/// </summary>
public class MapManager
{
	public MapManager(PhysicsManager physicsManager)
	{
		PhysicsManager = physicsManager;
		GridMap = new GridMap(physicsManager.Scene.WorldDefinition);
		CollisionSpatialGrid = new CollisionSpatialGrid(this, physicsManager.Scene.WorldDefinition.CellWidth);
	}

	/// <summary>
	///		Manager de físicas
	/// </summary>
	public PhysicsManager PhysicsManager { get; }

	/// <summary>
	///		Mapa de celdas
	/// </summary>
	public GridMap GridMap { get; }

	/// <summary>
	///		Grid para colisiones
	/// </summary>
	public CollisionSpatialGrid CollisionSpatialGrid { get; }
}
