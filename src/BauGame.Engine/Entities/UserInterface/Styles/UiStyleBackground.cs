using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de un fondo
/// </summary>
public class UiStyleBackground
{
	/// <summary>
	///		Textura
	/// </summary>
	public string? Texture { get; set; }

	/// <summary>
	///		Region
	/// </summary>
	public string? Region { get; set; }

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;
}