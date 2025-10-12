using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.Tilemaps;

public class AnimatedTile
{
    public int[] FrameIndices { get; private set; }
    public float FrameDuration { get; private set; }
    public bool Loop { get; private set; } = true;

    private int _currentFrame;
    private float _timer;

    public AnimatedTile(int[] frameIndices, float frameDuration, bool loop = true)
    {
        FrameIndices = frameIndices;
        FrameDuration = frameDuration;
        Loop = loop;
        _currentFrame = 0;
        _timer = 0f;
    }

    public int GetCurrentFrameIndex(GameTime gameTime)
    {
        if (FrameIndices.Length <= 1) return FrameIndices[0];

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= FrameDuration)
        {
            _currentFrame++;
            if (_currentFrame >= FrameIndices.Length)
            {
                if (Loop)
                    _currentFrame = 0;
                else
                    _currentFrame = FrameIndices.Length - 1;
            }
            _timer = 0f;
        }

        return FrameIndices[_currentFrame];
    }

    public void Reset()
    {
        _currentFrame = 0;
        _timer = 0f;
    }
}
