namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Behaviors;

// Componente de parpadeo
public class BlinkParticleBehavior(Particle particle) : AbstractParticleBehavior(particle)
{
    // Variables privadas
    private float _blinkTimer = 0f;
    private bool _visible = true;

    /// <summary>
    ///     Inicializa las propiedades del comportamiento
    /// </summary>
    public override void Reset()
    {
        _blinkTimer = 0f;
        _visible = true;
    }

    /// <summary>
    ///     Actualiza el comportamiento
    /// </summary>
    public override void Update(Managers.GameContext gameContext)
    {
        // Actualiza el tiempo de ejecución
        _blinkTimer += gameContext.DeltaTime;
        // Cambia la visibilidad del efecto
        if (_visible && _blinkTimer >= BlinkInterval)
        {
            _visible = false;
            _blinkTimer = 0f;
        }
        else if (!_visible && _blinkTimer >= BlinkDuration)
        {
            _visible = true;
            _blinkTimer = 0f;
        }
        // Modifica la transparencia para aumentar el color cuando parpadea
        if (!_visible)
            particle.Color = particle.Color * 0.3f;
    }

    /// <summary>
    ///     Intervalo para el parpadeo
    /// </summary>
    public float BlinkInterval = 0.2f;

    /// <summary>
    ///     Duración del parpadeo
    /// </summary>
    public float BlinkDuration = 0.05f;
}
