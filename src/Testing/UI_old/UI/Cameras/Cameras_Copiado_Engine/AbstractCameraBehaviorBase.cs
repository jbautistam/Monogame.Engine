namespace GameEngine.Cameras;

public abstract class AbstractCameraBehaviorBase
{
    protected readonly AbstractCameraBase Camera;
        
    public bool IsActive { get; set; } = true;
    public abstract int Priority { get; }
        
    public float? Duration { get; protected set; }
    public float ElapsedTime { get; private set; }
    public bool IsComplete { get; protected set; }

    protected AbstractCameraBehaviorBase(AbstractCameraBase camera)
    {
        Camera = camera;
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

    public virtual void Reset()
    {
        ElapsedTime = 0f;
        IsComplete = false;
        IsActive = true;
    }

    protected virtual void OnComplete() { }
}
