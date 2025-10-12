using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Particles;

// <summary>
///     Definición de una partícula
/// </summary>
public class Particle : Pool.IPoolable
{
    // Variables privadas
    private float _startLifeTime;
    private float? _lifeTime;

    /// <summary>
    ///     Modifica el estado de la partícula
    /// </summary>
    public void Update(GameTime gameTime)
    {
        float elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
        float dragFactor = Math.Max(1 - (elapsedTime * DragPerSecond), 0);

            // Inicializa la velocidad
            Velocity = Direction * Speed;
            // Guarda la posición anterior
            PreviousPosition = Position;
            // Acualiza la posición de la partícula basándose en la velocidad
            Position += Velocity * elapsedTime;
            // Aplica la resistencia para reducir la velocidad sobre el tiempo
            Velocity *= dragFactor;
            // Reduce el tiempo restante de vida
            if (_lifeTime is null)
                _lifeTime = LifeTime;
            else
                _lifeTime -= elapsedTime;
            // Cambia la opacidad de la partícula basándose en lo que le queda de vida
            Color = new Color(Color, (byte) (255f * _lifeTime / LifeTime));
    }

    /// <summary>
    ///     Resistencia al movimiento aplicado a la velocidad por segundo (por ejemplo, para simular la resistencia del aire o la gravedad)
    /// </summary>
    public float DragPerSecond { get; set; }= 0.9f;

    /// <summary>
    ///     Color actual de la partícula
    /// </summary>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    ///     Dirección hacia la que se mueve la partícula
    /// </summary>
    public required Vector2 Direction { get; set; }

    /// <summary>
    ///     Tiempo de vida inicial
    /// </summary>
    public required float LifeTime 
    { 
        get { return _startLifeTime; }
        set
        {
            _startLifeTime = value;
            _lifeTime = value;
        }
    }

    /// <summary>
    ///     Posición de la partícula
    /// </summary>
    public required Vector2 Position { get; set; }

    /// <summary>
    ///     Posición anterior de la partícula
    /// </summary>
    internal Vector2 PreviousPosition { get; private set; }

    /// <summary>
    ///     Escala de la partícula
    /// </summary>
    public Vector2 Scale { get; set; } = new(1, 1);

    /// <summary>
    ///     Longitud de la cola dibujada para cada partícula
    /// </summary>
    public float TailLength { get; set; } = 0;

    /// <summary>
    ///     Velocidad
    /// </summary>
    public float Speed { get; set; } = 1;

    /// <summary>
    ///     Vector de velocidad de la partícula
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    ///     Rotación
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    ///     Comprueba si la partícula aún está viva
    /// </summary>
    public bool Enabled => _lifeTime > 0;
}