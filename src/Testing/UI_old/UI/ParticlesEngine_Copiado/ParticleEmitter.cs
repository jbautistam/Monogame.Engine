using Microsoft.Xna.Framework;

namespace UI.CharactersEngine.ParticlesEngine;

/// <summary>
///     Emisor de partículas
/// </summary>
public class ParticleEmitter
{
    // Variables privadas
    private float _elapsedTime, _emissionTimer, _currentEmissionInterval;
    private Random _random;

    public ParticleEmitter(ParticleEngine particleEngine, Vector2 position, EmissionProfile profile, Random sharedRandom)
    {
        ParticleEngine = particleEngine;
        Pool = new ParticlePool(this);
        Position = position;
        Profile = profile;
        _random = sharedRandom;
    }

    /// <summary>
    ///     Actualiza el emisor
    /// </summary>
    public void Update(GameTime gameTime)
    {
        if (Enabled)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

                // Desactivamos si hemos pasado la duración o ejecutamos el motor si hemos superado el momento de inicio
                if (Profile.Duration > 0 && _elapsedTime > Profile.Start + Profile.Duration)
                    Enabled = false;
                else if (_elapsedTime > Profile.Start)
                {
                    // Incrementa el temporizador de la emisión
                    _emissionTimer += deltaTime;
                    // Si ha llegado el momento de emitir
                    if (_emissionTimer >= _currentEmissionInterval)
                    {
                        // Emite las partículas
                        EmitParticle();
                        // Recalcula el intervalo de tiempo para la siguiente emisión
                        _currentEmissionInterval = 1f / Math.Min(Profile.EmissionRate.GetValue(_random), 1);
                        _emissionTimer = 0;
                    }
                    // Actualiza el pool de partículas
                    Pool.Update(gameTime);
                }
                // Incrementa el tiempo de vida
                _elapsedTime += deltaTime;
        }
    }

    /// <summary>
    ///     Emite una partícula
    /// </summary>
    private void EmitParticle()
    {
        ParticleModel? particle = Pool.GetNext();

            // Si hay alguna partícula libre
            if (particle is not null)
            {
                Shapes.AbstractShapeEmitter.EmissionData data = Profile.Shape.GetEmissionData(_random, Profile.Location, Profile.DirectionMode, Profile.FixedDirection);

                    // Asigna la posición y la velocidad inicial
                    particle.Position = ParticleEngine.Position + Position + data.Position;
                    particle.Velocity = data.Direction * Profile.Speed.GetValue(_random);
                    // Actualiza el resto de propiedades
                    particle.TotalLifeTime = Profile.Lifetime.GetValue(_random);
                    particle.LifeTime = 0;
                    particle.Scale = Profile.StartScale.GetValue(_random);
                    particle.Color = Profile.Color.GetValue(_random);
            }
    }

    /// <summary>
    ///     Motor al que se asocia el emisos
    /// </summary>
    public ParticleEngine ParticleEngine { get; }

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
    public EmissionProfile Profile { get; }

    /// <summary>
    ///     Indica si está activo
    /// </summary>
    public bool Enabled { get; private set; } = true;
}

/*
---

## 6. Ejemplos de Combinaciones

### A. Humo Realista

```csharp
var smokeProfile = new EmissionProfile();
smokeProfile.Shape = new PointShape(10f);
smokeProfile.Lifetime = new FloatRange(2f, 4f);
smokeProfile.DirectionMode = EmissionDirectionMode.Fixed;
smokeProfile.FixedDirection = new Vector2(0, -1);
smokeProfile.Speed = new FloatRange(20, 50);

// Modificadores
smokeProfile.AddModifier(new GravityModifier(new Vector2(0, -10))); // Flota hacia arriba
smokeProfile.AddModifier(new DragModifier(0.98f)); // Fricción del aire
smokeProfile.AddModifier(new ScaleOverLifetimeModifier(0.5f, 3f)); // Crece con el tiempo
smokeProfile.AddModifier(new AlphaOverLifetimeModifier(0.8f, 0f)); // Desvanece
smokeProfile.AddModifier(new ColorOverLifetimeModifier(
    new Color(100, 100, 100, 200),
    new Color(50, 50, 50, 0)
));
```

### B. Explosión de Fuego

```csharp
var fireProfile = new EmissionProfile();
fireProfile.Shape = new PointShape(5f);
fireProfile.Lifetime = new FloatRange(0.5f, 1.5f);
fireProfile.DirectionMode = EmissionDirectionMode.Random;
fireProfile.Speed = new FloatRange(100, 300);

// Modificadores
fireProfile.AddModifier(new GravityModifier(new Vector2(0, 50))); // Cae ligeramente
fireProfile.AddModifier(new DragModifier(0.95f));
fireProfile.AddModifier(new ScaleOverLifetimeModifier(1f, 0.1f)); // Se encoge
fireProfile.AddModifier(new AlphaOverLifetimeModifier(1f, 0f));
fireProfile.AddModifier(new ColorOverLifetimeModifier(
    new GradientKeyframe(0f, Color.Yellow),
    new GradientKeyframe(0.3f, Color.Orange),
    new GradientKeyframe(0.6f, Color.Red),
    new GradientKeyframe(1f, Color.Gray)
));
```

### C. Lluvia Ácida

```csharp
var rainProfile = new EmissionProfile();
rainProfile.Shape = new RectShape(800f, 50f);
rainProfile.Location = EmissionLocation.Surface;
rainProfile.DirectionMode = EmissionDirectionMode.Fixed;
rainProfile.FixedDirection = new Vector2(0, 1);
rainProfile.Speed = new FloatRange(400, 600);
rainProfile.Lifetime = new FloatRange(1f, 2f);

// Modificadores
rainProfile.AddModifier(new GravityModifier(new Vector2(0, 200))); // Acelera hacia abajo
rainProfile.AddModifier(new DragModifier(0.99f)); // Poca fricción
```

### D. Magia de Atracción (Black Hole)

```csharp
var magicProfile = new EmissionProfile();
magicProfile.Shape = new RingShape(100f, 150f);
magicProfile.Location = EmissionLocation.Border;
magicProfile.DirectionMode = EmissionDirectionMode.Inward;
magicProfile.Speed = new FloatRange(50, 100);
magicProfile.Lifetime = new FloatRange(1f, 3f);

// Modificadores
magicProfile.AddModifier(new AttractionModifier(Vector2.Zero, -500f)); // Repulsión inicial
magicProfile.AddModifier(new RotationOverLifetimeModifier(2f)); // Gira
magicProfile.AddModifier(new ColorOverLifetimeModifier(
    Color.Cyan,
    Color.Purple
));
*/