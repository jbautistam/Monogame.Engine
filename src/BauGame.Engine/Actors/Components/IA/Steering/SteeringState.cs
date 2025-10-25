namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

public abstract class SteeringState
{
    protected Agent agent;
    protected Dictionary<ISteeringBehavior, float> behaviorWeights;

    public SteeringState(Agent agent)
    {
        this.agent = agent;
        behaviorWeights = new Dictionary<ISteeringBehavior, float>();
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void Update(float deltaTime);

    public Dictionary<ISteeringBehavior, float> GetBehaviors() => behaviorWeights;

    protected void SetBehaviorWeight(ISteeringBehavior behavior, float weight)
    {
        if (behaviorWeights.ContainsKey(behavior))
        {
            behaviorWeights[behavior] = weight;
        }
        else
        {
            behaviorWeights.Add(behavior, weight);
        }
    }
}