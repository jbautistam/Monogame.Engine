﻿using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Physics;

/// <summary>
///		Managers de físicas
/// </summary>
public class PhysicsManager
{
	public PhysicsManager(AbstractScene scene)
	{
		Scene = scene;
		CollisionSpatialGrid = new CollisionSpatialGrid(this, 100);
		RaycastingService = new RaycastingService(this);
	}

	/// <summary>
	///		Actualiza los datos de físicas
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Escena a la que se asocian las físicas
	/// </summary>
	public AbstractScene Scene { get; }

	/// <summary>
	///		Fuerza de gravedad (en unidades del mundo)
	/// </summary>
	public Vector2 WorldGravity { get; set; } = new Vector2(0, 981f);

	/// <summary>
	///		Grid para colisiones
	/// </summary>
	public CollisionSpatialGrid CollisionSpatialGrid { get; }

	/// <summary>
	///		Relaciones entre las capas físicas
	/// </summary>
	public PhysicLayersRelation LayersRelations { get; } = new();

	/// <summary>
	///		Manager de los métodos de raycasting
	/// </summary>
	public RaycastingService RaycastingService { get; }
}