using GameEngine.Cameras;
using GameEngine.Rendering;

namespace GameEngine.Layers;

public abstract class LayerBehaviorBase : ILayerBehavior
{
    protected readonly Layer Layer;
        
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
        
    public float? Duration { get; protected set; }
    public float ElapsedTime { get; private set; }
    public bool IsComplete { get; protected set; }

    protected LayerBehaviorBase(Layer layer)
    {
        Layer = layer;
    }

    public void Update(float deltaTime)
    {
        if (IsActive == false) return;
        if (IsComplete) return;

        ElapsedTime += deltaTime;

        if (Duration.HasValue && ElapsedTime >= Duration.Value)
        {
            IsComplete = true;
            OnComplete();
            return;
        }

        OnUpdate(deltaTime);
    }

    protected abstract void OnUpdate(float deltaTime);
    public abstract void Apply(List<RenderCommand> commands, AbstractCameraBase camera);

    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }

    protected virtual void OnComplete() { }
}