using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class MultiTargetFollowBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 15;
        
    public List<Vector2> Targets { get; } = new List<Vector2>();
    public float SmoothSpeed { get; set; } = 3f;
    public Vector2 Offset { get; set; } = Vector2.Zero;
        
    public float MinZoom { get; set; } = 0.5f;
    public float MaxZoom { get; set; } = 2f;
    public float ZoomMargin { get; set; } = 100f;
    public float ZoomSmoothSpeed { get; set; } = 2f;
        
    private float _currentZoom;
    private Vector2 _currentCenter;

    public MultiTargetFollowBehavior(AbstractCameraBase camera) : base(camera)
    {
        _currentZoom = camera.State.Zoom;
        _currentCenter = camera.State.Transform.Position;
    }

    public void AddTarget(Vector2 target)
    {
        Targets.Add(target);
    }

    public void RemoveTarget(Vector2 target)
    {
        Targets.Remove(target);
    }

    public void ClearTargets()
    {
        Targets.Clear();
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (Targets.Count == 0) return;
            
        RectangleF bounds = CalculateTargetsBounds();
        Vector2 center = bounds.Center + Offset;
            
        float requiredZoom = CalculateRequiredZoom(bounds);
        requiredZoom = MathHelper.Clamp(requiredZoom, MinZoom, MaxZoom);
            
        _currentCenter = MathUtils.Lerp(_currentCenter, center, SmoothSpeed * deltaTime);
        _currentZoom = MathHelper.Lerp(_currentZoom, requiredZoom, ZoomSmoothSpeed * deltaTime);
            
        Camera.SetPosition(_currentCenter);
        Camera.SetZoom(_currentZoom);
    }

    private RectangleF CalculateTargetsBounds()
    {
        if (Targets.Count == 0) return RectangleF.Empty;
            
        float minX = Targets[0].X;
        float maxX = Targets[0].X;
        float minY = Targets[0].Y;
        float maxY = Targets[0].Y;
            
        for (int i = 1; i < Targets.Count; i++)
        {
            Vector2 target = Targets[i];
                
            if (target.X < minX) minX = target.X;
            if (target.X > maxX) maxX = target.X;
            if (target.Y < minY) minY = target.Y;
            if (target.Y > maxY) maxY = target.Y;
        }
            
        return new RectangleF(minX, minY, maxX - minX, maxY - minY);
    }

    private float CalculateRequiredZoom(RectangleF bounds)
    {
        var viewport = Camera.CameraViewport;
            
        float viewWidth = viewport.Width;
        float viewHeight = viewport.Height;
            
        float contentWidth = bounds.Width + ZoomMargin * 2f;
        float contentHeight = bounds.Height + ZoomMargin * 2f;
            
        if (contentWidth <= 0 || contentHeight <= 0) return MaxZoom;
            
        float zoomX = viewWidth / contentWidth;
        float zoomY = viewHeight / contentHeight;
            
        return System.Math.Min(zoomX, zoomY);
    }

    public override void Reset()
    {
        base.Reset();
        Targets.Clear();
        _currentZoom = Camera.State.Zoom;
        _currentCenter = Camera.State.Transform.Position;
    }
}
