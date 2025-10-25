using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class IdleState : INPCState
{
    private float idleTimer;
    private float maxIdleTime;

    public void Enter(NPC npc)
    {
        npc.Velocity = Vector2.Zero;
        maxIdleTime = Random.Shared.Next(2, 6); // Tiempo aleatorio de inactividad
        idleTimer = 0f;
        // Puedes reproducir animación de idle aquí
    }

    public void Execute(NPC npc, GameTime gameTime)
    {
        idleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Si está cerca del jugador, cambiar a estado de interacción
        if (npc.IsPlayerNearby())
        {
            npc.StateMachine.ChangeState(new TalkingState());
            return;
        }

        // Si ha estado inactivo mucho tiempo, puede empezar a caminar
        if (idleTimer >= maxIdleTime)
        {
            npc.StateMachine.ChangeState(new WalkingState());
        }
    }

    public void Exit(NPC npc)
    {
        // Limpiar recursos si es necesario
    }
}