using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class NPC
{
    // ... propiedades existentes ...

    // Nuevas propiedades para patrulla
    public PatrolRoute PatrolRoute { get; set; }
    public float ArrivalDistance { get; set; } = 5f; // Distancia para considerar que llegó al waypoint
    public Vector2 FacingDirection { get; set; } = Vector2.Zero; // Dirección a la que mira
    public bool CanBeInterrupted { get; set; } = true; // Si puede ser interrumpido por el jugador

    // Constructor actualizado
    public NPC(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
        StateMachine = new StateMachine(this);
        Speed = 50f;
        ArrivalDistance = 5f;
        
        // Iniciar en estado idle o patrolling según si tiene ruta
        StateMachine.ChangeState(new IdleState());
    }

    // Método para asignar una ruta de patrulla
    public void SetPatrolRoute(PatrolRoute route)
    {
        PatrolRoute = route;
        if (route != null && route.Waypoints.Count > 0)
        {
            // Cambiar a estado de patrulla
            StateMachine.ChangeState(new PatrollingState());
        }
    }

    // Método para detener la patrulla y volver a idle
    public void StopPatrolling()
    {
        PatrolRoute = null;
        if (StateMachine.CurrentState is PatrollingState)
        {
            StateMachine.ChangeState(new IdleState());
        }
    }

    // ... resto de métodos existentes ...
}