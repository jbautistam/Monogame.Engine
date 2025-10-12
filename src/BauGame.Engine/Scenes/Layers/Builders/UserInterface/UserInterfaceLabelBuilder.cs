using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de etiquetas
/// </summary>
public class UserInterfaceLabelBuilder : AbstractElementUserInterfaceBuilder<UiLabel>
{
	public UserInterfaceLabelBuilder(UserInterfaceLayer layer, string text, float x, float y, float width, float height)
	{
		Item = new UiLabel(layer, new UiPosition(x, y, width, height))
						{
							Text = text
						};
	}

	/// <summary>
	///		Asigna la fuente
	/// </summary>
	public UserInterfaceLabelBuilder WithFont(string font)
	{
		// Asigna la fuente
		Item.Font = font;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el color
	/// </summary>
	public UserInterfaceLabelBuilder WithColor(Color color)
	{
		// Asigna el color
		Item.Color = color;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna la alineación
	/// </summary>
	public UserInterfaceLabelBuilder WithAlignment(UiLabel.HorizontalAlignmentType horizontalAlignment, UiLabel.VerticalAlignmentType verticalAlignment)
	{
		// Asigna la alineación
		Item.HorizontalAlignment = horizontalAlignment;
		Item.VerticalAlignment = verticalAlignment;
		// Devuelve el generador
		return this;
	}
}