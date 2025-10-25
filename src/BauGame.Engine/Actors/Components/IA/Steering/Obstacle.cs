using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public class Obstacle
{
    public Vector2 Position;
    public float Radius;

    public Obstacle(Vector2 position, float radius)
    {
        Position = position;
        Radius = radius;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        // Dibuja un círculo simple (muy básico, puedes mejorar con geometría o un sprite)
        int segments = 32;
        for (int i = 0; i < segments; i++)
        {
            float angle = (float)(i * (2 * Math.PI / segments));
            float nextAngle = (float)((i + 1) * (2 * Math.PI / segments));

            Vector2 p1 = Position + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Radius;
            Vector2 p2 = Position + new Vector2((float)Math.Cos(nextAngle), (float)Math.Sin(nextAngle)) * Radius;

            // Dibuja una línea entre p1 y p2
            // (MonoGame no tiene primitivas directas, así que dibujamos una imagen estirada)
            float length = Vector2.Distance(p1, p2);
            float rotation = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);

            spriteBatch.Draw(pixel, p1, null, Color.Gray, rotation, Vector2.Zero, new Vector2(length, 2f), SpriteEffects.None, 0f);
        }
    }
}