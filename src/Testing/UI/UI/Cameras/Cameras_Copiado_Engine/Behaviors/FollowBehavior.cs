using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class FollowBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 10;
        
    public Vector2 Target { get; set; }
    public float SmoothSpeed { get; set; } = 5f;
    public Vector2 Offset { get; set; } = Vector2.Zero;
        
    public Vector2 DeadZone { get; set; } = Vector2.Zero;
    public bool UseDeadZone { get; set; } = false;
        
    private Vector2 _currentVelocity;

    public FollowBehavior(AbstractCameraBase camera) : base(camera)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        Vector2 targetPos = Target + Offset;
        Vector2 currentPos = Camera.State.Transform.Position;
            
        if (UseDeadZone)
        {
            Vector2 diff = targetPos - currentPos;
                
            if (System.Math.Abs(diff.X) < DeadZone.X * 0.5f)
            {
                targetPos.X = currentPos.X;
            }
            else
            {
                float sign = System.Math.Sign(diff.X);
                targetPos.X -= sign * DeadZone.X * 0.5f;
            }
                
            if (System.Math.Abs(diff.Y) < DeadZone.Y * 0.5f)
            {
                targetPos.Y = currentPos.Y;
            }
            else
            {
                float sign = System.Math.Sign(diff.Y);
                targetPos.Y -= sign * DeadZone.Y * 0.5f;
            }
        }
            
        Vector2 newPos = MathUtils.Lerp(currentPos, targetPos, SmoothSpeed * deltaTime);
        Camera.SetPosition(newPos);
    }
}
