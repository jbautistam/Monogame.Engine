using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

/// <summary>
///     Definición de una partícula
/// </summary>
public class ParticleProperties : Pool.IPoolable
{
    // Variables privadas
    private AbstractTexture? _texture;

    /// <summary>
    ///     Inicializa las propiedades de la partícula
    /// </summary>
	public void Reset(Vector2 position, ParticleSpawnProperties spawnProperties, List<Behaviors.AbstractParticleBehavior> particleBehaviors)
	{
        // Inicializa las propiedades
		Position = position;
        Direction = GetVector(spawnProperties.Angle);
        Speed = GetValue(spawnProperties.Speed);
        LifeTime = GetValue(spawnProperties.LifeTime);
        Color = GetColor(spawnProperties.Color);
        Scale = GetVector(spawnProperties.Scale);
        Opacity = GetValue(spawnProperties.Opacity);
        TailLength = GetValue(spawnProperties.TailLength);
        TailDensity = GetValue(spawnProperties.TailDensity);
        Rotation = GetValue(spawnProperties.Rotation);
        Texture = spawnProperties.Texture;
        Region = spawnProperties.Region;
        // Asigna los comportamientos y los inicializa
        ParticleBehaviors.Clear();
        foreach (Behaviors.AbstractParticleBehavior particleBehavior in particleBehaviors)
            ParticleBehaviors.Add(particleBehavior.Clone());
        foreach (Behaviors.AbstractParticleBehavior particleBehavior in ParticleBehaviors)
            particleBehavior.Reset(this);
        // Obtiene la textura
        if (!string.IsNullOrWhiteSpace(Texture))
            _texture = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Texture);
        else
            _texture = null;
	}

    /// <summary>
    ///     Modifica el estado de la partícula
    /// </summary>
    public void Update(GameContext gameContext)
    {
        // Actualiza los comportamientos
        foreach (Behaviors.AbstractParticleBehavior particleBehavior in ParticleBehaviors)
            particleBehavior.Update(gameContext);
        // Inicializa la velocidad
        Velocity = Direction * Speed;
        // Guarda la posición anterior
        PreviousPosition = Position;
        // Actualiza la posición de la partícula basándose en la velocidad
        Position += Velocity * gameContext.DeltaTime;
        // Reduce el tiempo restante de vida
        LifeTime -= gameContext.DeltaTime;
    }

    /// <summary>
    ///     Dibuja las partículas activas y sus colas correspondientes
    /// </summary>
	internal void Draw(Camera2D camera, GameContext gameContext)
    {
        TextureRegion? region = _texture?.GetRegion(Region);

            if (region is not null)
            {
                Vector2 center = new(region.Region.Center.X, region.Region.Center.Y);
                Vector2 tailDirection = Position - PreviousPosition;
                float tailLength = TailLength * tailDirection.Length();

                    // Normaliza el vector de la dirección de cola para asegurarse que el movimiento es consistente
                    if (tailDirection != Vector2.Zero)
                        tailDirection.Normalize();
                    // Dibuja la partícula principal
                    region.Draw(camera, Position, center, Scale, SpriteEffects.None, Color * Opacity, Rotation);
                    // Dibuja la cola de partículas en segmentos
                    for (float tail = 0; tail < tailLength; tail += TailDensity)
                    {
                        Vector2 tailPosition = Position - tailDirection * tail;
                        float alpha = MathHelper.Clamp(1f - tail / tailLength, 0f, 1f);

                            // Dibuja el segmento de cola a una escala más pequeña
                            region.Draw(camera, tailPosition, center, Scale, SpriteEffects.None, Color * alpha, Rotation);
                    }
            }
    }

    /// <summary>
    ///     Obtiene un color teniendo en cuenta un rango
    /// </summary>
	private Color GetColor(ParticleSpawnProperties.Range<Color> range) => Tools.Randomizer.GetRandomColor(range.Minimum, range.Maximum);

    /// <summary>
    ///     Obtiene un vector teniendo en cuenta un rango
    /// </summary>
	private Vector2 GetVector(ParticleSpawnProperties.Range<float> range) => new(GetValue(range), GetValue(range));

    /// <summary>
    ///     Obtiene un valor aleatorio de un rango
    /// </summary>
    private float GetValue(ParticleSpawnProperties.Range<float> range)
    {
        if (range.Minimum == range.Maximum)
            return range.Minimum;
        else
            return Tools.Randomizer.GetRandom(range.Minimum, range.Maximum);
    }

	/// <summary>
	///     Posición de la partícula
	/// </summary>
	public Vector2 Position { get; set; }

    /// <summary>
    ///     Posición anterior de la partícula
    /// </summary>
    internal Vector2 PreviousPosition { get; private set; }

    /// <summary>
    ///     Dirección hacia la que se mueve la partícula
    /// </summary>
    public Vector2 Direction { get; set; }

    /// <summary>
    ///     Velocidad
    /// </summary>
    public float Speed { get; set; } = 1;

    /// <summary>
    ///     Tiempo de vida inicial
    /// </summary>
    public float LifeTime { get; set; }

	/// <summary>
	///     Color
	/// </summary>
	public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Escala de la partícula
    /// </summary>
    public Vector2 Scale { get; set; }

    /// <summary>
    ///     Opacidad
    /// </summary>
    public float Opacity { get; set; } = 1;

    /// <summary>
    ///     Longitud de la cola dibujada para cada partícula
    /// </summary>
    public float TailLength { get; set; } = 0;

    /// <summary>
    ///     Densidad de la cola dibujada para cada partícula
    /// </summary>
    public float TailDensity { get; set; } = 0;

    /// <summary>
    ///     Vector de velocidad de la partícula
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    ///     Rotación
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Textura
    /// </summary>
    public string? Texture { get; set; }

    /// <summary>
    ///     Región de la textura
    /// </summary>
    public string? Region { get; set; }

    /// <summary>
    ///     Comportamientos asociados a la partícula
    /// </summary>
    public List<Behaviors.AbstractParticleBehavior> ParticleBehaviors { get; } = [];

    /// <summary>
    ///     Comprueba si la partícula aún está viva
    /// </summary>
    public bool Enabled => LifeTime > 0;
}