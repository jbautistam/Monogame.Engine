namespace GameEngine.Behavior
{
	public interface IBehavior<TState>
    {
        bool IsActive { get; set; }
        int Priority { get; }
        
        float? Duration { get; }
        float ElapsedTime { get; }
        bool IsComplete { get; }
        
        void Update(float deltaTime);
        TState Apply(TState current);
        void Reset();
    }
}