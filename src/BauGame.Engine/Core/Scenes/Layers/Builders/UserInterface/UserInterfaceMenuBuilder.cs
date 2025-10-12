using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.UserInterface;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de menús
/// </summary>
public class UserInterfaceMenuBuilder : AbstractElementUserInterfaceBuilder<UiMenu>
{
	public UserInterfaceMenuBuilder(UserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiMenu(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Añade un fondo
	/// </summary>
	public UserInterfaceMenuBuilder WithBackground(string texture)
	{
		// Asigna el fondo
		Item.Background = new UserInterfaceBackgroundBuilder(Item.Layer, texture, 0, 0, 1, 1).Item;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo para las opciones seleccionadas
	/// </summary>
	public UserInterfaceMenuBuilder WithSelectedBackground(string texture)
	{
		// Asigna el fondo
		Item.SelectedBackground = new UserInterfaceBackgroundBuilder(Item.Layer, texture, 0, 0, 1, 1).Item;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo para las opciones no seleccionadas
	/// </summary>
	public UserInterfaceMenuBuilder WithUnselectedBackground(string texture)
	{
		// Asigna el fondo
		Item.UnselectedBackground = new UserInterfaceBackgroundBuilder(Item.Layer, texture, 0, 0, 1, 1).Item;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade un fondo para las opciones cuando el cursor del ratón está sobre la opción
	/// </summary>
	public UserInterfaceMenuBuilder WithHoverBackground(string texture)
	{
		// Asigna el fondo
		Item.HoverBackground = new UserInterfaceBackgroundBuilder(Item.Layer, texture, 0, 0, 1, 1).Item;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Cambia el color para las opciones seleccionadas
	/// </summary>
	public UserInterfaceMenuBuilder WithSelectedColor(Color color)
	{
		// Asigna el color
		Item.SelectedColor = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Cambia el color para las opciones no seleccionadas
	/// </summary>
	public UserInterfaceMenuBuilder WithUnselectedColor(Color color)
	{
		// Asigna el color
		Item.UnselectedColor = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Cambia el color para las opciones sobre las que se posiciona el ratón
	/// </summary>
	public UserInterfaceMenuBuilder WithHoverColor(Color color)
	{
		// Asigna el color
		Item.HoverColor = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una opción
	/// </summary>
	public UserInterfaceMenuBuilder WithOption(int optionId, string text, string font, float x, float y, float width, float height)
	{
		// Crea la opción
		Item.Options.Add(new UiMenuOption(Item.Layer, new UiPosition(x, y, width, height), optionId)
								{
									Text = text,
									Font = font
								}
						 );	
		// Devuelve el navegador
		return this;
	}
}