using Bau.Monogame.Engine.Domain.Core.Scenes.Layers.UserInterface;
using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de etiquetas que se muestran con el tiempo
/// </summary>
public class UserInterfaceBalloonLabelBuilder : AbstractElementUserInterfaceBuilder<UiBalloonLabel>
{
	public UserInterfaceBalloonLabelBuilder(UserInterfaceLayer layer, string text, float x, float y, float width, float height)
	{
		Item = new UiBalloonLabel(layer, new UiPosition(x, y, width, height))
									{
										Text = text
									};
	}

	/// <summary>
	///		Asigna la fuente
	/// </summary>
	public UserInterfaceBalloonLabelBuilder WithFont(string font)
	{
		// Asigna la fuente
		Item.Font = font;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el color
	/// </summary>
	public UserInterfaceBalloonLabelBuilder WithColor(Color color)
	{
		// Asigna el color
		Item.Color = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna la velocidad
	/// </summary>
	public UserInterfaceBalloonLabelBuilder WithSpeed(float speed)
	{
		// Asigna la velocidad
		Item.Speed = speed;
		// Devuelve el generador
		return this;
	}
}