using Microsoft.Xna.Framework;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Sequences.Commands.Movements;

/// <summary>
///     Comando para ejecutar movimientos entre diferentes puntos
/// </summary>
public class PatrolCommand(string actorId, float startTime, float duration) : AbstractSequenceCommand(actorId, startTime, duration)
{
    /// <summary>
    ///     Tipo de patrullaje
    /// </summary>
    public enum PatrolType 
    { 
        /// <summary>En bucle</summary>
        Loop,
        /// <summary>De inicio a fin y de final a inicio</summary>
        PingPong,
        /// <summary>Selección de un valor aleatorio</summary>
        Random,
        /// <summary>Se ejecuta la ruta sólo una vez</summary>
        Once
    }
    // Variables privadas
    private Vector2 _currentTarget;
    private int _currentIndex, _direction = 1;
    private float _waitTimer;
    private bool _waiting;
    
    /// <summary>
    ///     Aplica el comando al actor
    /// </summary>
    protected override void ApplySelf(Bau.BauEngine.Actors.AbstractActorDrawable actor)
    {
        if (_waiting)
        {
            // Incrementa el temporizador
            _waitTimer += 0.016f;
            // Si se supera el tiempo, se obtiene el siguiente punto
            if (_waitTimer >= WaitTime)
            {
                _waiting = false;
                SelectNextWaypoint();
            }
        }
        else
        {
            Vector2 toTarget = _currentTarget - actor.Transform.Bounds.Location;
            float distance = toTarget.Length();

                // Mueve hacia el siguiente punto, si         
                if (distance < 5f)
                {
                    // Cambia el punto actual por el punto de destino
                    actor.Transform.Bounds.Location = _currentTarget;
                    // Indica que vuelve al modo de espera
                    _waiting = true;
                    _waitTimer = 0;
                }
                else
                {
                    Vector2 moveDirection = Vector2.Normalize(toTarget);
                    float moveAmount = Math.Min(Speed * 0.016f, distance);
        
                        // Cambia la posición y rota hacia la dirección de movimiento si es necesario
                        actor.Transform.Bounds.Location += moveDirection * moveAmount;
                        if (RotateToTarget && toTarget != Vector2.Zero)
                            actor.Transform.Rotation = MathF.Atan2(moveDirection.Y, moveDirection.X);
                }
        }
    }
    
    /// <summary>
    ///     Selecciona el siguiente punto de control
    /// </summary>
    private void SelectNextWaypoint()
    {
        // Obtiene el siguiente índice
        switch (Type)
        {
            case PatrolType.Loop:
                    _currentIndex += _direction;
                break;
            case PatrolType.PingPong:
                    _currentIndex += _direction;
                    if (_currentIndex >= Waypoints.Count - 1 || _currentIndex <= 0)
                        _direction *= -1;
                break;
            case PatrolType.Random:
                    _currentIndex = Bau.BauEngine.Tools.Randomizer.Random.Next(Waypoints.Count);
                break;
            case PatrolType.Once:
                    if (_currentIndex < Waypoints.Count - 1)
                        _currentIndex += _direction;
                break;
        }
        // Normaliza el destino
        if (_currentIndex < 0 && _currentIndex > Waypoints.Count - 1)
        {
            if (_direction == 1)
                _currentIndex = 0;
            else
                _currentIndex = Waypoints.Count - 1;
        }
        // Obtiene el destino actual
        _currentTarget = Waypoints[_currentIndex];
    }
    
    /// <summary>
    ///     Tipo de movimiento
    /// </summary>
    public required PatrolType Type { get; init; }

    /// <summary>
    ///     Puntos de movimiento
    /// </summary>
    public List<Vector2> Waypoints { get; set; } = [];

    /// <summary>
    ///     Tiempo de espera entre pasos
    /// </summary>
    public required float WaitTime { get; init; }

    /// <summary>
    ///     Velocidad de movimiento
    /// </summary>
    public required float Speed { get; init; }

    /// <summary>
    ///     Indica si se debe rotar hacia el movimiento
    /// </summary>
    public bool RotateToTarget { get; set; }
}