using Microsoft.Xna.Framework;
using Bau.BauEngine.Tools.MathTools.Easing;
using Bau.BauEngine.Actors;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///		Comando para describir una órbita
/// </summary>
public class OrbitCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    // Variables privadas
    private Vector2 _center;
    private float _baseRotation;
    private bool _initialized;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(AbstractActorDrawable actor)
    {
        // Inicializa los datos
        if (!_initialized)
        {
            _baseRotation = actor.Transform.Rotation;
            _center = ToWorld(Center);
            _initialized = true;
        }
        // Calcula el efecto
        ComputeEffect(actor);
    }

    /// <summary>
    ///     Calcula el efecto
    /// </summary>
	private void ComputeEffect(AbstractActorDrawable actor)
	{
        float angleDiff = GetAngleDiff();
        float currentAngle = StartAngle + angleDiff * EasingFunctionsHelper.Apply(Progress, Easing);
        
            // Asigna la posición
            actor.Transform.Bounds.Location = CalculatePosition(currentAngle);
            // Rota el sprite para mirar hacia la dirección de movimiento
            actor.Transform.Rotation = _baseRotation + currentAngle + MathHelper.PiOver2;

        // Obtiene el ángulo entre inicio y fin
        float GetAngleDiff()
        {
            float angleDiff = EndAngle - StartAngle;

                // Dependiendo de si estamos en el sentido de las agujas del reloj o al contrario
                if (!Clockwise) 
                    return -angleDiff;
                else
                    return angleDiff;
        }

        // Calcula la posición con respecto al ángulo
        Vector2 CalculatePosition(float angle) => new(_center.X + MathF.Cos(angle) * RadiusX, _center.Y + MathF.Sin(angle) * RadiusY);
	}

	/// <summary>
	///     Centro de la órbita
	/// </summary>
	public required Vector2 Center { get; init; }

    /// <summary>
    ///     Radio X
    /// </summary>
    public float RadiusX { get; set; }

    /// <summary>
    ///     Radio Y
    /// </summary>
    public float RadiusY { get; set; }

    /// <summary>
    ///     Angulo inicial
    /// </summary>
    public float StartAngle { get; set; }

    /// <summary>
    ///     Angulo final
    /// </summary>
    public float EndAngle { get; set; }

    /// <summary>
    ///     Indica si se gira en el sentido del reloj o al contrario
    /// </summary>
    public bool Clockwise { get; set; }
}