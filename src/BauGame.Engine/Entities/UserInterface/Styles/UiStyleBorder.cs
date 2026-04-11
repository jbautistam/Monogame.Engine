using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de un borde
/// </summary>
public class UiStyleBorder
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
	///		Grosor
	/// </summary>
	public int Thickness { get; set; }

	/// <summary>
	///		Radio de las esquinas
	/// </summary>
	public int CornerRadius { get; set; }

	/// <summary>
	///		Color de la sombra
	/// </summary>
	public Color ShadowColor { get; set; } = Color.White;

	/// <summary>
	///		Offset de la sombra
	/// </summary>
	public Vector2 ShadowOffset { get; set; }

	/// <summary>
	///		Radio del blur de la sombra
	/// </summary>
	public int ShadowBlurRadius { get; set; }

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;
}