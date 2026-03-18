using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///		Comando para mostrar un movimiento que simula la flotabilidad en un líquido
/// </summary>
public class BuoyancyCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        float yOffset = MathF.Sin(Progress * Frequency + Phase) * Amplitude;
        float xOffset = MathF.Sin(Progress * 0.5f + Phase) * Drift;

            // Cambia la ubicación y rota suavemente imitando un balanceo        
            actor.Transform.Bounds.Location = new Vector2(actor.Transform.Bounds.Location.X + xOffset * 0.016f, BaseHeight + yOffset);
            actor.Transform.Rotation = MathF.Sin(Progress * Frequency * 0.5f + Phase) * 0.05f;
    }

    /// <summary>
    ///     Altura base
    /// </summary>
    public required float BaseHeight { get; init; }

    /// <summary>
    ///     Amplitud
    /// </summary>
    public required float Amplitude { get; init; }

    /// <summary>
    ///     Frecuencia
    /// </summary>
    public required float Frequency { get; init; }

    /// <summary>
    ///     Deriva
    /// </summary>
    public required float Drift { get; init; }

    /// <summary>
    ///     Fase
    /// </summary>
    public required float Phase { get; init; }
}