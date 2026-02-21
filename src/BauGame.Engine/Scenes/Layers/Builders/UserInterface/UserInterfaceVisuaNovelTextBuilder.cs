using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador para etiquetas con efecto de máquina de escribir
/// </summary>
public class UserInterfaceVisuaNovelTextBuilder : AbstractElementUserInterfaceBuilder<UiVisualNovelText>
{
	public UserInterfaceVisuaNovelTextBuilder(AbstractUserInterfaceLayer layer, string text, float x, float y, float width, float height)
	{
		Item = new UiVisualNovelText(layer, new UiPosition(x, y, width, height));
		if (Item.TypeWriter is not null)
			Item.TypeWriter.Text = text;
	}

	/// <summary>
	///		Asigna la fuente
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithFont(string font)
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
	public UserInterfaceVisuaNovelTextBuilder WithLeftAvatar(string asset, string? region, bool visible)
	{
		// Asigna el avatar izquierdo
		Item.LeftAvatar = CreateAvatar(asset, region);
		Item.LeftAvatarVisible = visible;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el avatar derecho
	/// </summary>
	public UserInterfaceVisuaNovelTextBuilder WithRightAvatar(string asset, string? region, bool visible)
	{
		// Asigna el avatar derecho
		Item.RightAvatar = CreateAvatar(asset, region);
		Item.RightAvatarVisible = visible;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Crea un avatar
	/// </summary>
	private UiImage CreateAvatar(string asset, string? region)
	{
		UiImage image = new(Item.Layer, new UiPosition(0, 0, 0, 0));

			// Crea la definición del sprite
			image.Sprite = new Entities.Common.SpriteDefinition(Item.Layer, asset, region);
			// Devuelve la imagen
			return image;
	}
}