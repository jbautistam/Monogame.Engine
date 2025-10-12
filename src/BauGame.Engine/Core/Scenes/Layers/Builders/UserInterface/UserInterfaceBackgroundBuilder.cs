using Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de fondos
/// </summary>
public class UserInterfaceBackgroundBuilder : AbstractElementUserInterfaceBuilder<UiBackground>
{
	public UserInterfaceBackgroundBuilder(UserInterfaceLayer layer, string texture, float x, float y, float width, float height)
	{
		Item = new UiBackground(layer, new UiPosition(x, y, width, height))
						{
							Texture = texture
						};
	}
}