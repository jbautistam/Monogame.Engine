using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class PatrollingState : INPCState
{
    private enum PatrolSubState
    {
        MovingToWaypoint,
        WaitingAtWaypoint
    }

    private PatrolSubState currentSubState;
    private float waitTimer;
    private float waitDuration;
    private Vector2 targetPosition;
    private bool hasReachedWaypoint;

    public void Enter(NPC npc)
    {
        currentSubState = PatrolSubState.MovingToWaypoint;
        hasReachedWaypoint = false;
        waitTimer = 0f;
        waitDuration = 0f;

        // Obtener el waypoint actual
        var currentWaypoint = npc.PatrolRoute?.GetCurrentWaypoint();
        if (currentWaypoint != null)
        {
            targetPosition = currentWaypoint.Position;
            SetDirectionToTarget(npc, targetPosition);
        }
        else
        {
            // Si no hay ruta, volver a idle
            npc.StateMachine.ChangeState(new IdleState());
        }
    }

    public void Execute(NPC npc, GameTime gameTime)
    {
        // Verificar si el jugador está cerca para cambiar a estado de interacción
        if (npc.IsPlayerNearby() && npc.CanBeInterrupted)
        {
            npc.StateMachine.ChangeState(new TalkingState());
            return;
        }

        switch (currentSubState)
        {
            case PatrolSubState.MovingToWaypoint:
                MoveTowardsWaypoint(npc, gameTime);
                break;

            case PatrolSubState.WaitingAtWaypoint:
                WaitForDuration(npc, gameTime);
                break;
        }
    }

    public void Exit(NPC npc)
    {
        npc.Velocity = Vector2.Zero;
    }

    private void MoveTowardsWaypoint(NPC npc, GameTime gameTime)
    {
        if (npc.PatrolRoute == null) return;

        float distanceToTarget = npc.PatrolRoute.DistanceToCurrentWaypoint(npc.Position);

        // Si está cerca del waypoint
        if (distanceToTarget < npc.ArrivalDistance)
        {
            // Detenerse en el waypoint
            npc.Position = npc.PatrolRoute.GetCurrentWaypoint().Position;
            npc.Velocity = Vector2.Zero;
            hasReachedWaypoint = true;

            // Verificar si hay tiempo de espera en este waypoint
            float waitTime = npc.PatrolRoute.GetCurrentWaypoint().WaitTime;
            if (waitTime > 0)
            {
                currentSubState = PatrolSubState.WaitingAtWaypoint;
                waitTimer = 0f;
                waitDuration = waitTime;
                
                // Aquí podrías reproducir animación de espera
                PlayWaypointAnimation(npc);
            }
            else
            {
                // Ir al siguiente waypoint inmediatamente
                MoveToNextWaypoint(npc);
            }
        }
        else
        {
            // Continuar moviéndose hacia el waypoint
            SetDirectionToTarget(npc, targetPosition);
            npc.Position += npc.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private void WaitForDuration(NPC npc, GameTime gameTime)
    {
        waitTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Durante la espera, el NPC puede hacer cosas como mirar alrededor
        LookAround(npc, gameTime);

        if (waitTimer >= waitDuration)
        {
            // Terminar espera y moverse al siguiente waypoint
            currentSubState = PatrolSubState.MovingToWaypoint;
            MoveToNextWaypoint(npc);
        }
    }

    private void MoveToNextWaypoint(NPC npc)
    {
        if (npc.PatrolRoute == null) return;

        // Obtener el siguiente waypoint
        var nextWaypoint = npc.PatrolRoute.GetNextWaypoint();
        if (nextWaypoint != null)
        {
            targetPosition = nextWaypoint.Position;
            SetDirectionToTarget(npc, targetPosition);
            
            // Aquí podrías reproducir animación de movimiento
            PlayMovementAnimation(npc);
        }
    }

    private void SetDirectionToTarget(NPC npc, Vector2 target)
    {
        Vector2 direction = target - npc.Position;
        if (direction.Length() > 0)
        {
            direction.Normalize();
            npc.Velocity = direction * npc.Speed;
            
            // Actualizar dirección para animaciones/sprites
            npc.FacingDirection = direction;
        }
        else
        {
            npc.Velocity = Vector2.Zero;
        }
    }

    private void PlayWaypointAnimation(NPC npc)
    {
        // Ejemplo: reproducir animación de espera
        var currentWaypoint = npc.PatrolRoute?.GetCurrentWaypoint();
        if (currentWaypoint != null && !string.IsNullOrEmpty(currentWaypoint.Animation))
        {
            // npc.AnimationPlayer?.Play(currentWaypoint.Animation);
        }
        else
        {
            // npc.AnimationPlayer?.Play("idle");
        }
    }

    private void PlayMovementAnimation(NPC npc)
    {
        // npc.AnimationPlayer?.Play("walk");
    }

    private void LookAround(NPC npc, GameTime gameTime)
    {
        // Animación opcional: hacer que el NPC mire alrededor mientras espera
        // Por ejemplo, cambiar ligeramente la dirección de mirada
    }
}