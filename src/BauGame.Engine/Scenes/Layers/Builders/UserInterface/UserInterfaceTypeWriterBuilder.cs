using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Bau.Libraries.BauGame.Engine.Entities.Common.Sprites;
using Bau.Libraries.BauGame.Engine.Entities.UserInterface.TypeWriter;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador para etiquetas con efecto de máquina de escribir
/// </summary>
public class UserInterfaceTypeWriterBuilder : AbstractElementUserInterfaceBuilder<UiTypeWriterLabel>
{
	public UserInterfaceTypeWriterBuilder(AbstractUserInterfaceLayer layer, string text, float x, float y, float width, float height)
	{
		Item = new UiTypeWriterLabel(layer, new UiPosition(x, y, width, height))
						{
							Text = text
						};
	}

	/// <summary>
	///		Asigna la fuente
	/// </summary>
	public UserInterfaceTypeWriterBuilder WithFont(SpriteTextDefinition font)
	{
		// Asigna la fuente
		Item.Font = font;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna la velocidad
	/// </summary>
	public UserInterfaceTypeWriterBuilder WithSpeed(float speed)
	{
		// Asigna la velocidad
		Item.Speed = speed;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el modo
	/// </summary>
	public UserInterfaceTypeWriterBuilder WithMode(UiTypeWriterLabel.WriteMode mode)
	{
		// Asigna el modo
		Item.Mode = mode;
		// Devuelve el generador
		return this;
	}
}