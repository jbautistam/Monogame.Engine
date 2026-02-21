using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Bau.Libraries.BauGame.Engine.Scenes.Layers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;

/// <summary>
///		Estilo de un elemento
/// </summary>
public class UiStyle(UiStylesCollection stylesParent, UiStyle.StyleType type)
{
	/// <summary>
	///		Tipo para el que se define el estilo
	/// </summary>
	public enum StyleType
	{
		Disabled,
		Pressed,
		Selected,
		Hover,
		Normal
	}

    /// <summary>
    ///     Actualiza el elemento
    /// </summary>
    public void Update(GameContext gameContext)
    {
		Background?.Update(gameContext);
		Border?.Update(gameContext);
    }

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public void Draw(Camera2D camera, Rectangle position, GameContext gameContext)
	{
		Background?.Draw(camera, position, gameContext);
		Border?.Draw(camera, position, gameContext);
	}

	/// <summary>
	///		Colección padre
	/// </summary>
	public UiStylesCollection StylesParent { get; } = stylesParent;

	/// <summary>
	///		Capa a la que se asocia el estilo
	/// </summary>
	public AbstractUserInterfaceLayer Layer { get; } = stylesParent.Layer;

	/// <summary>
	///		Tipo
	/// </summary>
	public StyleType Type { get; } = type;

	/// <summary>
	///		Margen externo
	/// </summary>
	public UiMargin Margin { get; set; } = new(0);

	/// <summary>
	///		Espaciado interno
	/// </summary>
	public UiMargin Padding { get; set; } = new(0);

	/// <summary>
	///		Fondo
	/// </summary>
	public Backgrounds.UiAbstractBackground? Background { get; set; }

	/// <summary>
	///		Borde
	/// </summary>
	public Borders.UiAbstractBorder? Border { get; set; }

	/// <summary>
	///		Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Opacidad
	/// </summary>
	public float Opacity { get; set; }
}