using Bau.BauEngine.Entities.UserInterface.Backgrounds;
using Bau.BauEngine.Entities.UserInterface.Styles;

namespace Bau.BauEngine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de fondos
/// </summary>
public class UserInterfaceBackgroundBuilder
{
	public UserInterfaceBackgroundBuilder(UiStyle style, string texture, string? region)
	{
		Background = new UiBackground(style)
						{
							Sprite = new Entities.Sprites.SpriteDefinition(texture, region)
						};
	}

	/// <summary>
	///		Genera el fondo
	/// </summary>
	public UiBackground Build() => Background;

	/// <summary>
	///		Fondo
	/// </summary>
	public UiBackground Background { get; }
}