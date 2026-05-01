using Bau.BauEngine.Managers;
using Bau.BauEngine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Entities.UserInterface.Backgrounds;

/// <summary>
///     Clase base para los fondos
/// </summary>
public abstract class UiAbstractBackground(Styles.UiStyle style)
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
}
