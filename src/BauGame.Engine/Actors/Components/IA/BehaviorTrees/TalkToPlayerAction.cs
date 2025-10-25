using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.BehaviorTrees;

public class TalkToPlayerAction : BehaviorNode
{
    private bool dialogueStarted = false;

    public TalkToPlayerAction(string name = "TalkToPlayer") : base(name) { }

    public override BehaviorStatus Execute(NPC npc, GameTime gameTime)
    {
        if (!dialogueStarted)
        {
            npc.StartDialogue();
            dialogueStarted = true;
        }

        if (npc.IsDialogueFinished())
        {
            Reset();
            return BehaviorStatus.Success;
        }

        return BehaviorStatus.Running;
    }

    public override void Reset()
    {
        base.Reset();
        dialogueStarted = false;
        // No necesitamos resetear el diálogo aquí
    }
}