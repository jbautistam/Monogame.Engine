namespace Bau.BauEngine.Entities.UserInterface;

/// <summary>
///		Definición de una pantalla
/// </summary>
public class UiScreen
{
	public UiScreen(Scenes.Layers.AbstractUserInterfaceLayer layer)
	{
		Layer = layer;
		Styles = new Styles.UiStylesCollection(layer);
	}

	/// <summary>
	///		Capa a la que se asocia la definición de pantalla
	/// </summary>
	public Scenes.Layers.AbstractUserInterfaceLayer Layer { get; }

	/// <summary>
	///		Estilos de la pantalla
	/// </summary>
	public Styles.UiStylesCollection Styles { get; }

	/// <summary>
	///		Componentes de la pantalla
	/// </summary>
	public UiElementList Components { get; } = new();
}
