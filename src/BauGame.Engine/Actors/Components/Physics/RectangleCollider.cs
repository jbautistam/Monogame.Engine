using Bau.Libraries.BauGame.Engine.Models;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

/// <summary>
///		Representación de un colider como rectángulo
/// </summary>
public class RectangleCollider(CollisionComponent collision, RectangleF? relative) : AbstractCollider(collision)
{
    /// <summary>
    ///     Obtiene el rectángulo para pruebas AABB
    /// </summary>
    public override RectangleF GetBoundsAABB() => GetBounds();

    /// <summary>
    ///     Comprueba si dos <see cref="AbstractCollider"/> están colisionando
    /// </summary>
    protected override bool IsCollidingWith(AbstractCollider second)
	{
		if (second is CircleCollider circleCollider)
			return circleCollider.GetReal().Intersects(GetBounds());
		else if (second is RectangleCollider rectangleCollider)
			return GetBounds().Intersects(rectangleCollider.GetBounds());
		else
			return false;
	}

    /// <summary>
    ///     Obtiene el rectángulo teniendo en cuenta la transformación
    /// </summary>
    public RectangleF GetBounds()
    {
        RectangleF world = Collision.Owner.Transform.BoundsCentered;
        float scaledWidth = world.Width * Collision.Owner.Renderer.Scale.X;
        float scaledHeight = world.Height * Collision.Owner.Renderer.Scale.Y;

            if (Relative is null)
                return new RectangleF(world.X - 0.5f * scaledWidth, world.Y - 0.5f * scaledHeight,
                                      scaledWidth, scaledHeight);
            else
                return world + Relative;
    }

    /// <summary>
    ///     Posiciones relativas
    /// </summary>
    public RectangleF? Relative { get; } = relative;
}