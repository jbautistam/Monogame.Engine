using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;

namespace GameEngine.Layers.Behaviors;

public class FlashBehavior : LayerBehaviorBase
{
    public override int Priority => 60;
        
    public Color FlashColor { get; set; } = Color.White;
    public float FadeOutSpeed { get; set; } = 5f;
        
    private float _currentIntensity;

    public FlashBehavior(Layer layer, float duration) : base(layer)
    {
        Duration = duration;
        _currentIntensity = 1f;
    }

    protected override void OnUpdate(float deltaTime)
    {
        _currentIntensity -= FadeOutSpeed * deltaTime;
            
        if (_currentIntensity <= 0)
        {
            _currentIntensity = 0;
            IsComplete = true;
        }
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        Color flash = new Color(
            (byte)(FlashColor.R * _currentIntensity),
            (byte)(FlashColor.G * _currentIntensity),
            (byte)(FlashColor.B * _currentIntensity),
            (byte)(255 * _currentIntensity)
        );
            
        foreach (var command in commands)
        {
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Color = BlendAdditive(texCmd.Color, flash);
            }
            else if (command is TextRenderCommand textCmd)
            {
                textCmd.Color = BlendAdditive(textCmd.Color, flash);
            }
        }
    }

    private Color BlendAdditive(Color baseColor, Color blend)
    {
        return new Color(
            (byte)System.Math.Min(255, baseColor.R + blend.R),
            (byte)System.Math.Min(255, baseColor.G + blend.G),
            (byte)System.Math.Min(255, baseColor.B + blend.B),
            baseColor.A
        );
    }
}
