using Microsoft.Xna.Framework;
using GameEngine.Rendering;
using GameEngine.Cameras;

namespace GameEngine.Layers.Behaviors;

public class TintBehavior : LayerBehaviorBase
{
    public override int Priority => 50;
        
    public Color Tint { get; set; } = Color.White;
    public float Intensity { get; set; } = 1f;

    public TintBehavior(Layer layer) : base(layer)
    {
    }

    protected override void OnUpdate(float deltaTime)
    {
    }

    public override void Apply(List<RenderCommand> commands, AbstractCameraBase camera)
    {
        Color blended = new Color(
            (byte)(Tint.R * Intensity),
            (byte)(Tint.G * Intensity),
            (byte)(Tint.B * Intensity),
            (byte)(Tint.A * Intensity)
        );
            
        foreach (var command in commands)
        {
            if (command is TextureRenderCommand texCmd)
            {
                texCmd.Color = BlendColors(texCmd.Color, blended);
            }
            else if (command is TextRenderCommand textCmd)
            {
                textCmd.Color = BlendColors(textCmd.Color, blended);
            }
        }
    }

    private Color BlendColors(Color baseColor, Color tint)
    {
        return new Color(
            (byte)((baseColor.R * tint.R) / 255),
            (byte)((baseColor.G * tint.G) / 255),
            (byte)((baseColor.B * tint.B) / 255),
            (byte)((baseColor.A * tint.A) / 255)
        );
    }
}
