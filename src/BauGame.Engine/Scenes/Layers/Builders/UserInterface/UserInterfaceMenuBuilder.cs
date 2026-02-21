using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de menús
/// </summary>
public class UserInterfaceMenuBuilder : AbstractElementUserInterfaceBuilder<UiMenu>
{
	public UserInterfaceMenuBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiMenu(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Añade una opción
	/// </summary>
	public UserInterfaceMenuBuilder WithOption(int optionId, string text, string font, float x, float y, float width, float height)
	{
		// Crea la opción
		Item.Options.Add(new UiMenuOption(Item, new UiPosition(x, y, width, height), optionId)
								{
									Text = text,
									Font = font
								}
						 );	
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una opción
	/// </summary>
	public UserInterfaceMenuBuilder WithOptionsStyle(string style)
	{
		// Crea la opción
		Item.StyleOptions = style;	
		// Devuelve el navegador
		return this;
	}
}