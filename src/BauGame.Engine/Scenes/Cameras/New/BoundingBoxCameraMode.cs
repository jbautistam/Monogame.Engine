public class BoundingBoxCameraMode : ICameraMode
{
    private Rectangle _bounds;
    private bool _isValid;

    public void SetBounds(Rectangle bounds)
    {
        _bounds = bounds;
        _isValid = true;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (!_isValid)
        {
            camera._desiredPosition = camera.Position;
            return;
        }

        // Centro del rectángulo
        Vector2 center = new Vector2(_bounds.Center.X, _bounds.Center.Y);

        // Ajustar zoom si es necesario (opcional)
        // Aquí asumimos zoom fijo; podrías calcular zoom dinámico

        camera._desiredPosition = center;

        // Aplicar clamp si está activo
        if (camera.UseClamp && camera.WorldBounds.HasValue)
        {
            var world = camera.WorldBounds.Value;
            var halfView = new Vector2(camera.ViewportWidth / (2f * camera.Zoom), camera.ViewportHeight / (2f * camera.Zoom));

            camera._desiredPosition.X = MathHelper.Clamp(camera._desiredPosition.X, world.Left + halfView.X, world.Right - halfView.X);
            camera._desiredPosition.Y = MathHelper.Clamp(camera._desiredPosition.Y, world.Top + halfView.Y, world.Bottom - halfView.Y);
        }
    }

    public void Reset() => _isValid = false;
}
