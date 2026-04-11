using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de presentación genérico
/// </summary>
public class UiStylePresentation
{
	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;

	/// <summary>
	///		Rotación
	/// </summary>
	public float Rotation { get; set; }
}