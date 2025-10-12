using Microsoft.Xna.Framework;

namespace Bau.Monogame.Engine.Domain.UI;

public class ClickableFrame : Frame
{
    public enum State
    {
        Normal,
        Hover,
        Pressed
    }

    public State CurrentState { get; private set; } = State.Normal;
    public Color NormalColor { get; set; } = new Color(70, 70, 70);
    public Color HoverColor { get; set; } = new Color(100, 100, 100);
    public Color PressedColor { get; set; } = new Color(50, 50, 50);

    public bool EnableScaleAnimation { get; set; } = true;
    public Vector2 NormalScale { get; set; } = Vector2.One;
    public Vector2 HoverScale { get; set; } = new Vector2(1.05f);
    public Vector2 PressedScale { get; set; } = new Vector2(0.95f);

    public bool EnableRotationAnimation { get; set; } = false;
    public float NormalRotation { get; set; } = 0f;
    public float HoverRotation { get; set; } = 0f;
    public float PressedRotation { get; set; } = 0f;

    public bool EnableShadow { get; set; } = true;
    public Vector2 NormalShadowOffset { get; set; } = Vector2.Zero;
    public Vector2 HoverShadowOffset { get; set; } = new Vector2(2, 2);
    public Vector2 PressedShadowOffset { get; set; } = new Vector2(0, 0);
    public Color ShadowColor { get; set; } = new Color(0, 0, 0, 100);

    private Vector2 _currentScale;
    private Vector2 _targetScale;
    private float _currentRotation;
    private float _targetRotation;
    private Vector2 _currentShadowOffset;
    private Vector2 _targetShadowOffset;

    private float _animationDuration = 0.15f;
    private float _timer;
    private bool _isAnimating;

    public ClickableFrame()
    {
        _currentScale = NormalScale;
        _targetScale = NormalScale;
        _currentRotation = NormalRotation;
        _targetRotation = NormalRotation;
        _currentShadowOffset = NormalShadowOffset;
        _targetShadowOffset = NormalShadowOffset;
        LayerDepth = 0.8f;
    }

    protected override void UpdateUI(GameTime gameTime)
    {
        base.UpdateUI(gameTime);

        if (_isAnimating)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            float t = MathHelper.Clamp(_timer / _animationDuration, 0, 1);

            if (EnableScaleAnimation) _currentScale = Vector2.Lerp(_currentScale, _targetScale, t);
            if (EnableRotationAnimation) _currentRotation = MathHelper.Lerp(_currentRotation, _targetRotation, t);
            if (EnableShadow) _currentShadowOffset = Vector2.Lerp(_currentShadowOffset, _targetShadowOffset, t);

            if (t >= 1f) _isAnimating = false;
        }

        var prevState = CurrentState;
        UpdateState();

        if (CurrentState != prevState)
        {
            _timer = 0f;
            _isAnimating = true;
            if (EnableScaleAnimation) _currentScale = _targetScale;
            if (EnableRotationAnimation) _currentRotation = _targetRotation;
            if (EnableShadow) _currentShadowOffset = _targetShadowOffset;
            SetTargetState(CurrentState);
        }
    }

    private void UpdateState()
    {
        if (!Enabled) CurrentState = State.Normal;
        else if (IsHovered && InputManager.Instance.IsMouseLeftPressed) CurrentState = State.Pressed;
        else if (IsHovered) CurrentState = State.Hover;
        else CurrentState = State.Normal;
    }

    private void SetTargetState(State state)
    {
        switch (state)
        {
            case State.Hover:
                BackgroundColor = HoverColor;
                _targetScale = HoverScale;
                _targetRotation = HoverRotation;
                _targetShadowOffset = HoverShadowOffset;
                break;
            case State.Pressed:
                BackgroundColor = PressedColor;
                _targetScale = PressedScale;
                _targetRotation = PressedRotation;
                _targetShadowOffset = PressedShadowOffset;
                break;
            default:
                BackgroundColor = NormalColor;
                _targetScale = NormalScale;
                _targetRotation = NormalRotation;
                _targetShadowOffset = NormalShadowOffset;
                break;
        }
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;

        Vector2 drawPosition = AbsolutePosition;
        if (EnableShadow && _currentShadowOffset != Vector2.Zero)
        {
            var shadowBounds = new Rectangle((int)(drawPosition.X + _currentShadowOffset.X), (int)(drawPosition.Y + _currentShadowOffset.Y), (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(TextureHelper.Pixel, shadowBounds, ShadowColor);
        }

        Vector2 origin = new Vector2(Size.X / 2f, Size.Y / 2f);
        Vector2 scaledPosition = drawPosition + origin - origin * _currentScale;

        spriteBatch.Draw(TextureHelper.Pixel, scaledPosition, Bounds, BackgroundColor, MathHelper.ToRadians(_currentRotation), origin, _currentScale, SpriteEffects.None, 0f);

        if (DrawBorder && BorderThickness > 0)
        {
            var rect = new Rectangle((int)drawPosition.X, (int)drawPosition.Y, (int)Size.X, (int)Size.Y);
            int t = BorderThickness;
            spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), BorderColor);
            spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), BorderColor);
            spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), BorderColor);
            spriteBatch.Draw(TextureHelper.Pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), BorderColor);
        }
    }

    public event Action OnClick;
    public event Action<State> OnStateChanged;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (IsHovered && InputManager.Instance.JustMouseLeftPressed) OnClick?.Invoke();
        if (CurrentState != State.Normal) OnStateChanged?.Invoke(CurrentState);
    }
}
