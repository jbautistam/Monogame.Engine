using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.TileMap;

/// <summary>
///     Definición de un patrón
/// </summary>
public class TileActor(TileMapActor map, int tileDefinitionId, bool isSolid) : AbstractActor(map.Layer, map.ZOrder)
{
	// Variables privadas
	private Components.Physics.CollisionComponent? _collision;

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void StartActor()
	{
		TileMapActor.TileDefinition? definition = Map.TileDefinitions.Get(TileDefinitionId.ToString());

			if (definition is not null)
			{
				Renderer.Texture = definition.Texture;
				Renderer.Region = definition.Region;
				if (!string.IsNullOrWhiteSpace(definition.Animation))
					Renderer.Animator.SetAnimation(definition.Animation, true);
				Renderer.Start();
			}
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		// Actualiza el tamaño del actor a partir del tamaño de la textura
		Transform.Bounds.Resize(Renderer.GetSize());
		// Actualiza el componente de colisión
		if (IsSolid && _collision is null)
		{
			// Crea la colisión
			_collision = new Components.Physics.CollisionComponent(this, map.PhysicsLayer);
			_collision.Colliders.Add(new Components.Physics.RectangleCollider(_collision, null));
			// Añade la colisión a la lista de componentes
			Components.Add(_collision);
		}
		else if (!IsSolid && _collision is not null)
		{
			// Quita la colisión de la capa de físicas
			_collision.End();
			// Elimina la colisión
			Components.Remove(_collision);
			_collision = null;
		}
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameContext gameContext)
	{
		// ... por ahora sólo implementa la interface
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor()
	{
		// ... por ahora sólo implementa la interface
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