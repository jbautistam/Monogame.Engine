using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Borders;

/// <summary>
///     Clase base para los fondos
/// </summary>
public abstract class UiAbstractBorder(Styles.UiStyle style)
{
	/// <summary>
	///		Actualiza el fondo
	/// </summary>
	public abstract void Update(GameContext gameContext);

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public abstract void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, Rectangle position, GameContext gameContext);

	/// <summary>
	///		Obtiene un rectángulo que coloca al borde por fuera del contenido
	/// </summary>
	protected Rectangle Inflate(Rectangle bounds)
	{
		return new Rectangle(bounds.X - Thickness, bounds.Y - Thickness, bounds.Width + 2 * Thickness, bounds.Height + 2 * Thickness);
	}

	/// <summary>
	///		Estilo padre
	/// </summary>
	public Styles.UiStyle Style { get; } = style;

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; } = 1;

    /// <summary>
    ///		Ancho del borde en píxeles
    /// </summary>
    public int Thickness { get; set; }
}