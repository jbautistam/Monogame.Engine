namespace Bau.Libraries.BauGame.Engine.Actors.Components;

/// <summary>
///		Colección de componentes
/// </summary>
public class ComponentCollectionModel(AbstractActor owner) : List<AbstractComponent>
{
	/// <summary>
	///		Actualiza los componentes
	/// </summary>
	public void Update(Managers.GameContext gameContext)
	{
		foreach (AbstractComponent component in this)
			if (component.Enabled)
				component.Update(gameContext);
	}

	/// <summary>
	///		Dibuja los componentes
	/// </summary>
	internal void Draw(Scenes.Cameras.Camera2D camera, Managers.GameContext gameContext)
	{
		foreach (AbstractComponent component in this)
			if (component.Enabled && (component.Drawable || GameEngine.Instance.EngineSettings.DebugMode))
				component.Draw(camera, gameContext);
	}

    /// <summary>
	///		Obtiene el primer componente de un tipo
	/// </summary>
    public TypeComponent? GetComponent<TypeComponent>() where TypeComponent : AbstractComponent => this.OfType<TypeComponent>().FirstOrDefault();

    /// <summary>
	///		Comprueba si existe un componente de un tipo
	/// </summary>
    public bool Any<TypeComponent>() where TypeComponent : AbstractComponent => this.OfType<TypeComponent>().Any();
    
	/// <summary>
	///		Obtiene los componentes de un tipo
	/// </summary>
    public List<TypeComponent> GetComponents<TypeComponent>() where TypeComponent : AbstractComponent => this.OfType<TypeComponent>().ToList();

	/// <summary>
	///		Actor al que pertenecen los componentes
	/// </summary>
	public AbstractActor Owner { get; } = owner;
}