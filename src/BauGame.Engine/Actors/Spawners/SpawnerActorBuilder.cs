using Bau.Libraries.BauGame.Engine.Actors.Particles.Emisors;
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
    ///     Añade un generador de tipo punto
    /// </summary>
    public SpawnerActorBuilder WithPointSpawner(float x, float y, float width, float height, bool edgeOnly, float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new PointEmissorShape(), triggerTime);
    }

    /// <summary>
    ///     Añade un generador de tipo círculo
    /// </summary>
    public SpawnerActorBuilder WithCircleSpawner(float x, float y, float radius, bool edgeOnly, float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new CircleEmissorShape()
                                                        {
                                                            Radius = radius,
                                                            EdgeOnly = edgeOnly
                                                        }, 
                           triggerTime);
    }

    /// <summary>
    ///     Añade un generador de tipo rectángulo
    /// </summary>
    public SpawnerActorBuilder WithRectangleSpawner(float x, float y, float width, float height, bool edgeOnly, float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new RectangleEmissorShape()
                                                        {
                                                            Size = new Vector2(width, height),
                                                            EdgeOnly = edgeOnly
                                                        }, 
                           triggerTime);
    }

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(float x, float y, AbstractEmissorShape emissor, float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), emissor, triggerTime);
    }

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(Vector2 position, AbstractEmissorShape emissor, float triggerTime)
    {
        // Añade el generador
        Spawner.AddSpawner(position, emissor, triggerTime);
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