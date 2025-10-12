using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bau.Monogame.Engine.Domain.UI;

[Flags]
public enum Anchor
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Top = 1 << 2,
    Bottom = 1 << 3,
    CenterX = 1 << 4,
    CenterY = 1 << 5,
    TopLeft = Left | Top,
    TopRight = Right | Top,
    BottomLeft = Left | Bottom,
    BottomRight = Right | Bottom,
    Center = CenterX | CenterY,
    Stretch = Left | Right | Top | Bottom
}

public abstract class UIElement : Actor
{
    public Anchor Anchor { get; set; } = Anchor.TopLeft;
    public Vector2 Margins { get; set; } = Vector2.Zero;
    public bool IsHovered { get; protected set; }

    private Vector2? _cachedScreenSize;
    private Rectangle? _cachedBounds;

    public override Vector2 AbsolutePosition
    {
        get
        {
            var screenSize = UIManager.ScreenSize;
            if (_cachedScreenSize.HasValue && _cachedScreenSize.Value == screenSize && _cachedBounds.HasValue)
                return new Vector2(_cachedBounds.Value.X, _cachedBounds.Value.Y);

            Vector2 pos = Position;
            if (Anchor != Anchor.None)
            {
                float x = 0, y = 0;
                if (Anchor.HasFlag(Anchor.Left) && Anchor.HasFlag(Anchor.Right))
                {
                    x = Margins.X;
                    Size = new Vector2(screenSize.X - Margins.X - Margins.Y, Size.Y);
                }
                else if (Anchor.HasFlag(Anchor.Left)) x = Margins.X;
                else if (Anchor.HasFlag(Anchor.Right)) x = screenSize.X - Size.X - Margins.X;
                else if (Anchor.HasFlag(Anchor.CenterX)) x = (screenSize.X - Size.X) / 2 + Margins.X;

                if (Anchor.HasFlag(Anchor.Top) && Anchor.HasFlag(Anchor.Bottom))
                {
                    y = Margins.Y;
                    Size = new Vector2(Size.X, screenSize.Y - Margins.X - Margins.Y);
                }
                else if (Anchor.HasFlag(Anchor.Top)) y = Margins.Y;
                else if (Anchor.HasFlag(Anchor.Bottom)) y = screenSize.Y - Size.Y - Margins.Y;
                else if (Anchor.HasFlag(Anchor.CenterY)) y = (screenSize.Y - Size.Y) / 2 + Margins.Y;

                pos = new Vector2(x, y);
            }

            if (Parent != null) pos += Parent.AbsolutePosition;
            _cachedScreenSize = screenSize;
            _cachedBounds = new Rectangle((int)pos.X, (int)pos.Y, (int)Size.X, (int)Size.Y);
            return pos;
        }
    }

    public override Rectangle Bounds => new Rectangle((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);

    public void InvalidateLayout()
    {
        _cachedScreenSize = null;
        _cachedBounds = null;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        var mousePos = InputManager.Instance.MousePosition;
        var mouseRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
        IsHovered = Bounds.Contains(mouseRect);
    }

    public abstract void DrawUI(SpriteBatch spriteBatch);
    public override void Draw(SpriteBatch spriteBatch) => DrawUI(spriteBatch);
}
