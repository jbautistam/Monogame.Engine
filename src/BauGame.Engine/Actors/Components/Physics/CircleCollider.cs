using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Models;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.Physics;

/// <summary>
///		Representación de un colider como círculo
/// </summary>
public class CircleCollider(CollisionComponent collision, Circle? relative) : AbstractCollider(collision)
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
			if (Relative is null)
				return new Circle(new Vector2(bounds.X, bounds.Y), 0.5f * bounds.Width);
			else
				return new Circle(new Vector2(bounds.X + Relative?.Center.X ?? 0,
											  bounds.Y + Relative?.Center.Y ?? 0),
								  Relative?.Radius ?? 0);
	}

	/// <summary>
	///		Centro
	/// </summary>
	public Circle? Relative { get; } = relative;
}