using Bau.Libraries.BauGame.Engine.Managers;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Spawners;

/// <summary>
///     Ola de enemigos a lanzar
/// </summary>
public class SpawnerWaveModel(Vector2 position, Particles.Emisors.AbstractEmissorShape emissor, float triggerTime)
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
            action(new FactoryParameters(name, emissor.GetEmissionPosition(Position), Tools.Randomizer.GetRandomDirection()));
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
    public Particles.Emisors.AbstractEmissorShape emissor { get; } = emissor;

    /// <summary>
    ///     Tiempo de lanzamiento
    /// </summary>
    public float TriggerTime { get; } = triggerTime;

    /// <summary>
    ///     Actores que se van a lanzar en una ola
    /// </summary>
    public List<(string name, Action<FactoryParameters>)> ActorFactories { get; } = [];
}
