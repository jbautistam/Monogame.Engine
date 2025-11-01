using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

/// <summary>
///		Componente para el manejo de colisiones
/// </summary>
public class CollisionComponent(AbstractActor owner, int physicLayerId) : AbstractComponent(owner, false)
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	public override void Start()
	{
		// ... no hace nada
	}

	/// <summary>
	///		Actualiza el componente para sus físicas
	/// </summary>
	public override void UpdatePhysics(Managers.GameContext gameContext)
	{
        if (Colliders.Count > 0 && Owner.PreviuosTransform != Owner.Transform)
            Owner.Layer.Scene.PhysicsManager.CollisionSpatialGrid.Add(Owner);
	}

	/// <summary>
	///		Actualiza el componente
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el componente
	/// </summary>
	public override void Draw(Camera2D camera, Managers.GameContext gameContext)
	{
		foreach (AbstractCollider collider in Colliders)
			switch (collider)
			{
				case RectangleCollider rectangle:
						camera.SpriteBatchController.DrawRectangleOutline(collider.GetBoundsAABB().ToRectangle(), Color.White, 2);
					break;
				case CircleCollider circle:
						camera.SpriteBatchController.DrawRectangleOutline(collider.GetBoundsAABB().ToRectangle(), Color.Red, 2);
					break;
			}
	}

	/// <summary>
	///		Finaliza el componente (quita el objeto del grid de colisiones)
	/// </summary>
	public override void End()
	{
		Owner.Layer.Scene.PhysicsManager.CollisionSpatialGrid.Remove(Owner);
	}

	/// <summary>
	///		Desactiva las colisiones
	/// </summary>
	public void ToggleEnabled(bool enabled)
	{
		foreach (AbstractCollider collider in Colliders)
			collider.Enabled = enabled;
	}

	/// <summary>
	///		Id de la capa de física
	/// </summary>
	public int PhysicLayerId { get; set; } = physicLayerId;

    /// <summary>
    ///     Elementos que pueden colisionar
    /// </summary>
    public List<AbstractCollider> Colliders { get; } = [];
}