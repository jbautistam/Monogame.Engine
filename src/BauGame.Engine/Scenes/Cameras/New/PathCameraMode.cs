// PathCameraMode.cs
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class PathCameraMode : ICameraMode
{
    private List<Vector2> _path = new List<Vector2>();
    private float _duration;
    private float _timer;
    private Vector2 _startPos;
    private bool _isActive;

    public void SetPath(List<Vector2> path, float duration)
    {
        _path = new List<Vector2>(path);
        _duration = duration;
        _timer = 0f;
        _isActive = true;
    }

    public void Update(GameTime gameTime, Camera2D camera)
    {
        if (!_isActive || _path.Count == 0) return;

        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float t = MathHelper.Clamp(_timer / _duration, 0f, 1f);

        if (_path.Count == 1)
        {
            camera._desiredPosition = _path[0];
        }
        else
        {
            // Interpolación lineal simple entre puntos (puedes usar Catmull-Rom para suavidad)
            float segmentLength = 1f / (_path.Count - 1);
            int index = (int)(t / segmentLength);
            index = MathHelper.Clamp(index, 0, _path.Count - 2);

            float localT = (t - index * segmentLength) / segmentLength;
            camera._desiredPosition = Vector2.Lerp(_path[index], _path[index + 1], localT);
        }

        if (t >= 1f)
            _isActive = false;
    }

    public void Reset() => _isActive = false;
}
