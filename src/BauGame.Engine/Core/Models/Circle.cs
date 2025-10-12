using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Models;

/// <summary>
///		Estructura con los datos de un círculo
/// </summary>
public struct Circle(Vector2 center, float radius)
{
    /// <summary>
    ///     Comprueba si están colisionando dos cículos
    /// </summary>
    public bool Intersects(Circle other)
    {
        float dx = Center.X - other.Center.X;
        float dy = Center.Y - other.Center.Y;
        float radii = Radius + other.Radius;

            // Devuelve el valor que indica si los cículos colisionan
            return dx * dx + dy * dy <= radii * radii;
    }

    /// <summary>
    ///     Comprueba si está colisionando con un rectángulo
    /// </summary>
    public bool Intersects(RectangleF rectangle)
    {
        float closestX = MathHelper.Clamp(Center.X, rectangle.X - rectangle.Width / 2, rectangle.X + rectangle.Width / 2);
        float closestY = MathHelper.Clamp(Center.Y, rectangle.Y - rectangle.Height / 2, rectangle.Y + rectangle.Height / 2);
        float dx = Center.X - closestX;
        float dy = Center.Y - closestY;

            // Comprueba si el centro está dentro del rectángulo
            return dx * dx + dy * dy <= Radius * Radius;
    }

	/// <summary>
	///		Obtiene el rectángulo que contiene el círculo
	/// </summary>
	public RectangleF ToRectangleF() => new(Center.X - Radius, Center.Y - Radius, Radius * 2, Radius * 2);

    /// <summary>
    ///     Clona el objeto
    /// </summary>
    public Circle Clone() => new(new Vector2(Center.X, Center.Y), Radius);

	/// <summary>
	///		Centro
	/// </summary>
	public Vector2 Center { get; } = center;

	/// <summary>
	///		Radio
	/// </summary>
	public float Radius { get; } = radius;
}