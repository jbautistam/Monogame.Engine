using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.Styles;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de fondos
/// </summary>
public class UserInterfaceBackgroundBuilder
{
	public UserInterfaceBackgroundBuilder(UiStyle style, string texture)
	{
		Background = new UiBackground(style)
						{
							Texture = texture
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