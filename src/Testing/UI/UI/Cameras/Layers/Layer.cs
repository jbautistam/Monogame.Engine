using GameEngine.Entities;
using GameEngine.Cameras;
using GameEngine.Rendering;

namespace GameEngine.Layers;

public class Layer
{
    public string Name { get; }
    public int DrawOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsVisible { get; set; } = true;
        
    private readonly List<Actor> _actors = new List<Actor>();
    private readonly List<ILayerBehavior> _behaviors = new List<ILayerBehavior>();
        
    public IReadOnlyList<Actor> Actors => _actors;
    public IReadOnlyList<ILayerBehavior> Behaviors => _behaviors;
        
    public uint CameraMask { get; set; } = 0xFFFFFFFF;

    public Layer(string name, int drawOrder)
    {
        Name = name;
        DrawOrder = drawOrder;
    }

    public void AddActor(Actor actor)
    {
        _actors.Add(actor);
    }

    public void RemoveActor(Actor actor)
    {
        _actors.Remove(actor);
    }

    public void AddBehavior(ILayerBehavior behavior)
    {
        _behaviors.Add(behavior);
    }

    public void RemoveBehavior(ILayerBehavior behavior)
    {
        _behaviors.Remove(behavior);
    }

    public void Update(float deltaTime)
    {
        if (IsActive == false) return;

        foreach (var actor in _actors)
        {
            actor.Update(deltaTime);
        }

        for (int i = _behaviors.Count - 1; i >= 0; i--)
        {
            var behavior = _behaviors[i];
                
            if (behavior.IsActive == false) continue;
                
            behavior.Update(deltaTime);
                
            if (behavior.IsComplete && behavior.Duration.HasValue)
            {
                _behaviors.RemoveAt(i);
            }
        }
    }

    public void CollectRenderCommands(CameraDirector cameraDirector)
    {
        if (IsVisible == false) return;

        List<AbstractCameraBase> targetCameras = new List<AbstractCameraBase>();
            
        for (int i = 0; i < cameraDirector.Cameras.Count; i++)
        {
            var camera = cameraDirector.Cameras[i];
            uint cameraBit = (uint)(1 << i);
                
            if ((CameraMask & cameraBit) != 0)
            {
                targetCameras.Add(camera);
            }
        }

        foreach (var camera in targetCameras)
        {
            List<RenderCommand> commands = new List<RenderCommand>();

            foreach (var actor in _actors)
            {
                if (actor.IsActive == false) continue;
                if (actor.IsVisibleAtCamera(camera) == false) continue;
                    
                actor.CollectRenderCommands(camera, commands);
            }

            List<ILayerBehavior> activeBehaviors = new List<ILayerBehavior>();
                
            foreach (var behavior in _behaviors)
            {
                if (behavior.IsActive && behavior.IsComplete == false)
                {
                    activeBehaviors.Add(behavior);
                }
            }
                
            activeBehaviors.Sort((a, b) => a.Priority.CompareTo(b.Priority));

            foreach (var behavior in activeBehaviors)
            {
                behavior.Apply(commands, camera);
            }

            foreach (var command in commands)
            {
                cameraDirector.Submit(camera, command);
            }
        }
    }
}
