using Bau.Libraries.BauGame.Engine.Models;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

/// <summary>
///		Base para los colider
/// </summary>
public abstract class AbstractCollider(CollisionComponent collision)
{
    /// <summary>
    ///     Comprueba si dos <see cref="AbstractCollider"/> están colisionando
    /// </summary>
    public bool IsColliding(AbstractCollider second)
    {
        if (!Enabled || !GetBoundsAABB().Intersects(second.GetBoundsAABB()))
            return false;
        else
            return IsCollidingWith(second);
    }

	/// <summary>
	///		Obtiene los límites del collider para pruebas rápidas AABB
	/// </summary>
	public abstract RectangleF GetBoundsAABB();

    /// <summary>
    ///     Comprueba si dos <see cref="AbstractCollider"/> están colisionando
    /// </summary>
    protected abstract bool IsCollidingWith(AbstractCollider second);

    /// <summary>
    ///     Colisión
    /// </summary>
    public CollisionComponent Collision { get; } = collision;

	/// <summary>
	///		Indica si el Collider está activo
	/// </summary>
	public bool Enabled { get; set; } = true;
}