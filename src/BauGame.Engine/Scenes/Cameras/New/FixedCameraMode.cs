// FixedCameraMode.cs
public class FixedCameraMode : ICameraMode
{
    public Vector2 FixedPosition { get; set; }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        camera._desiredPosition = FixedPosition;
    }

    public void Reset() { }
}

