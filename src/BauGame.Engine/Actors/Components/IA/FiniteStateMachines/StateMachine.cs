using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class StateMachine
{
    public INPCState CurrentState { get; private set; }
    public NPC NPC { get; private set; }

    public StateMachine(NPC npc)
    {
        NPC = npc;
    }

    public void ChangeState(INPCState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit(NPC);
            
        CurrentState = newState;
        CurrentState.Enter(NPC);
    }

    public void Update(GameTime gameTime)
    {
        CurrentState?.Execute(NPC, gameTime);
    }
}