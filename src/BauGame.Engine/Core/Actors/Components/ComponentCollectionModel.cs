using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Core.Actors.Components;

/// <summary>
///		Colección de componentes
/// </summary>
public class ComponentCollectionModel(AbstractActor owner) : List<AbstractComponent>
{
	/// <summary>
	///		Actualiza los componentes
	/// </summary>
	public void Update(GameTime gameTime)
	{
		foreach (AbstractComponent component in this)
			if (component.Enabled)
				component.Update(gameTime);
	}

	/// <summary>
	///		Dibuja los componentes
	/// </summary>
	internal void Draw(Scenes.Cameras.Camera2D camera, GameTime gameTime)
	{
		foreach (AbstractComponent component in this)
			if (component.Enabled && component.Drawable)
				component.Draw(camera, gameTime);
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