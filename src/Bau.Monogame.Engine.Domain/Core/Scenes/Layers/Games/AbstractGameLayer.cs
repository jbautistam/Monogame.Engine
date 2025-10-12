using Bau.Monogame.Engine.Domain.Core.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Core.Scenes.Layers.Games;

/// <summary>
///		Clase abstracta de definición de partidas
/// </summary>
public abstract class AbstractGameLayer : AbstractLayer
{
	public AbstractGameLayer(AbstractScene scene, string name, int sortOrder) : base(scene, name, LayerType.Game, sortOrder)
	{
		ProjectileManager = new GameManagers.ProjectileManager(this);
		ExplosionsManager = new GameManagers.ExplosionsManager(this);
	}

	/// <summary>
	///		Inicia la capa
	/// </summary>
	protected override void StartLayer()
	{
		// Inicia la capa de la partida
		StartGameLayer();
	}

	/// <summary>
	///		Inicia la capa de partida
	/// </summary>
	protected abstract void StartGameLayer();

	/// <summary>
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	protected override void UpdateLayer(GameTime gameTime)
	{
		// Actualiza los datos de los managers
		ProjectileManager.Update(gameTime);
		ExplosionsManager.Update(gameTime);
		// Actualiza la capa de la partida
		UpdateGameLayer(gameTime);
	}

	/// <summary>
	///		Actualiza los datos particulares de la capa de juego
	/// </summary>
	protected abstract void UpdateGameLayer(GameTime gameTime);

	/// <summary>
	///		Dibuja la capa (los actores se dibujan por separado)
	/// </summary>
	protected override void DrawLayer(Camera2D camera, GameTime gameTime)
	{
		// Dibuja los datos de los managers
		ProjectileManager.Draw(camera, gameTime);
		ExplosionsManager.Draw(camera, gameTime);
		// Dibuja la capa de partida
		DrawGameLayer(camera, gameTime);
	}

	/// <summary>
	///		Dibuja los datos de la partida
	/// </summary>
	protected abstract void DrawGameLayer(Camera2D camera, GameTime gameTime);

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	protected override void EndLayer()
	{
		// Limipia los datos
		ProjectileManager.Clear();
		ExplosionsManager.Clear();
		// Finaliza la capa
		EndGameLayer();
	}

	/// <summary>
	///		Finaliza la capa de la partida
	/// </summary>
	protected abstract void EndGameLayer();

	/// <summary>
	///		Manager para proyectiles
	/// </summary>
	public GameManagers.ProjectileManager ProjectileManager { get; }

	/// <summary>
	///		Manager para explosioens
	/// </summary>
	public GameManagers.ExplosionsManager ExplosionsManager { get; }
}
