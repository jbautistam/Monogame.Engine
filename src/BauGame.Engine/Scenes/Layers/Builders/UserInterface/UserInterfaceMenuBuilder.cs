using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;

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
	public UserInterfaceMenuBuilder WithOption(int optionId, string text, SpriteTextDefinition font, string style, float x, float y, float width, float height)
	{
		// Crea la opción
		Item.AddOption(new UiMenuOption(Item, new UiPosition(x, y, width, height), optionId)
								{
									Text = text,
									Style = style,
									Font = font
								}
						 );	
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una opción
	/// </summary>
	public UserInterfaceMenuBuilder WithOption(int optionId, string text, string font, string style, float x, float y, float width, float height)
	{
		return WithOption(optionId, text, new SpriteTextDefinition(font), style, x, y, width, height);
	}
}