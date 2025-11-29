public class LookAtCameraMode : ICameraMode
{
    public Vector2 Target { get; set; }
    public float Distance { get; set; } = 300f;
    public float AngleRadians { get; set; } = 0f; // 0 = arriba, π/2 = derecha, etc.

    public void Update(GameTime gameTime, Camera2D camera)
    {
        Vector2 offset = new Vector2(
            (float)Math.Cos(AngleRadians),
            (float)Math.Sin(AngleRadians)
        ) * Distance;

        camera._desiredPosition = Target + offset;
    }

    public void Reset() { }
}
