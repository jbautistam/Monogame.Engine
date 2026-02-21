using Bau.Libraries.BauGame.Engine.Entities.UserInterface;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de etiquetas
/// </summary>
public class UserInterfaceImageBuilder : AbstractElementUserInterfaceBuilder<UiImage>
{
	public UserInterfaceImageBuilder(AbstractUserInterfaceLayer layer, string asset, string? region, float x, float y, float width, float height)
	{
		Item = new UiImage(layer, new UiPosition(x, y, width, height));
		Item.Sprite = new Entities.Common.SpriteDefinition(layer, asset, region);
	}

	/// <summary>
	///		Asigna la rotación
	/// </summary>
	public UserInterfaceImageBuilder WithRotation(float rotation)
	{
		// Asigna la rotación
		Item.Rotation = rotation;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el origen
	/// </summary>
	public UserInterfaceImageBuilder WithOrigin(Vector2 origin)
	{
		// Asigna el origen
		Item.Origin = origin;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el valor que indica cómo se ajusta la imagen a las dimensiones
	/// </summary>
	public UserInterfaceImageBuilder WithStretch(bool stretch, bool preserveAspectRatio)
	{
		// Asigna los valores de ajuste
		Item.Stretch = stretch;
		Item.PreserveAspectRatio = preserveAspectRatio;
		// Devuelve el generador
		return this;
	}
}