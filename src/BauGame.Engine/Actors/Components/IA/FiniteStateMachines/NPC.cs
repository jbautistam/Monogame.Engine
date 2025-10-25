using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.FiniteStateMachines;

public class NPC
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Speed { get; set; } = 50f;
    public StateMachine StateMachine { get; private set; }
    
    // Referencia al jugador (para detección de proximidad)
    public Player Player { get; set; }
    
    // Sistema de diálogo
    public DialogueSystem DialogueSystem { get; set; }

    public NPC(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
        StateMachine = new StateMachine(this);
        
        // Iniciar en estado idle
        StateMachine.ChangeState(new IdleState());
    }

    public void Update(GameTime gameTime)
    {
        StateMachine.Update(gameTime);
        
        // Actualizar posición
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

    // Métodos auxiliares para los estados
    public bool IsPlayerNearby()
    {
        if (Player == null) return false;
        return Vector2.Distance(Position, Player.Position) < 100f; // Radio de detección
    }

    public bool WillCollide()
    {
        // Implementar lógica de detección de colisiones
        // Por ejemplo, verificar contra tiles del mapa
        return false; // Placeholder
    }

    public void StartDialogue()
    {
        DialogueSystem?.StartDialogue(this);
    }

    public bool IsDialogueFinished()
    {
        return DialogueSystem?.IsFinished ?? true;
    }

    public void EndDialogue()
    {
        DialogueSystem?.EndDialogue();
    }
}