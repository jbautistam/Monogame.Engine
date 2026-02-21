using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de botones
/// </summary>
public class UserInterfaceButtonBuilder : AbstractElementUserInterfaceBuilder<UiButton>
{
	public UserInterfaceButtonBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiButton(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Añade una etiqueta
	/// </summary>
	public UserInterfaceButtonBuilder WithLabel(UiLabel label)
	{
		// Asigna la etiqueta
		Item.Label = label;
		// Devuelve el generador
		return this;
	}
}