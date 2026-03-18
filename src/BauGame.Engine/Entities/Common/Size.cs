using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.Common;

/// <summary>
///		Estructura con los datos de tamaño
/// </summary>
public struct Size(float width, float height)
{
	/// <summary>
	///		Clona la estructura <see cref="Size"/>
	/// </summary>
	public Size Clone() => new(Width, Height);

	/// <summary>
	///		Ancho
	/// </summary>
	public float Width { get; } = width;

	/// <summary>
	///		Alto
	/// </summary>
	public float Height { get; } = height;

	/// <summary>
	///		Centro
	/// </summary>
	public Vector2 Center => new(0.5f * Width, 0.5f * Height);
}