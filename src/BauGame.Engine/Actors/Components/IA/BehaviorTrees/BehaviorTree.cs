using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class BehaviorTree
{
    private BehaviorNode root;
    private NPC npc;

    public BehaviorTree(BehaviorNode rootNode, NPC npcOwner)
    {
        root = rootNode;
        npc = npcOwner;
    }

    public void Update(GameTime gameTime)
    {
        if (root != null)
        {
            root.Execute(npc, gameTime);
        }
    }

    public void Reset()
    {
        root?.Reset();
    }
}