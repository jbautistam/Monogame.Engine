using Bau.Libraries.BauGame.Engine.Entities.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Clase base para los interfaces de usuario
/// </summary>
public abstract class AbstractElementUserInterfaceBuilder<TypeElement> where TypeElement : UiElement
{
	/// <summary>
	///		Asigna el Id
	/// </summary>
	public AbstractElementUserInterfaceBuilder<TypeElement> WithId(string id)
	{
		// Asigna el Id
		Item.Id = id;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Asigna el estilo
	/// </summary>
	public AbstractElementUserInterfaceBuilder<TypeElement> WithStyle(string style)
	{
		// Asigna el estilo
		Item.Style = style;
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera el elemento
	/// </summary>
	public TypeElement Build() => Item;

	/// <summary>
	///		Elemento
	/// </summary>
	public TypeElement Item { get; protected set; } = default!;
}