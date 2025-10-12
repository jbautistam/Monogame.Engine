using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Bau.Monogame.Engine.Domain.UI;

public interface ISelectable
{
    bool IsSelected { get; set; }
    event Action<ISelectable> OnSelected;
}

public class ListBox : Panel
{
    public List<UIElement> Items { get; } = new List<UIElement>();
    public Scrollbar Scrollbar { get; private set; }
    public float Spacing { get; set; } = 2f;
    public bool IsScrollable { get; private set; } = false;
    public bool AllowSelection { get; set; } = true;
    public UIElement SelectedItem { get; private set; }

    private float _contentHeight = 0f;
    private float _scrollOffset = 0f;

    public ListBox()
    {
        Scrollbar = new Scrollbar
        {
            Direction = Scrollbar.Orientation.Vertical,
            Size = new Vector2(20, Size.Y),
            Position = new Vector2(Size.X - 20, 0),
            ThumbSizeRatio = 0.5f,
            TrackColor = new Color(30, 30, 50, 200),
            Visible = false
        };
        Scrollbar.OnValueChanged += OnScrollbarValueChanged;
        AddChild(Scrollbar);
    }

    public void AddItem(UIElement item)
    {
        Items.Add(item);
        UpdateLayout();
    }

    public void ClearItems()
    {
        Items.Clear();
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            if (Children[i] != Scrollbar)
                RemoveChild(Children[i]);
        }
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        _contentHeight = 0f;
        foreach (var item in Items) _contentHeight += item.Size.Y + Spacing;
        if (Items.Count > 0) _contentHeight -= Spacing;

        IsScrollable = _contentHeight > Size.Y;
        Scrollbar.Visible = IsScrollable;

        if (IsScrollable)
        {
            Scrollbar.ThumbSizeRatio = MathHelper.Clamp(Size.Y / _contentHeight, 0.1f, 1f);
            Scrollbar.Size = new Vector2(20, Size.Y);
            Scrollbar.Position = new Vector2(Size.X - 20, 0);
        }

        float y = 0f;
        foreach (var item in Items)
        {
            item.Position = new Vector2(0, y - _scrollOffset);
            if (!Children.Contains(item)) AddChild(item);
            y += item.Size.Y + Spacing;
        }
    }

    private void OnScrollbarValueChanged(float value)
    {
        _scrollOffset = value * (_contentHeight - Size.Y);
        UpdateLayout();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Bounds.Contains((int)InputManager.Instance.MousePosition.X, (int)InputManager.Instance.MousePosition.Y))
        {
            int scrollDelta = InputManager.Instance.MouseScrollDelta;
            if (scrollDelta != 0 && IsScrollable)
            {
                float scrollSpeed = 20f;
                _scrollOffset = MathHelper.Clamp(_scrollOffset - scrollDelta * scrollSpeed, 0, _contentHeight - Size.Y);
                Scrollbar.SetValue(_contentHeight > Size.Y ? _scrollOffset / (_contentHeight - Size.Y) : 0);
            }
        }

        if (AllowSelection)
        {
            foreach (var item in Items)
            {
                if (item.IsHovered && InputManager.Instance.JustMouseLeftPressed)
                {
                    if (item is ISelectable selectable)
                    {
                        foreach (var other in Items) if (other is ISelectable otherSelectable && otherSelectable != selectable) otherSelectable.IsSelected = false;
                        selectable.IsSelected = true;
                        SelectedItem = item;
                        OnItemSelected?.Invoke(item);
                    }
                    else
                    {
                        SelectedItem = item;
                        OnItemSelected?.Invoke(item);
                    }
                }
            }
        }
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        base.DrawUI(spriteBatch);
        foreach (var child in Children)
        {
            if (child != Scrollbar && child.Bounds.Intersects(Bounds)) child.Draw(spriteBatch);
        }
        if (IsScrollable) Scrollbar.Draw(spriteBatch);
    }

    public event Action<UIElement> OnItemSelected;
}
