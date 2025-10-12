namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Clase base para los interfaces de usuario
/// </summary>
public abstract class AbstractElementUserInterfaceBuilder<TypeElement> where TypeElement : Layers.UserInterface.UiElement
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
	///		Genera el elemento
	/// </summary>
	public TypeElement Build() => Item;

	/// <summary>
	///		Elemento
	/// </summary>
	public TypeElement Item { get; protected set; } = default!;
}