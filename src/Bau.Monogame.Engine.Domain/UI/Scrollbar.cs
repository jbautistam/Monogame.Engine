using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bau.Monogame.Engine.Domain.UI;

public class Scrollbar : UIElement
{
    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    public Orientation Direction { get; set; } = Orientation.Vertical;
    public float Value { get; private set; } = 0f;
    public float ThumbSizeRatio { get; set; } = 0.3f;
    public Color TrackColor { get; set; } = new Color(50, 50, 50, 200);
    public Vector2 Padding { get; set; } = new Vector2(2);

    public event Action<float> OnValueChanged;

    private ClickableFrame _thumb;
    private bool _isDragging = false;
    private Vector2 _dragStartMousePos;
    private float _dragStartValue;

    public Scrollbar()
    {
        _thumb = new ClickableFrame
        {
            BackgroundColor = new Color(150, 150, 150),
            HoverColor = new Color(180, 180, 180),
            PressedColor = new Color(120, 120, 120),
            BorderColor = Color.Gray,
            BorderThickness = 1,
            EnableScaleAnimation = false,
            EnableRotationAnimation = false,
            EnableShadow = false
        };
        _thumb.OnClick += () => { };
        _thumb.Parent = this;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateThumbPosition();

        var mousePos = InputManager.Instance.MousePosition;
        var mouseRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);

        if (_isDragging)
        {
            if (InputManager.Instance.IsMouseLeftPressed)
            {
                float delta = Direction == Orientation.Horizontal
                    ? (mousePos.X - _dragStartMousePos.X) / (Bounds.Width - _thumb.Bounds.Width)
                    : (mousePos.Y - _dragStartMousePos.Y) / (Bounds.Height - _thumb.Bounds.Height);
                Value = MathHelper.Clamp(_dragStartValue + delta, 0f, 1f);
                OnValueChanged?.Invoke(Value);
            }
            else
            {
                _isDragging = false;
            }
        }
        else
        {
            if (_thumb.IsHovered && InputManager.Instance.JustMouseLeftPressed)
            {
                _isDragging = true;
                _dragStartMousePos = mousePos;
                _dragStartValue = Value;
            }
        }

        _thumb.Update(gameTime);
    }

    private void UpdateThumbPosition()
    {
        float trackLength = Direction == Orientation.Horizontal ? Bounds.Width : Bounds.Height;
        float thumbLength = trackLength * ThumbSizeRatio;
        float availableLength = trackLength - thumbLength;
        float thumbOffset = availableLength * Value;

        if (Direction == Orientation.Horizontal)
        {
            _thumb.Position = new Vector2(Padding.X + thumbOffset, Padding.Y);
            _thumb.Size = new Vector2(thumbLength, Bounds.Height - Padding.Y * 2);
        }
        else
        {
            _thumb.Position = new Vector2(Padding.X, Padding.Y + thumbOffset);
            _thumb.Size = new Vector2(Bounds.Width - Padding.X * 2, thumbLength);
        }
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(TextureHelper.Pixel, Bounds, TrackColor);
        _thumb.Draw(spriteBatch);
    }

    public void SetValue(float value)
    {
        Value = MathHelper.Clamp(value, 0f, 1f);
        OnValueChanged?.Invoke(Value);
    }

    public void SetSize(Vector2 size)
    {
        Size = size;
        UpdateThumbPosition();
    }
}
