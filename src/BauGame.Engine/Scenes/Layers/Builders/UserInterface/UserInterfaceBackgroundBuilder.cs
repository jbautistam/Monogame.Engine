using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de fondos
/// </summary>
public class UserInterfaceBackgroundBuilder : AbstractElementUserInterfaceBuilder<UiBackground>
{
	public UserInterfaceBackgroundBuilder(AbstractUserInterfaceLayer layer, string texture, float x, float y, float width, float height)
	{
		Item = new UiBackground(layer, new UiPosition(x, y, width, height))
						{
							Texture = texture
						};
	}
}