using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador para cuadros de diálogo de una Visual Novel
/// </summary>
public class UserInterfaceVisuaNovelTextBuilder : AbstractElementUserInterfaceBuilder<UiVisualNovelDialog>
{
	public UserInterfaceVisuaNovelTextBuilder(AbstractUserInterfaceLayer layer, string text, float x, float y, float width, float height)
	{
		Item = new UiVisualNovelDialog(layer, new UiPosition(x, y, width, height));
		Item.TypeWriter = new UiTypeWriterLabel(layer, new UiPosition(0, 0, 1, 1));
		Item.TypeWriter.Text = text;
	}

	/// <summary>
	///		Asigna la fuente
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithFont(SpriteTextDefinition font)
	{
		// Asigna la fuente
		if (Item.TypeWriter is not null)
			Item.TypeWriter.Font = font;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna la velocidad
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithSpeed(float speed)
	{
		// Asigna la velocidad
		if (Item.TypeWriter is not null)
			Item.TypeWriter.Speed = speed;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el estilo al cuadro de texto
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithTextStyle(string style)
	{
		// Asigna el estilo
		if (Item.TypeWriter is not null)
			Item.TypeWriter.Style = style;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el modo
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithMode(UiTypeWriterLabel.WriteMode mode)
	{
		// Asigna el modo
		if (Item.TypeWriter is not null)
			Item.TypeWriter.Mode = mode;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el avatar izquierdo
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithLeftAvatar(UiVisualNovelAvatar avatar)
	{
		// Asigna el avatar izquierdo
		Item.LeftAvatar = avatar;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el avatar derecho
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithRightAvatar(UiVisualNovelAvatar avatar)
	{
		// Asigna el avatar derecho
		Item.RightAvatar = avatar;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el cursor
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithCursor(string asset, string? region)
	{
		// Asigna el cursor
		Item.Cursor = CreateImage(asset, region, string.Empty);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Crea una imagen
	/// </summary>
	private UiImage CreateImage(string asset, string? region, string style)
	{
		UiImage image = new(Item.Layer, new UiPosition(0, 0, 0, 0));

			// Crea la definición de la imagen
			image.Sprite = new Entities.Common.Sprites.SpriteDefinition(asset, region);
			image.Style = style;
			// Devuelve la imagen
			return image;
	}

	/// <summary>
	///		Asigna la cabecera
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithHeader(string text, SpriteTextDefinition font, string asset, string? region, bool visible, string style)
	{
		// Crea la cabecera
		Item.Header = new UiVisualNovelHeader(Item.Layer, new UiPosition(0, 0, 0, 0));
		Item.Header.Label = new UiLabel(Item.Layer, new UiPosition(0, 0, 0, 0));
		Item.Header.Label.Font = font;
		Item.Header.Label.Text = text;
		Item.Header.Style = style;
		Item.Header.Image = CreateImage(asset, region, string.Empty);
		Item.Header.Image.Visible  = visible;
		Item.Header.Visible = visible;
		// Devuelve el generador
		return this;
	}
}