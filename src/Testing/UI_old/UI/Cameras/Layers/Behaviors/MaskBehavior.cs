using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;

namespace GameEngine.Layers.Behaviors;

public class MaskBehavior : LayerBehaviorBase
{
    public override int Priority => 70;
        
    public enum MaskShape { Rectangle, Circle, Ellipse }
        
    public MaskShape Shape { get; set; } = MaskShape.Circle;
    public Vector2 Center { get; set; }
    public float Radius { get; set; } = 100f;
    public Vector2 Size { get; set; } = new Vector2(200, 200);
    public bool Invert { get; set; } = false;
    public float Softness { get; set; } = 0f;

    public MaskBehavior(Layer layer) : base(layer)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        for (int i = commands.Count - 1; i >= 0; i--)
        {
            var command = commands[i];
            bool inside = IsInside(command.Bounds.Center);
                
            if (Invert)
            {
                inside = !inside;
            }
                
            if (inside == false)
            {
                if (Softness > 0 && IsNearEdge(command.Bounds.Center))
                {
                    ApplySoftness(command);
                }
                else
                {
                    commands.RemoveAt(i);
                }
            }
        }
    }

    private bool IsInside(Vector2 point)
    {
        switch (Shape)
        {
            case MaskShape.Rectangle:
                float halfW = Size.X * 0.5f;
                float halfH = Size.Y * 0.5f;
                return point.X >= Center.X - halfW && point.X <= Center.X + halfW &&
                        point.Y >= Center.Y - halfH && point.Y <= Center.Y + halfH;
                           
            case MaskShape.Circle:
                return (point - Center).LengthSquared() <= Radius * Radius;
                    
            case MaskShape.Ellipse:
                float dx = (point.X - Center.X) / (Size.X * 0.5f);
                float dy = (point.Y - Center.Y) / (Size.Y * 0.5f);
                return dx * dx + dy * dy <= 1f;
        }
            
        return false;
    }

    private bool IsNearEdge(Vector2 point)
    {
        return true;
    }

    private void ApplySoftness(RenderCommand command)
    {
        if (command is TextureRenderCommand texCmd)
        {
            texCmd.Color = new Color(
                texCmd.Color.R,
                texCmd.Color.G,
                texCmd.Color.B,
                (byte)(texCmd.Color.A * 0.5f)
            );
        }
    }
}
