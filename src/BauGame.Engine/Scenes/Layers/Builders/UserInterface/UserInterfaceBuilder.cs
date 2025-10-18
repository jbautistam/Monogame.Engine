using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Builder para interface de usuario
/// </summary>
public class UserInterfaceBuilder()
{
	/// <summary>
	///		Añade un elemento a la capa
	/// </summary>
	public UserInterfaceBuilder WithItem(UiElement item)
	{
		// Añade el elemento
		Items.Add(item);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera los elementos
	/// </summary>
	public List<UiElement> Build() => Items;

	/// <summary>
	///		Elementos de la capa de interface de usuario
	/// </summary>
	public List<UiElement> Items { get; } = [];
}
