using GameEngine.Cameras;
using GameEngine.Rendering;

namespace GameEngine.Layers;

public interface ILayerBehavior
{
    bool IsActive { get; set; }
    int Priority { get; }
        
    float? Duration { get; }
    float ElapsedTime { get; }
    bool IsComplete { get; }
        
    void Update(float deltaTime);
    void Apply(List<RenderCommand> commands, AbstractCameraBase camera);
    void Reset();
}