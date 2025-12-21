using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Spawners;

/// <summary>
///		Generador de <see cref="SpawnerActor"/>
/// </summary>
public class SpawnerActorBuilder
{
    public SpawnerActorBuilder(Scenes.Layers.AbstractLayer layer)
    {
        Spawner = new(layer);
    }

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(float x, float y, float radius, float triggerTime) => WithSpawner(new Vector2(x, y), radius, triggerTime);

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(Vector2 position, float radius, float triggerTime)
    {
        // Añade el generador
        Spawner.AddSpawner(position, radius, triggerTime);
        // Devuelve el generador para que se siga configurando
        return this;
    }

    /// <summary>
    ///     Añade una ola al generador
    /// </summary>
    public SpawnerActorBuilder WithWave(string name, Action<SpawnerWaveModel.FactoryParameters> parameters)
    {
        // Añade el generador
        Spawner.SpawnerWaves[Spawner.SpawnerWaves.Count - 1].AddActorFactory(name, parameters);
        // Devuelve el generador para que se siga configurando
        return this;
    }

    /// <summary>
    ///     Genera el actor
    /// </summary>
    public SpawnerActor Build() => Spawner;

    /// <summary>
    ///     Actor principal
    /// </summary>
    public SpawnerActor Spawner { get; }
}