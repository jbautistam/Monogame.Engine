using Bau.BauEngine.Entities.UserInterface;
using Bau.BauEngine.Entities.Sprites;

namespace Bau.BauEngine.Scenes.Layers.Builders.UserInterface;

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
	public UserInterfaceMenuBuilder WithOption(int optionId, string text, string style, float x, float y, float width, float height)
	{
		// Crea la opción
		Item.AddOption(new UiMenuOption(Item, new UiPosition(x, y, width, height), optionId)
								{
									Text = text,
									Style = style
								}
						 );	
		// Devuelve el generador
		return this;
	}
}