using Microsoft.Xna.Framework;
using Bau.BauEngine.Actors.ParticlesEngine.Particles;

namespace Bau.BauEngine.Actors.ParticlesEngine.Emitters;

/// <summary>
///     Emisor de partículas
/// </summary>
public class ParticleEmitter
{
    // Variables privadas
    private float _elapsedTime, _emissionTimer, _currentEmissionInterval;

    public ParticleEmitter(ParticleEngineActor particleEngine, Vector2 position, ParticleEmitterShape shape, ParticleEmitterProfile profile)
    {
        ParticleEngine = particleEngine;
        Pool = new ParticlePool(this);
        Position = position;
        EmitterShape = shape;
        Profile = profile;
    }

    /// <summary>
    ///     Actualiza el emisor
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        if (Enabled)
        {
            // Desactivamos si hemos pasado la duración o ejecutamos el motor si hemos superado el momento de inicio
            if (Profile.Duration > 0 && _elapsedTime > Profile.Start + Profile.Duration)
                Enabled = false;
            else if (_elapsedTime > Profile.Start)
            {
                // Incrementa el temporizador de la emisión
                _emissionTimer += gameContext.DeltaTime;
                // Si ha llegado el momento de emitir
                if (_emissionTimer >= _currentEmissionInterval)
                {
                    // Emite las partículas
                    EmitParticle(Profile.ParticlesPerEmission.GetValue());
                    // Recalcula el intervalo de tiempo para la siguiente emisión
                    _currentEmissionInterval = 1f / Profile.EmissionRate.GetValue();
                    _emissionTimer = 0;
                }
            }
            // Incrementa el tiempo de vida
            _elapsedTime += gameContext.DeltaTime;
        }
    }

    /// <summary>
    ///     Emite una partícula
    /// </summary>
    private void EmitParticle(int particles)
    {
        for (int index = 0; index < particles; index++)
        {
            ParticleModel? particle = Pool.GetNext();

                // Si hay alguna partícula libre
                if (particle is not null)
                {
                    Shapes.AbstractShapeEmitter.EmissionData data = EmitterShape.Shape.GetEmissionData(EmitterShape.Location, 
                                                                                                       EmitterShape.DirectionMode, 
                                                                                                       EmitterShape.FixedDirection);

                        // Asigna la posición y la velocidad inicial
                        particle.Position = ParticleEngine.Transform.Bounds.Location + Position + data.Position;
                        particle.Velocity = data.Direction * Profile.Speed.GetValue();
                        // Actualiza el resto de propiedades
                        particle.TotalLifeTime = Profile.Lifetime.GetValue();
                        particle.LifeTime = 0;
                        particle.Scale = Profile.Scale.GetValue();
                        particle.Color = Profile.Color.GetValue();
                }
        }
    }

    /// <summary>
    ///     Motor al que se asocia el emisor
    /// </summary>
    public ParticleEngineActor ParticleEngine { get; }

    /// <summary>
    ///     Pool de partículas
    /// </summary>
    public ParticlePool Pool { get; }

    /// <summary>
    ///     Posición relativa al motor
    /// </summary>
    public Vector2 Position { get; }

    /// <summary>
    ///     Perfil de emisión de partículas
    /// </summary>
    public ParticleEmitterProfile Profile { get; }

    /// <summary>
    ///     Forma de emisión de las partículas
    /// </summary>
    public ParticleEmitterShape EmitterShape { get; }

    /// <summary>
    ///     Modificadores
    /// </summary>
    public List<Modifiers.AbstractParticleModifier> Modifiers { get; } = [];

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    public bool Enabled { get; private set; } = true;
}