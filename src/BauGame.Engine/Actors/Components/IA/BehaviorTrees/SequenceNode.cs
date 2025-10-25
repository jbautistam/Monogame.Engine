using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class SequenceNode : BehaviorNode
{
    private List<BehaviorNode> children;
    private int currentChildIndex;

    public SequenceNode(string name = "Sequence") : base(name)
    {
        children = new List<BehaviorNode>();
        currentChildIndex = 0;
    }

    public void AddChild(BehaviorNode child)
    {
        children.Add(child);
    }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        // Ejecutar hijos secuencialmente hasta que uno falle
        while (currentChildIndex < children.Count)
        {
            var child = children[currentChildIndex];
            var childStatus = child.Execute(npc, gameTime);

            switch (childStatus)
            {
                case BehaviorStatus.Success:
                    currentChildIndex++;
                    break;
                    
                case BehaviorStatus.Running:
                    return BehaviorStatus.Running;
                    
                case BehaviorStatus.Failure:
                    Reset(); // Resetear para próxima ejecución
                    return BehaviorStatus.Failure;
            }
        }

        // Todos los hijos tuvieron éxito
        Reset();
        return BehaviorStatus.Success;
    }

    public override void Reset()
    {
        base.Reset();
        currentChildIndex = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }
}