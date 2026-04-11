using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de un texto
/// </summary>
public class UiStyleText
{
	/// <summary>
	///		Fuente
	/// </summary>
	public string? Font { get; set; }

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