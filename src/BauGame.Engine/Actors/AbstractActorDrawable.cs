using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors;

/// <summary>
///		Clase abstracta para la definición de actores
/// </summary>
public abstract class AbstractActorDrawable : AbstractActor, Interfaces.IActorDrawable, Interfaces.IActorPhysics
{
	protected AbstractActorDrawable(Scenes.Layers.AbstractLayer layer, int? zOrder) : base(layer, zOrder)
	{
		Renderer = new Components.Renderers.RendererAnimatorComponent(this);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartSelf()
	{
		// Añade el renderer a la lista de componentes
		Components.Add(Renderer);
		// Inicializa el actor
		StartActor();
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected abstract void StartActor();

	/// <summary>
	///		Actualiza las físicas
	/// </summary>
    public void UpdatePhysics(Managers.GameContext gameContext)
    {
		// Actualiza el tamaño del actor a partir del tamaño de la textura
		Transform.Bounds.Resize(Renderer.GetSize());
		// Actualiza las físicas
		Components.UpdatePhysics(gameContext);
    }

	/// <summary>
	///		Obtiene los objetos con los que se tiene contacto hasta cierto punto
	/// </summary>
	public List<Scenes.Physics.KinematicCollisionModel> Raycast(Vector2 direction, float distance, bool stopAtFirst)
	{
		return Layer.Scene.PhysicsManager.RaycastingService.Raycast(this, direction, distance, stopAtFirst);
	}

	/// <summary>
	///		Actualiza el actor y sus componentes
	/// </summary>
    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
		UpdateActor(gameContext);
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected abstract void UpdateActor(Managers.GameContext gameContext);
    
	/// <summary>
	///		Dibuja el actor y los componentes
	/// </summary>
    public void Draw(Scenes.Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext)
    {
		if (renderingManager.Scene.Camera.IsAtView(Transform.Bounds) || MustDrawOutOfCamera())
		{
			// Dibuja los componentes
			Components.Draw(renderingManager, gameContext);
			// Llama al actor para que se dibuje si es necesario
			DrawSelf(renderingManager, gameContext);
		}
    }

	/// <summary>
	///		Indica si se debe dibujar aunque su posición esté fuera de cámara (por ejemplo, los sistemas de partículas
	///	pueden tener los emisores fuera del punto de vista de la cámara pero partículas que se tienen que dibujar)
	/// </summary>
	protected virtual bool MustDrawOutOfCamera() => false;

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected abstract void DrawSelf(Scenes.Rendering.AbstractRenderingManager renderingManager, Managers.GameContext gameContext);

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndSelf(Managers.GameContext gameContext)
	{
		// Finaliza el trabajo con el actor
		EndActor(gameContext);
		// Quita el actor de la lista de objetivos de la cámara
		Layer.Scene.Camera.TargetsManager.Remove(this);
	}

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected abstract void EndActor(Managers.GameContext gameContext);

	/// <summary>
	///		Objeto de representación
	/// </summary>
	public Components.Renderers.AbstractRendererComponent Renderer { get; protected set; }
}