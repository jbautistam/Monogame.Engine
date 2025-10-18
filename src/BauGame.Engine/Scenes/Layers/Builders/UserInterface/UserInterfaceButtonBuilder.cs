using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de botones
/// </summary>
public class UserInterfaceButtonBuilder
{
	public UserInterfaceButtonBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Button = new UiButton(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Añade una etiqueta
	/// </summary>
	public UserInterfaceButtonBuilder WithLabel(UiLabel label)
	{
		// Asigna la etiqueta
		Button.Label = label;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo
	/// </summary>
	public UserInterfaceButtonBuilder WithBackground(string texture)
	{
		// Asigna el fondo
		Button.Background = CreateBackground(texture);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo
	/// </summary>
	public UserInterfaceButtonBuilder WithHoverBackground(string texture)
	{
		// Asigna el fondo
		Button.HoverBackground = CreateBackground(texture);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo
	/// </summary>
	public UserInterfaceButtonBuilder WithPressedBackground(string texture)
	{
		// Asigna el fondo
		Button.PressedBackground = CreateBackground(texture);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo
	/// </summary>
	public UserInterfaceButtonBuilder WithSelectedBackground(string texture)
	{
		// Asigna el fondo
		Button.SelectedBackground = CreateBackground(texture);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Crea un fondo
	/// </summary>
	private UiBackground CreateBackground(string texture) => new UserInterfaceBackgroundBuilder(Button.Layer, texture, 0, 0, 1, 1).Item;

	/// <summary>
	///		Vuelve al resultado anterior
	/// </summary>
	public UiButton Build() => Button;

	/// <summary>
	///		Datos del componente
	/// </summary>
	public UiButton Button { get; }
}