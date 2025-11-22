
namespace Bau.Libraries.BauGame.Engine.Actors.Components;

/// <summary>
///		Colección de componentes
/// </summary>
public class ComponentCollectionModel(AbstractActor owner)
{
	/// <summary>
	///		Añade un componente
	/// </summary>
	public void Add(AbstractComponent component)
	{
		Items.Add(component);
	}

	/// <summary>
	///		Arranca la colección de componentes
	/// </summary>
	public void Start()
	{
		foreach (AbstractComponent component in Items)
			component.Start();
	}

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
    public void UpdatePhysics(Managers.GameContext gameContext)
    {
		foreach (AbstractComponent component in Items)
			component.UpdatePhysics(gameContext);
    }

	/// <summary>
	///		Actualiza los componentes
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		foreach (AbstractComponent component in Items)
			if (component.Enabled)
				component.Update(gameContext);
	}

	/// <summary>
	///		Dibuja los componentes
	/// </summary>
	internal void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
	{
		foreach (AbstractComponent component in Items)
			if (component.Enabled && (component.Drawable || GameEngine.Instance.EngineSettings.DebugMode))
				component.Draw(camera, gameContext);
	}

	/// <summary>
	///		Elimina el componente de la lista
	/// </summary>
	public void Remove(AbstractComponent component)
	{
		// Finaliza el componente
		component.End();
		// Quita el componente de la lista
		Items.Remove(component);
	}

	/// <summary>
	///		Finaliza el trabajo con los componenetes
	/// </summary>
	public void End()
	{
		foreach (AbstractComponent component in Items)
			component.End();
	}

    /// <summary>
	///		Obtiene el primer componente de un tipo
	/// </summary>
    public TypeComponent? GetComponent<TypeComponent>() where TypeComponent : AbstractComponent => Items.OfType<TypeComponent>().FirstOrDefault();

    /// <summary>
	///		Comprueba si existe un componente de un tipo
	/// </summary>
    public bool Any<TypeComponent>() where TypeComponent : AbstractComponent => Items.OfType<TypeComponent>().Any();
    
	/// <summary>
	///		Obtiene los componentes de un tipo
	/// </summary>
    public List<TypeComponent> GetComponents<TypeComponent>() where TypeComponent : AbstractComponent => Items.OfType<TypeComponent>().ToList();

	/// <summary>
	///		Actor al que pertenecen los componentes
	/// </summary>
	public AbstractActor Owner { get; } = owner;

	/// <summary>
	///		Lista de componentes
	/// </summary>
	private List<AbstractComponent> Items { get; } = [];
}