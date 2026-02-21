using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Generador de <see cref="UiProgressBar"/>
/// </summary>
public class UserInterfaceProgressBarBuilder : AbstractElementUserInterfaceBuilder<UiProgressBar>
{
	public UserInterfaceProgressBarBuilder(AbstractUserInterfaceLayer layer, float x, float y, float width, float height)
	{
		Item = new UiProgressBar(layer, new UiPosition(x, y, width, height));
	}

	/// <summary>
	///		Asigna el valor máximo
	/// </summary>
	public UserInterfaceProgressBarBuilder WithMaximum(int maximum)
	{
		// Asigna el valor máximo
		Item.Maximum = maximum;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el valor actual
	/// </summary>
	public UserInterfaceProgressBarBuilder WithValue(int value)
	{
		// Asigna el valor
		Item.Value = value;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna la orientación
	/// </summary>
	public UserInterfaceProgressBarBuilder WithOrientation(UiProgressBar.OrientationMode orientation)
	{
		// Asigna la orientación
		Item.Orientation = orientation;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el tiempo que dura la animación para pasar del valor anterior al actual
	/// </summary>
	public UserInterfaceProgressBarBuilder WithAnimationTime(float time)
	{
		// Asigna el tiempo que dura la animación
		Item.AnimationTime = Math.Clamp(time, 0.01f, 5);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la textura de fondo de la barra de progreso
	/// </summary>
	public UserInterfaceProgressBarBuilder WithBackgroundBar(string texture)
	{
		// Asigna la textura
		Item.BackgroundBarTexture = texture;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade la textura de la barra de progreso
	/// </summary>
	public UserInterfaceProgressBarBuilder WithBar(string texture)
	{
		// Asigna la textura
		Item.Texture = texture;
		// Devuelve el generador
		return this;
	}
}