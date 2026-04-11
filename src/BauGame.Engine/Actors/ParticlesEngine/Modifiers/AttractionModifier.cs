using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Modifiers;

/// <summary>
///     Modificador para cambiar la atracción hacia un punto
/// </summary>
public class AttractionModifier(Vector2 attractorPosition, float force, float minDistance = 1) : AbstractParticleModifier
{
    /// <summary>
    ///     Clona los datos de un modificador
    /// </summary>
    public override AbstractParticleModifier Clone() => new AttractionModifier(new Vector2(attractorPosition.X, attractorPosition.Y), force, minDistance);

    /// <summary>
    ///     Actualiza la velocidad de la partícula
    /// </summary>
    protected override void UpdateSelf(ParticleModel particle, float deltaTime, float normalizedAge)
    {
        Vector2 direction = attractorPosition - particle.Position;
        float distance = direction.Length();
        
            // Normaliza la distancia
            if (distance < minDistance) 
                distance = minDistance;
            // Calcula la nueva velocidad
            particle.Velocity += Vector2.Normalize(direction) * force / (distance * distance) * deltaTime;
    }
}
