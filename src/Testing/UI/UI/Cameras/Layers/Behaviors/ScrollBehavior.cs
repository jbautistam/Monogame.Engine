using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;
using GameEngine.Math;

namespace GameEngine.Layers.Behaviors;

public class ScrollBehavior : LayerBehaviorBase
{
    public override int Priority => 20;
        
    public Vector2 Speed { get; set; } = Vector2.Zero;
    public bool Wrap { get; set; } = false;
    public Vector2 WrapBounds { get; set; } = Vector2.Zero;
        
    private Vector2 _currentOffset;

    public ScrollBehavior(Layer layer) : base(layer)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        _currentOffset += Speed * deltaTime;
            
        if (Wrap && WrapBounds != Vector2.Zero)
        {
            _currentOffset.X = _currentOffset.X % WrapBounds.X;
            _currentOffset.Y = _currentOffset.Y % WrapBounds.Y;
                
            if (_currentOffset.X < 0) _currentOffset.X += WrapBounds.X;
            if (_currentOffset.Y < 0) _currentOffset.Y += WrapBounds.Y;
        }
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        foreach (var command in commands)
        {
            command.Bounds = new RectangleF(
                command.Bounds.Position + _currentOffset,
                command.Bounds.Size
            );
                
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Position += _currentOffset;
            }
            else if (command is TextRenderCommand textCmd)
            {
                textCmd.Position += _currentOffset;
            }
        }
    }

    public override void Reset()
    {
        base.Reset();
        _currentOffset = Vector2.Zero;
    }
}
