public class FreeCameraMode : ICameraMode
{
    public float MoveSpeed { get; set; } = 500f;

    public void Update(GameTime gameTime, Camera2D camera)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var kb = Keyboard.GetState();

        Vector2 input = Vector2.Zero;
        if (kb.IsKeyDown(Keys.A)) input.X -= 1;
        if (kb.IsKeyDown(Keys.D)) input.X += 1;
        if (kb.IsKeyDown(Keys.W)) input.Y -= 1;
        if (kb.IsKeyDown(Keys.S)) input.Y += 1;

        if (input.LengthSquared() > 0)
            input.Normalize();

        camera._desiredPosition += input * MoveSpeed * dt;

        // Clamp opcional
        if (camera.UseClamp && camera.WorldBounds.HasValue)
        {
            var bounds = camera.WorldBounds.Value;
            var halfView = new Vector2(camera.ViewportWidth / (2f * camera.Zoom), camera.ViewportHeight / (2f * camera.Zoom));

            camera._desiredPosition.X = MathHelper.Clamp(camera._desiredPosition.X, bounds.Left + halfView.X, bounds.Right - halfView.X);
            camera._desiredPosition.Y = MathHelper.Clamp(camera._desiredPosition.Y, bounds.Top + halfView.Y, bounds.Bottom - halfView.Y);
        }
    }

    public void Reset() { }
}
