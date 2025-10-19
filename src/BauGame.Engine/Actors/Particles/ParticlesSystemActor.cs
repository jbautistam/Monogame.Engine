using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///     Manager de un sistema de partículas
/// </summary>
public class ParticlesSystemActor(Scenes.Layers.AbstractLayer layer, int zOrder) : AbstractActor(layer, zOrder)
{
    // Constantes privadas
    private const float TailDensity = 5f;
    // Variables privadas
    private Pool.ObjectPool<Particle> _particles = new();
    private AbstractTexture? _texture;

    /// <summary>
    ///     Arranca el sistema
    /// </summary>
	public override void Start()
	{
        _texture = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Texture);
	}

    /// <summary>
    ///     Emite las partículas basándos en el tipo de efecto
    /// </summary>
    public void Emit()
    {
        Effect?.Emit(this);
    }

    /// <summary>
    ///     Crea una partícula
    /// </summary>
	internal Particle CreateParticle(Vector2 position, Vector2 direction, float speed, float lifetime, float scale, Color color)
	{
        Particle? particle = _particles.GetFirstInactive();

            // Añade la partícula al pool si no existía o la reinicia si ya existía
            if (particle is null)
            {
                // Crea una nueva partícula
                particle = new Particle
                                        {
                                            Position = position,
                                            Direction = direction,
                                            Speed = speed,
                                            LifeTime = lifetime,
                                            Color = color,
                                            Scale = new Vector2(scale, scale)
                                        };
                // y la añade al pool
                _particles.Add(particle);
            }
            else
            {
                particle.Position = position;
                particle.Direction = direction;
                particle.Speed = speed;
                particle.LifeTime = lifetime;
                particle.Color = color;
                particle.Scale = new Vector2(scale, scale);
            }
            // Devuelve la partícula creada
            return particle;
	}

    /// <summary>
    ///     Actualiza el actor
    /// </summary>
	protected override void UpdateActor(Managers.GameContext gameContext)
	{
        foreach (Particle particle in _particles.Enumerate())
            particle.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja las partículas activas y sus colas correspondientes
    /// </summary>
	protected override void DrawActor(Camera2D camera, Managers.GameContext gameContext)
    {
        TextureRegion? region = _texture?.GetRegion(Region);

            if (region is not null)
                foreach (Particle particle in _particles.Enumerate())
                {
                    Vector2 center = new(region.Region.Center.X, region.Region.Center.Y);
                    Vector2 tailDirection = particle.Position - particle.PreviousPosition;
                    float tailLength = particle.TailLength * tailDirection.Length();

                        // Normaliza el vector de la dirección de cola para asegurarse que el movimiento es consistente
                        if (tailDirection != Vector2.Zero)
                            tailDirection.Normalize();
                        // Dibuja la partícula principal
                        region.Draw(camera, particle.Position, center, particle.Scale, SpriteEffects.None, particle.Color, particle.Rotation);
                        // Dibuja la cola de partículas en segmentos
                        for (float tail = 0; tail < tailLength; tail += TailDensity)
                        {
                            Vector2 tailPosition = particle.Position - tailDirection * tail;
                            float alpha = MathHelper.Clamp(1f - tail / tailLength, 0f, 1f);
                            Color tailColor = particle.Color * alpha;

                                // Dibuja el segmento de cola a una escala más pequeña
                                region.Draw(camera, tailPosition, center, particle.Scale, SpriteEffects.None, tailColor, particle.Rotation);
                        }
                }
    }

	/// <summary>
	///		Finaliza el trabajo con el actor
	/// </summary>
	protected override void EndActor()
	{
        _particles.Clear();
	}

    /// <summary>
    ///     Posición desde donde se emiten las partículas
    /// </summary>
    public required Vector2 Position { get; set; }

    /// <summary>
    ///     Textura que utiliza este conjunto de partículas
    /// </summary>
    public required string Texture { get; init; }

    /// <summary>
    ///     Región de la textura
    /// </summary>
    public required string Region { get; init; }

    /// <summary>
    ///     Efecto
    /// </summary>
    public Effects.AbstracParticlesEffect? Effect { get; set; }
}