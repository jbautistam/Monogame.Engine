using GameEngine.Math;
using Microsoft.Xna.Framework;

namespace GameEngine.Cameras.Behaviors;

public class RecoilBehavior : AbstractCameraBehaviorBase
{
    public override int Priority => 80;
        
    public Vector2 Direction { get; set; } = Vector2.Zero;
    public float MaxRecoil { get; set; } = 20f;
    public float RecoverySpeed { get; set; } = 10f;
    public float Decay { get; set; } = 0.9f;
        
    private Vector2 _currentRecoil;
    private Vector2 _originalPosition;

    public RecoilBehavior(AbstractCameraBase camera) : base(camera)
    {
    }

    public void AddRecoil(float amount, Vector2 direction)
    {
        Vector2 newRecoil = direction * amount;
        _currentRecoil += newRecoil;
            
        float length = _currentRecoil.Length();
        if (length > MaxRecoil)
        {
            _currentRecoil = _currentRecoil / length * MaxRecoil;
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        if (ElapsedTime == 0f)
        {
            _originalPosition = Camera.State.Transform.Position;
        }
            
        _currentRecoil *= Decay;
            
        Vector2 targetPos = _originalPosition + _currentRecoil;
        Camera.SetPosition(targetPos);
            
        _originalPosition = MathUtils.Lerp(
            _originalPosition, 
            Camera.State.Transform.Position - _currentRecoil, 
            RecoverySpeed * deltaTime
        );
            
        if (_currentRecoil.LengthSquared() < 0.01f)
        {
            IsComplete = true;
        }
    }

    protected override void OnComplete()
    {
        Camera.SetPosition(_originalPosition);
    }

    public override void Reset()
    {
        base.Reset();
        _currentRecoil = Vector2.Zero;
    }
}
