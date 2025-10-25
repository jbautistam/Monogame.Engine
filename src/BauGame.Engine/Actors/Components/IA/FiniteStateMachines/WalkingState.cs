using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class WalkingState : INPCState
{
    private float walkTimer;
    private float maxWalkTime;
    private Vector2 direction;

    public void Enter(NPC npc)
    {
        maxWalkTime = Random.Shared.Next(3, 8);
        walkTimer = 0f;
        
        // Dirección aleatoria
        float angle = MathHelper.ToRadians(Random.Shared.Next(0, 360));
        direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        npc.Velocity = direction * npc.Speed;
        
        // Puedes reproducir animación de caminar aquí
    }

    public void Execute(NPC npc, GameTime gameTime)
    {
        walkTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Verificar colisiones y cambiar dirección si es necesario
        if (npc.WillCollide())
        {
            // Cambiar dirección aleatoria
            float angle = MathHelper.ToRadians(Random.Shared.Next(0, 360));
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            npc.Velocity = direction * npc.Speed;
        }

        // Si está cerca del jugador, ir a hablar
        if (npc.IsPlayerNearby())
        {
            npc.StateMachine.ChangeState(new TalkingState());
            return;
        }

        // Si ha caminado suficiente tiempo, volver a idle
        if (walkTimer >= maxWalkTime)
        {
            npc.StateMachine.ChangeState(new IdleState());
        }
    }

    public void Exit(NPC npc)
    {
        npc.Velocity = Vector2.Zero;
    }
}