// FollowCameraMode.cs
public class FollowCameraMode : ICameraMode
{
    private readonly Camera2D _camera;
    private readonly List<Vector2> _targets = new List<Vector2>();

    public FollowCameraMode(Camera2D camera)
    {
        _camera = camera;
    }

    public void AddTarget(Vector2 target) => _targets.Add(target);
    public void ClearTargets() => _targets.Clear();

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (_targets.Count == 0) return;

        Vector2 targetCenter = _targets.Aggregate(Vector2.Zero, (sum, t) => sum + t) / _targets.Count;
        ApplyDeadZoneAndClamp(camera, targetCenter);
    }

    private void ApplyDeadZoneAndClamp(Camera2D cam, Vector2 targetCenter)
    {
        if (cam.DeadZone.HasValue)
        {
            Vector2 screenTarget = cam.WorldToScreenPure(targetCenter);
            Rectangle deadRect = cam.DeadZone.Value;
            Vector2 deadCenter = new Vector2(deadRect.Center.X, deadRect.Center.Y);
            Vector2 offset = screenTarget - deadCenter;

            if (Math.Abs(offset.X) > deadRect.Width * 0.5f || Math.Abs(offset.Y) > deadRect.Height * 0.5f)
            {
                cam._desiredPosition = targetCenter - cam.ScreenToWorldPure(deadCenter);
            }
            else
            {
                cam._desiredPosition = cam.Position;
            }
        }
        else
        {
            cam._desiredPosition = targetCenter;
        }

        // Clamp al mundo
        if (cam.UseClamp && cam.WorldBounds.HasValue)
        {
            var bounds = cam.WorldBounds.Value;
            var halfView = new Vector2(cam.ViewportWidth / (2f * cam.Zoom), cam.ViewportHeight / (2f * cam.Zoom));

            if (bounds.Width > cam.ViewportWidth / cam.Zoom)
                cam._desiredPosition.X = MathHelper.Clamp(cam._desiredPosition.X, bounds.Left + halfView.X, bounds.Right - halfView.X);
            else
                cam._desiredPosition.X = bounds.Center.X;

            if (bounds.Height > cam.ViewportHeight / cam.Zoom)
                cam._desiredPosition.Y = MathHelper.Clamp(cam._desiredPosition.Y, bounds.Top + halfView.Y, bounds.Bottom - halfView.Y);
            else
                cam._desiredPosition.Y = bounds.Center.Y;
        }
    }

    public void Reset() => _targets.Clear();
}
