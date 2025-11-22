using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.TilesMap;

public class AnimatedTile
{
    private float _timer;
    private int _currentFrameIndex;

    public AnimatedTile(List<int> frames, float frameDuration)
    {
        Frames = frames;
        FrameDuration = frameDuration;
    }

    public int GetCurrentFrame(GameTime gameTime)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timer >= FrameDuration)
        {
            _timer = 0;
            _currentFrameIndex = (_currentFrameIndex + 1) % Frames.Count;
        }
        return Frames[_currentFrameIndex];
    }
    public List<int> Frames { get; }
    public float FrameDuration { get; }
}
