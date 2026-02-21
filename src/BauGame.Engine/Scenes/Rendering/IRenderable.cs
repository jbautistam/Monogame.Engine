using Bau.Libraries.BauGame.Engine.Entities.Common;
using Bau.Libraries.BauGame.Engine.Scenes.CamerasNew;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

public interface IRenderable
{
    void CollectRenderCommands(AbstractCameraBase camera, List<RenderCommand> commands);
    RectangleF GetRenderBounds();
    bool IsVisibleAtCamera(AbstractCameraBase camera);
}