using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///     Actor con un manager de un sistema de partículas
/// </summary>
public class ParticlesSystemEngineActor(Scenes.Layers.AbstractLayer layer, int? zOrder = null) : AbstractActor(layer, zOrder)
{
    // Variables privadas
    private float _elapsedTime;

    /// <summary>
    ///     Arranca el sistema
    /// </summary>
	public override void StartActor()
	{
        // Inicializa el tiempo
        _elapsedTime = 0;
        // Arranca los controladores
        foreach (ParticlesPoolManager poolManager in Spawners)
            poolManager.Start(this);
	}

    /// <summary>
    ///     Actualiza el actor
    /// </summary>
	protected override void UpdateActor(Managers.GameContext gameContext)
	{
        // Si se ha pasado el tiempo, destruye el actor
        if (Duration > 0 && _elapsedTime > Duration)
        {
            Enabled = false;
            Layer.Actors.Destroy(this, TimeSpan.FromSeconds(1));
        }
        else
        {
            // Incrementa el tiempo
            _elapsedTime += gameContext.DeltaTime;
            // Actualiza los pools de partículas
            foreach (ParticlesPoolManager spawnerBehavior in Spawners)
                if (spawnerBehavior.Enabled)
                    spawnerBehavior.Update(gameContext);
        }
	}

    /// <summary>
    ///     Dibuja las partículas activas y sus colas correspondientes
    /// </summary>
	protected override void DrawActor(Camera2D camera, Managers.GameContext gameContext)
    {
        foreach (ParticlesPoolManager spawnerBehavior in Spawners)
            if (spawnerBehavior.Enabled)
                spawnerBehavior.Draw(camera, gameContext);
    }

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
        foreach (ParticlesPoolManager spawnerBehavior in Spawners)
            spawnerBehavior.Enabled = false;
	}

    /// <summary>
    ///     Posición desde donde se emiten las partículas
    /// </summary>
    public required Vector2 Position { get; set; }

    /// <summary>
    ///     Generadores de partículas
    /// </summary>
    public List<ParticlesPoolManager> Spawners { get; } = [];

    /// <summary>
    ///     Duración (0 para un sistema infinito)
    /// </summary>
    public float Duration { get; set; } = 1.0f;
}