using Bau.Libraries.BauGame.Engine.Managers;
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
	protected override void StartSelf()
	{
	}

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerWaveModel AddSpawner(Vector2 position, Particles.Emisors.AbstractEmissorShape emissor, float triggerTime)
    {
        SpawnerWaveModel spawnerWave = new(position, emissor, triggerTime);

            // Añade el generador
            SpawnerWaves.Add(spawnerWave);
            // Devuelve el generador para que se siga configurando
            return spawnerWave;
    }

	/// <summary>
	///		Actualiza el actor
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        foreach (SpawnerWaveModel spawnerWave in SpawnerWaves)
            if (spawnerWave.MustRaise(gameContext))
                spawnerWave.Raise();
	}

	/// <summary>
	///		Finaliza el actor
	/// </summary>
	protected override void EndSelf(GameContext gameContext)
	{
	}

    /// <summary>
    ///     Olas que se deben lanzar
    /// </summary>
    public List<SpawnerWaveModel> SpawnerWaves { get; } = [];
}