using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.CamerasNew.Behaviors;

public class CinematicPathBehavior(AbstractCameraBase camera) : AbstractCameraBehavior(camera, 0)
{
    public Vector2[] ControlPoints { get; set; }
    public float[] WaitTimes { get; set; }
    public bool Loop { get; set; } = false;
        
    private int _currentSegment;
    private float _segmentProgress;
    private float _waitTimer;

    protected override void UpdateSelf(Managers.GameContext gameContext)
    {
        if (ControlPoints == null || ControlPoints.Length < 2) return;
            
        if (_waitTimer > 0)
        {
            _waitTimer -= gameContext.DeltaTime;
            return;
        }
            
        int segments = ControlPoints.Length - 1;
        float segmentDuration = Duration / segments;
            
        _segmentProgress += gameContext.DeltaTime / segmentDuration;
            
        while (_segmentProgress >= 1f && _currentSegment < segments)
        {
            _segmentProgress -= 1f;
            _currentSegment++;
                
            if (WaitTimes != null && _currentSegment < WaitTimes.Length)
            {
                _waitTimer = WaitTimes[_currentSegment];
            }
        }
            
        if (_currentSegment >= segments)
        {
            if (Loop)
            {
                _currentSegment = 0;
                _segmentProgress = 0f;
            }
            else
            {
                IsComplete = true;
                return;
            }
        }
            
        Vector2 p0 = ControlPoints[Math.Max(0, _currentSegment - 1)];
        Vector2 p1 = ControlPoints[_currentSegment];
        Vector2 p2 = ControlPoints[Math.Min(ControlPoints.Length - 1, _currentSegment + 1)];
        Vector2 p3 = ControlPoints[Math.Min(ControlPoints.Length - 1, _currentSegment + 2)];
            
        Vector2 newPos = CatmullRom(p0, p1, p2, p3, _segmentProgress);
        Camera.SetPosition(newPos);
    }

    private Vector2 CatmullRom(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
            
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }
}
