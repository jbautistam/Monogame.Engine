using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;

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
	public abstract void Draw(Scenes.Rendering.RenderingManager renderingManager, Rectangle position, GameContext gameContext);

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
