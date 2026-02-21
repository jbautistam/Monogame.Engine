using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;
using GameEngine.Math;

namespace GameEngine.Layers.Behaviors;

public class WaveDistortionBehavior : LayerBehaviorBase
{
    public override int Priority => 30;
        
    public float Amplitude { get; set; } = 5f;
    public float Frequency { get; set; } = 0.1f;
    public float Speed { get; set; } = 2f;
    public bool Vertical { get; set; } = false;
        
    private float _phase;

    public WaveDistortionBehavior(Layer layer) : base(layer)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
        _phase += Speed * deltaTime;
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        foreach (var command in commands)
        {
            float pos = Vertical ? command.Bounds.Position.Y : command.Bounds.Position.X;
            float wave = MathF.Sin(pos * Frequency + _phase) * Amplitude;
                
            Vector2 offset = Vertical ? new Vector2(0, wave) : new Vector2(wave, 0);
                
            command.Bounds = new RectangleF(
                command.Bounds.Position + offset,
                command.Bounds.Size
            );
                
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Position += offset;
            }
            else if (command is TextRenderCommand textCmd)
            {
                textCmd.Position += offset;
            }
        }
    }
}
