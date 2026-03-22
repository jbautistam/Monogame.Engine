using Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.Spawners;

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
    public SpawnerActorBuilder WithPointSpawner(float x, float y, float width, float height, 
                                                AbstractShapeEmitter.EmissionLocationMode location, AbstractShapeEmitter.EmissionDirectionMode direction, 
                                                float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new PointShapeEmitter()
                                                            {
                                                                EmissionLocation = location,
                                                                EmissionDirection = direction
                                                            },
                           triggerTime);
    }

    /// <summary>
    ///     Añade un generador de tipo círculo
    /// </summary>
    public SpawnerActorBuilder WithCircleSpawner(float x, float y, float radius, 
                                                 AbstractShapeEmitter.EmissionLocationMode location, AbstractShapeEmitter.EmissionDirectionMode direction, 
                                                 float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new CircleShapeEmitter(radius)
                                                            {
                                                                EmissionLocation = location,
                                                                EmissionDirection = direction
                                                            }, 
                           triggerTime);
    }

    /// <summary>
    ///     Añade un generador de tipo rectángulo
    /// </summary>
    public SpawnerActorBuilder WithRectangleSpawner(float x, float y, float width, float height, 
                                                    AbstractShapeEmitter.EmissionLocationMode location, AbstractShapeEmitter.EmissionDirectionMode direction, 
                                                    float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), new RectangleShapeEmitter(width, height)
                                                            {
                                                                EmissionLocation = location,
                                                                EmissionDirection = direction
                                                            }, 
                           triggerTime);
    }

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(float x, float y, AbstractShapeEmitter emitter,  float triggerTime)
    {
        return WithSpawner(new Vector2(x, y), emitter, triggerTime);
    }

    /// <summary>
    ///     Añade un generador
    /// </summary>
    public SpawnerActorBuilder WithSpawner(Vector2 position, AbstractShapeEmitter emitter, float triggerTime)
    {
        // Añade el generador
        Spawner.AddSpawner(position, emitter, triggerTime);
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