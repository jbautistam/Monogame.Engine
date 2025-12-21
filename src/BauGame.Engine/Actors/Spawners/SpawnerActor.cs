using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Spawners;

/// <summary>
///		Actor utilizado para lanzar elementos
/// </summary>
public class SpawnerActor(Scenes.Layers.AbstractLayer layer) : AbstractActor(layer, null)
{
	/// <summary>
	///		Arranca el actor
	/// </summary>
	public override void StartActor()
	{
	}

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerWaveModel AddSpawner(float x, float y, float radius, float triggerTime) => AddSpawner(new Vector2(x, y), radius, triggerTime);

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerWaveModel AddSpawner(Vector2 position, float radius, float triggerTime)
    {
        SpawnerWaveModel spawnerWave = new(position, radius, triggerTime);

            // Añade el generador
            SpawnerWaves.Add(spawnerWave);
            // Devuelve el generador para que se siga configurando
            return spawnerWave;
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateActor(GameContext gameContext)
	{
        foreach (SpawnerWaveModel spawnerWave in SpawnerWaves)
            if (spawnerWave.MustRaise(gameContext))
                spawnerWave.Raise();
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
	protected override void DrawActor(Camera2D camera, GameContext gameContext)
	{
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndActor()
	{
	}

    /// <summary>
    ///     Olas que se deben lanzar
    /// </summary>
    public List<SpawnerWaveModel> SpawnerWaves { get; } = [];
}