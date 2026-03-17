using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.TileMap;

/// <summary>
///     Definición de un patrón
/// </summary>
public class TileActor(TileMapActor map, int tileDefinitionId, bool isSolid) : AbstractActorDrawable(map.Layer, map.ZOrder)
{
	// Variables privadas
	private Components.Physics.CollisionComponent? _collision;

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	protected override void StartActor()
	{
		TileMapActor.TileDefinition? definition = Map.TileDefinitions.Get(TileDefinitionId.ToString());

			// Añade la textura al renderer
			if (definition is not null)
			{
				// Añade la textura
				Renderer.Sprite = new Entities.Common.Sprites.SpriteDefinition(definition.Texture, definition.Region);
				// Añade la animación
				if (!string.IsNullOrWhiteSpace(definition.Animation))
					Renderer.Animator.SetAnimation(definition.Animation, true);
				// Inicializa el renderer
				Renderer.Start();
			}
			// Crea la colisión
			if (IsSolid && _collision is null)
			{
				// Crea la colisión
				_collision = new Components.Physics.CollisionComponent(this, Map.PhysicsLayer);
				_collision.Colliders.Add(new Components.Physics.RectangleCollider(_collision, null));
				// Añade la colisión a la lista de componentes del actor
				Components.Add(_collision);
			}
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		Transform.Bounds.Resize(Renderer.GetSize());
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawSelf(Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
	{
		// ... por ahora sólo implementa la interface
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor(GameContext gameContext)
	{
		if (!IsSolid && _collision is not null)
		{
			// Quita la colisión de la capa de físicas
			_collision.End();
			// Elimina la colisión
			Components.Remove(_collision);
			_collision = null;
		}
	}

	/// <summary>
	///		Mapa al que se asocia el tesel
	/// </summary>
	public TileMapActor Map { get; } = map;

    /// <summary>
    ///     Id de la definición de patrón
    /// </summary>
    public int TileDefinitionId { get; } = tileDefinitionId;

    /// <summary>
    ///     Indica si es un patrón sólido
    /// </summary>
    public bool IsSolid { get; set; } = isSolid;
}