using Bau.Libraries.BauGame.Engine.Core.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Components.Physics;

/// <summary>
///		Componente para el manejo de colisiones
/// </summary>
public class CollisionComponent(AbstractActor owner, int physicLayerId) : AbstractComponent(owner, false)
{
	/// <summary>
	///		Actualiza el componente para sus físicas
	/// </summary>
	public override void UpdatePhysics(GameTime gameTime)
	{
        if (Colliders.Count > 0 && Owner.PreviuosTransform != Owner.Transform)
            Owner.Layer.Scene.PhysicsManager.CollisionSpatialGrid.Add(Owner);
	}

	/// <summary>
	///		Actualiza el componente
	/// </summary>
	public override void Update(GameTime gameTime)
	{
	}

	/// <summary>
	///		Dibuja el componente
	/// </summary>
	public override void Draw(Camera2D camera, GameTime gameTime)
	{
	}

	/// <summary>
	///		Finaliza el componente (quita el objeto del grid de colisiones)
	/// </summary>
	public override void End()
	{
		Owner.Layer.Scene.PhysicsManager.CollisionSpatialGrid.Remove(Owner);
	}

	/// <summary>
	///		Id de la capa de física
	/// </summary>
	public int PhysicLayerId { get; } = physicLayerId;

    /// <summary>
    ///     Elementos que pueden colisionar
    /// </summary>
    public List<AbstractCollider> Colliders { get; } = [];
}