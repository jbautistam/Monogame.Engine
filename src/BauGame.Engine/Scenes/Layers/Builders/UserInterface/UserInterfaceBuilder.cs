using Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Builders.UserInterface;

/// <summary>
///		Builder para interface de usuario
/// </summary>
public class UserInterfaceBuilder(AbstractScene scene, string name, int sortIndex)
{
	/// <summary>
	///		Añade un elemento a la capa
	/// </summary>
	public UserInterfaceBuilder WithItem(UiElement item)
	{
		// Añade el elemento
		Layer.Items.Add(item);
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Genera la capa
	/// </summary>
	public UserInterfaceLayer Build() => Layer;

	/// <summary>
	///		Datos de la capa
	/// </summary>
	public UserInterfaceLayer Layer { get; } = new(scene, name, sortIndex);
}
