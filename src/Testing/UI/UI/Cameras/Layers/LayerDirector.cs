using GameEngine.Cameras;

namespace GameEngine.Layers;

public class LayerDirector
{
    private readonly List<Layer> _layers = new List<Layer>();

    public IReadOnlyList<Layer> Layers => _layers;

    public Layer CreateLayer(string name, int drawOrder)
    {
        var layer = new Layer(name, drawOrder);
        _layers.Add(layer);
        return layer;
    }

    public void RemoveLayer(Layer layer)
    {
        _layers.Remove(layer);
    }

    public void SortLayersByDrawOrder()
    {
        _layers.Sort((a, b) => a.DrawOrder.CompareTo(b.DrawOrder));
    }

    public void Update(float deltaTime)
    {
        foreach (var layer in _layers)
        {
            layer.Update(deltaTime);
        }
    }

    public void CollectRenderCommands(CameraDirector cameraDirector)
    {
        SortLayersByDrawOrder();
            
        foreach (var layer in _layers)
        {
            layer.CollectRenderCommands(cameraDirector);
        }
    }
}