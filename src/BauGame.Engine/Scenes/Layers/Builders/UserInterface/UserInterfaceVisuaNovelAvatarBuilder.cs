using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador para avatares de un cuadro de diálogo de una visual novel
/// </summary>
public class UserInterfaceVisuaNovelAvatarBuilder : AbstractElementUserInterfaceBuilder<UiVisualNovelAvatar>
{
	public UserInterfaceVisuaNovelAvatarBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiVisualNovelAvatar(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Asigna la imagen del avatar
	/// </summary>
	public UserInterfaceVisuaNovelAvatarBuilder WithImage(string asset, string? region, bool visible, string style)
	{
		UiImage image = new(Item.Layer, new UiPosition(0, 0, 1, 1));

			// Crea la definición de la imagen
			image.Sprite = new Entities.Common.SpriteDefinition(asset, region);
			image.Style = style;
			// Asigna el avatar
			Item.Avatar = image;
			Item.Visible = visible;
			// Devuelve el generador
			return this;
	}
}