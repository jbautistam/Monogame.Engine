using Bau.Libraries.BauGame.Engine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Spawners;

/// <summary>
///     Ola de enemigos a lanzar
/// </summary>
public class SpawnerWaveModel(Vector2 offset, float radius, float triggerTime)
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
            action(new FactoryParameters(name, Tools.Randomizer.GetRandomOffset(Offset, Radius), Tools.Randomizer.GetRandomDirection()));
        // Indica que ya se ha lanzado
        _elapsed = 0;
    }

    /// <summary>
    ///     Posición inicial del lanzamiento
    /// </summary>
    public Vector2 Offset { get; } = offset;

    /// <summary>
    ///     Radio de lanzamiento de la ola
    /// </summary>
    public float Radius { get; } = radius;

    /// <summary>
    ///     Tiempo de lanzamiento
    /// </summary>
    public float TriggerTime { get; } = triggerTime;

    /// <summary>
    ///     Actores que se van a lanzar en una ola
    /// </summary>
    public List<(string name, Action<FactoryParameters>)> ActorFactories { get; } = [];
}
