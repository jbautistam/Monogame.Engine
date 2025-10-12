using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Actors;

/// <summary>
///		Clase abstracta para la definición de actores
/// </summary>
public abstract class AbstractActor : Pool.IPoolable
{
	protected AbstractActor(Scenes.Layers.AbstractLayer layer)
	{
		Layer = layer;
		Transform = new Components.Transforms.TransformComponent(this);
		PreviuosTransform = new Components.Transforms.TransformComponent(this);
		Renderer = new Components.Renderers.RendererComponent(this);
		Components = new Components.ComponentCollectionModel(this);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public abstract void Start();

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
    public void UpdatePhisics(GameTime gameTime)
    {
		// Actualiza el tamaño del actor a partir del tamaño de la textura
		Transform.WorldBounds.Resize(Renderer.GetSize());
		// Actualiza las físicas
		foreach (Components.AbstractComponent component in Components)
			component.UpdatePhysics(gameTime);
    }

	/// <summary>
	///		Actualiza el actor y sus componentes
	/// </summary>
    public void Update(GameTime gameTime)
    {
		// Guarda la transformación actual (actes de actualizar la transformación en el Update)
		PreviuosTransform = Transform.Clone();
		// Primero actualiza el actor
		UpdateActor(gameTime);
		// y después los componentes
		Renderer.Update(gameTime);
		Components.Update(gameTime);
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected abstract void UpdateActor(GameTime gameTime);
    
	/// <summary>
	///		Dibuja el actor y los componentes
	/// </summary>
    public void Draw(Scenes.Cameras.Camera2D camera, GameTime gameTime)
    {
		// Dibuja la textura y los componentes
		Renderer.Draw(camera, gameTime);
		Components.Draw(camera, gameTime);
		// Llama al actor para que se dibuje si es necesario
		DrawActor(camera, gameTime);
    }

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected abstract void DrawActor(Scenes.Cameras.Camera2D camera, GameTime gameTime);

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	public void End()
	{
		// Desactiva el actor
		Enabled = false;
		// Detiene los componentes
		foreach (Components.AbstractComponent component in Components)
			component.End();
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected abstract void EndActor();

	/// <summary>
	///		Capa a la que se asocia el actor
	/// </summary>
	public Scenes.Layers.AbstractLayer Layer { get; }

	/// <summary>
	///		Indica si el actor está activo
	/// </summary>
	public bool Enabled { get; set; } = true;

	/// <summary>
	///		Datos de posición
	/// </summary>
	public Components.Transforms.TransformComponent PreviuosTransform { get; private set; }

	/// <summary>
	///		Datos de posición
	/// </summary>
	public Components.Transforms.TransformComponent Transform { get; }

	/// <summary>
	///		Objeto de representación
	/// </summary>
	public Components.Renderers.RendererComponent Renderer { get; }

	/// <summary>
	///		Componentes
	/// </summary>
	public Components.ComponentCollectionModel Components { get; }
}