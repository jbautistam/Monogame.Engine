using Bau.Libraries.BauGame.Engine.Models;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

/// <summary>
///		Representación de un colider como rectángulo
/// </summary>
public class RectangleCollider(CollisionComponent collision, RectangleF relative) : AbstractCollider(collision)
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
        RectangleF bounds = Collision.Owner.Transform.WorldBounds;

            // Calcula el rectángulo con respecto al padre
            return new RectangleF(bounds.X + Scale(Relative.X, bounds.Width),
                                  bounds.Y + Scale(Relative.Y, bounds.Height),
                                  bounds.Width + Scale(Relative.Width, bounds.Width),
                                  bounds.Height + Scale(Relative.Height, bounds.Height));
    }

    /// <summary>
    ///     Posiciones relativas
    /// </summary>
    public RectangleF Relative { get; } = relative;
}