using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Managers;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Games.Routes;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

/// <summary>
///     Estado que permite patrullar a un actor
/// </summary>
public class PatrollingState(string name, PropertiesState properties, WaypointRouteModel route) : AbstractState(name, properties)
{
    // Variables privadas
    private Vector2? _nextWaypoint;
    private float _elapsed;
    private Steering.ArriveSteering? _steering;

	/// <summary>
	///		Inicializa el estado
	/// </summary>
	protected override void StartState()
	{
		_nextWaypoint = Route.GetNextWaypoint(null, Looping);
	}

    /// <summary>
    ///     Actualiza el estado del nodo
    /// </summary>
	protected override string? UpdateState(GameContext gameContext)
	{
        string? nextState = Name;

            // Si no queda ninguna ruta o se ha pasado el tiempo, se pasa al siguiente estado
            if (Properties.Duration != 0 && _elapsed > Properties.Duration)
                nextState = Properties.NextState;
		    else if (_nextWaypoint is null || StateMachine is null)
                nextState = Properties.NextState;
            else
            {
                float distance = Route.DistanceToWaypoint(StateMachine.Brain.Owner.Transform.Center, _nextWaypoint ?? Vector2.Zero);

                    if (distance < ArrivalDistance)
                        _nextWaypoint = Route.GetNextWaypoint(_nextWaypoint, Looping);
                    else if (_nextWaypoint is not null)
                        UpdateSteering(_nextWaypoint ?? Vector2.Zero);
            }
            // Incrementa el tiempo
            _elapsed += gameContext.DeltaTime;
            // Devuelve el siguiente estado
            return nextState;
	}

    /// <summary>
    ///     Actualiza el movimiento
    /// </summary>
    private void UpdateSteering(Vector2 target)
    {
        // Crea el controlador de movimientos y lo añade al manager
        if (_steering is null)
        {
            _steering = new Steering.ArriveSteering();
            if (StateMachine is not null)
                StateMachine.Brain.AgentSteeringManager.Add(_steering, 1);
        }
        // Cambia la dirección
        _steering.Target = target;
        _steering.ArrivalDistance = ArrivalDistance;
        _steering.SlowingDistance = SlowingDistance;
    }

    /// <summary>
    ///     Finaliza el estado
    /// </summary>
	public override void End()
	{
		// ... en esta caso simplemente implementa la interface
	}

    /// <summary>
    ///     Ruta que sigue el actor
    /// </summary>
    public WaypointRouteModel Route { get; } = route;

    /// <summary>
    ///     Distancia mínima a los puntos de ruta
    /// </summary>
    public float ArrivalDistance { get; set; } = 0.2f;

    /// <summary>
    ///     Distancia a la que empiez a frenar
    /// </summary>
    public float SlowingDistance { get; set; } = 1f;

    /// <summary>
    ///     Indica si la ruta se debe seguir en bucle
    /// </summary>
    public bool Looping { get; set; }
}