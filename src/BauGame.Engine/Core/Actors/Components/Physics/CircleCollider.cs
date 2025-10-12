using Microsoft.Xna.Framework;
using Bau.Monogame.Engine.Domain.Core.Models;

namespace Bau.Monogame.Engine.Domain.Core.Actors.Components.Physics;

/// <summary>
///		Representación de un colider como círculo
/// </summary>
public class CircleCollider(CollisionComponent collision, Circle relative) : AbstractCollider(collision)
{
	/// <summary>
	///		Obtiene los límites como rectángulo
	/// </summary>
	public override RectangleF GetBoundsAABB() => GetReal().ToRectangleF();

    /// <summary>
    ///     Comprueba si dos <see cref="AbstractCollider"/> están colisionando
    /// </summary>
    protected override bool IsCollidingWith(AbstractCollider second)
	{
		if (second is CircleCollider circleCollider)
			return GetReal().Intersects(circleCollider.GetReal());
		else if (second is RectangleCollider rectangleCollider)
			return GetReal().Intersects(rectangleCollider.GetBounds());
		else
			return false;
	}

	/// <summary>
	///		Obtiene el centro real
	/// </summary>
	public Circle GetReal()
	{
        RectangleF bounds = Collision.Owner.Transform.WorldBounds;

            // Calcula el círculo con respecto al padre
            return new Circle(new Vector2(bounds.X + Scale(Relative.Center.X, bounds.Width),
										  bounds.Y + Scale(Relative.Center.Y, bounds.Height)),
							  Scale(Relative.Radius, bounds.Width));
	}

	/// <summary>
	///		Centro
	/// </summary>
	public Circle Relative { get; } = relative;
}