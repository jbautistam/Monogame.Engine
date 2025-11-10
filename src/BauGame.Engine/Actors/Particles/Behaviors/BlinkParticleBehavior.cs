namespace Bau.Libraries.BauGame.Engine.Actors.Particles.Behaviors;

/// <summary>
///     Comportamiento de parpadeo de una partícula
/// </summary>
public class BlinkParticleBehavior : AbstractParticleBehavior
{
    // Variables privadas
    private float _blinkTimer = 0f;
    private bool _visible = true;
    private float _startOpacity;

    /// <summary>
    ///     Clona el comportamiento
    /// </summary>
	public override AbstractParticleBehavior Clone()
	{
		return new BlinkParticleBehavior()
                        {
                            BlinkInterval = BlinkInterval,
                            BlinkDuration = BlinkDuration
                        };
	}

    /// <summary>
    ///     Inicializa las propiedades del comportamiento
    /// </summary>
    protected override void ResetBehavior()
    {
        _startOpacity = Particle.Opacity;
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
            Particle.Opacity = 0.3f;
        else
            Particle.Opacity = _startOpacity;
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
