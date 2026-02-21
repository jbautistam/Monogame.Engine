using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class BoundsConstraintBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 100;
        
    public bool SoftConstraint { get; set; } = false;
    public float Softness { get; set; } = 0.5f;

    public BoundsConstraintBehavior(AbstractCameraBase camera) : base(camera)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        var viewport = Camera.CameraViewport;
        if (viewport == null) return;
            
        var visible = Camera.State.GetVisibleBounds(viewport.ToViewport(Camera.Director.Viewport));
        var world = Camera.Director.Scene.WorldBounds;
            
        if (world.IsEmpty) return;
            
        float halfVisibleWidth = visible.Width * 0.5f;
        float halfVisibleHeight = visible.Height * 0.5f;
            
        Vector2 targetPos = Camera.State.Transform.Position;
            
        float minX = world.Left + halfVisibleWidth;
        float maxX = world.Right - halfVisibleWidth;
        float minY = world.Top + halfVisibleHeight;
        float maxY = world.Bottom - halfVisibleHeight;
            
        if (SoftConstraint)
        {
            if (targetPos.X < minX)
            {
                float over = minX - targetPos.X;
                targetPos.X = minX - over * Softness;
            }
            else if (targetPos.X > maxX)
            {
                float over = targetPos.X - maxX;
                targetPos.X = maxX + over * Softness;
            }
                
            if (targetPos.Y < minY)
            {
                float over = minY - targetPos.Y;
                targetPos.Y = minY - over * Softness;
            }
            else if (targetPos.Y > maxY)
            {
                float over = targetPos.Y - maxY;
                targetPos.Y = maxY + over * Softness;
            }
        }
        else
        {
            targetPos.X = MathHelper.Clamp(targetPos.X, minX, maxX);
            targetPos.Y = MathHelper.Clamp(targetPos.Y, minY, maxY);
        }
            
        Camera.SetPosition(targetPos);
    }
}
