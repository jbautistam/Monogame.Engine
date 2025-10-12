namespace Bau.Libraries.BauGame.Engine.Core.Models;

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
}