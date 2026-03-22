using Bau.BauEngine.Actors.ParticlesEngine.Emitters.Shapes;
using Bau.BauEngine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.BauEngine.Actors.Spawners;

/// <summary>
///     Ola de enemigos a lanzar
/// </summary>
public class SpawnerWaveModel(Vector2 position, AbstractShapeEmitter emitter, float triggerTime)
{
    // Registros públicos
    public record FactoryParameters(string Name, Vector2 Position, Vector2 Direction);
    // Variables privadas
    private float _elapsed;

    /// <summary>
    ///     Añade un generador de actores
    /// </summary>
    public void AddActorFactory(string name, Action<FactoryParameters> actorFactory)
    {
        ActorFactories.Add((name, actorFactory));
    }

    /// <summary>
    ///     Comprueba si se debe lanzar la ola
    /// </summary>
    public bool MustRaise(GameContext gameContext)
    {
        // Incrementa el tiempo pasado
        _elapsed += gameContext.DeltaTime;
        // Comprueba si se debe lanzar
        return _elapsed >= TriggerTime;
    }

    /// <summary>
    ///     Lanza la ola
    /// </summary>
    public void Raise()
    {
        // Lanza los elementos de la ola
        foreach ((string name, Action<FactoryParameters> action) in ActorFactories)
        {
			AbstractShapeEmitter.EmissionData emission = Emitter.GetEmissionData(null);

                // Crea la acción
                action(new FactoryParameters(name, emission.Position, emission.Direction));
        }   
        // Indica que ya se ha lanzado
        _elapsed = 0;
    }

    /// <summary>
    ///     Posición
    /// </summary>
    public Vector2 Position { get; } = position;

    /// <summary>
    ///     Emisor para lanzamiento
    /// </summary>
    public AbstractShapeEmitter Emitter { get; } = emitter;

    /// <summary>
    ///     Tiempo de lanzamiento
    /// </summary>
    public float TriggerTime { get; } = triggerTime;

    /// <summary>
    ///     Actores que se van a lanzar en una ola
    /// </summary>
    public List<(string name, Action<FactoryParameters>)> ActorFactories { get; } = [];
}
