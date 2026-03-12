using GameEngine.Cameras;
using GameEngine.Math;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;

namespace GameEngine.Entities;

public abstract class Actor : IRenderable
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
        
    protected Actor(Vector2 position)
    {
        Position = position;
    }

    public abstract void Update(float deltaTime);
        
    public void CollectRenderCommands(AbstractCameraBase camera, List<RenderCommand> commands)
    {
        if (IsActive && IsVisible && IsVisibleAtCamera(camera))
        {
            IEnumerable<RenderCommand> generatedCommands;
            
            if (camera.Type ==AbstractCameraBase.CameraType.Screen)
            {
                generatedCommands = GenerateScreenCommands();
            }
            else
            {
                generatedCommands = GenerateWorldCommands();
            }
            
            foreach (var command in generatedCommands)
            {
                commands.Add(command);
            }
        }
    }

    protected abstract IEnumerable<RenderCommand> GenerateWorldCommands();
    protected abstract IEnumerable<RenderCommand> GenerateScreenCommands();

    public abstract RectangleF GetRenderBounds();

    /// <summary>
    ///     Comprueba si es visible en una cámara
    /// </summary>
    public virtual bool IsVisibleAtCamera(AbstractCameraBase camera) => camera.IsInView(GetRenderBounds());
}