using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Actors.TileMap;

/// <summary>
///		Actor para definir un mapa de tiles
/// </summary>
public class TileMapActor(Scenes.Layers.AbstractLayer layer, int physicsLayer, int zOrder) : AbstractActor(layer, zOrder)
{
	// Registros
	public record TileDefinition(int Id, string Texture, string Region, string? Animation);

	/// <summary>
	///		Añade una definición
	/// </summary>
	public void AddDefinition(int id, string texture, string region, string? animation)
	{
		TileDefinitions.Add(id.ToString(), new TileDefinition(id, texture, region, animation));
	}

	/// <summary>
	///		Añade un tile
	/// </summary>
	public void AddTile(int tileId, float x, float y, bool isSolid)
	{
		TileActor tile = new(this, tileId, isSolid);

			// Si es una celda sólida, la asigna al mapa
			if (isSolid)
				Layer.Scene.PhysicsManager.MapManager.GridMap.SetTileFromWorld(x, y, Scenes.Physics.Mapping.GridMap.TileType.Blocked);
			// Coloca el actor
			tile.Transform.Bounds.MoveTo(x, y);
			// Añade el actor a la colección
			Tiles.Add(tile);
	}

	/// <summary>
	///		Inicializa el actor
	/// </summary>
	public override void StartActor()
	{
		foreach (TileActor tile in Tiles)
			tile.Start();
	}

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
		foreach (TileActor tile in Tiles)
			tile.Update(gameContext);
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameContext gameContext)
	{
		foreach (TileActor tile in Tiles)
			if (camera.IsAtView(tile.Transform.Bounds))
				tile.Draw(camera, gameContext);
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor()
	{
		foreach (TileActor tile in Tiles)
			tile.End();
	}

	/// <summary>
	///		Código de la capa de físicas
	/// </summary>
	public int PhysicsLayer { get; } = physicsLayer;

	/// <summary>
	///		Definiciones de patrones del mapa
	/// </summary>
	public Base.DictionaryModel<TileDefinition> TileDefinitions { get; } = new();

	/// <summary>
	///		Patrones del mapa
	/// </summary>
	private List<TileActor> Tiles { get; } = [];
}
