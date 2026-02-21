using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;
using GameEngine.Math;

namespace GameEngine.Layers.Behaviors;

public class ParallaxBehavior : LayerBehaviorBase
{
    public override int Priority => 10;
        
    public Vector2 Factor { get; set; } = new Vector2(0.5f, 0.5f);
    public Vector2 BasePosition { get; set; } = Vector2.Zero;

    public ParallaxBehavior(Layer layer) : base(layer)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        Vector2 cameraPos = camera.State.Transform.Position;
        Vector2 parallaxOffset = BasePosition - cameraPos * Factor;
        Vector2 delta = parallaxOffset - BasePosition + cameraPos;
            
        foreach (var command in commands)
        {
            command.Bounds = new RectangleF(
                command.Bounds.Position + delta,
                command.Bounds.Size
            );
                
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Position += delta;
            }
            else if (command is TextRenderCommand textCmd)
            {
                textCmd.Position += delta;
            }
        }
    }
}
