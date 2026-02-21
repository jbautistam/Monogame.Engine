using GameEngine.Cameras;
using GameEngine.Math;

namespace GameEngine.Rendering;

public interface IRenderable
{
    void CollectRenderCommands(AbstractCameraBase camera, List<RenderCommand> commands);
    RectangleF GetRenderBounds();
    bool IsVisibleAtCamera(AbstractCameraBase camera);
}