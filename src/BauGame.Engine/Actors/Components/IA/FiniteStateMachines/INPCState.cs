using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public interface INPCState
{
    void Enter(NPC npc);
    void Execute(NPC npc, GameTime gameTime);
    void Exit(NPC npc);
}