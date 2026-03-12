using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class DollyZoomBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 20;
        
    public Vector2 Target { get; set; }
    public float StartDistance { get; set; } = 300f;
    public float EndDistance { get; set; } = 150f;
    public float StartZoom { get; set; } = 1f;
    public float EndZoom { get; set; } = 2f;
        
    private Vector2 _startPosition;
    private float _initialZoom;

    public DollyZoomBehavior(AbstractCameraBase camera, float duration) : base(camera)
    {
        Duration = duration;
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (ElapsedTime == 0f)
        {
            _startPosition = Camera.State.Transform.Position;
            _initialZoom = Camera.State.Zoom;
        }
            
        float t = ElapsedTime / Duration.Value;
        t = MathHelper.SmoothStep(0f, 1f, t);
            
        float currentDistance = MathHelper.Lerp(StartDistance, EndDistance, t);
        float currentZoom = MathHelper.Lerp(StartZoom, EndZoom, t);
            
        Vector2 toTarget = Target - _startPosition;
        toTarget.Normalize();
            
        Vector2 newPos = Target - toTarget * currentDistance;
            
        Camera.SetPosition(newPos);
        Camera.SetZoom(currentZoom);
    }

    protected override void OnComplete()
    {
        Camera.SetZoom(EndZoom);
    }

    public override void Reset()
    {
        base.Reset();
        Camera.SetZoom(_initialZoom);
    }
}
