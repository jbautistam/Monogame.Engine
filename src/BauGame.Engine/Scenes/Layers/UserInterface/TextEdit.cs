using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;

namespace Bau.Monogame.Engine.Domain.UI;

public class TextEdit : UIElement
{
    public string Text { get; private set; } = "";
    public string Placeholder { get; set; } = "";
    public int MaxLength { get; set; } = 100;
    public Color TextColor { get; set; } = Color.White;
    public Color PlaceholderColor { get; set; } = new Color(150, 150, 150);
    public Color CursorColor { get; set; } = Color.White;

    public Frame Frame { get; set; }
    public Label Label { get; private set; }

    private bool _isFocused = false;
    private int _cursorPosition = 0;
    private float _cursorTimer = 0f;
    private bool _cursorVisible = true;
    private float _cursorBlinkTime = 0.5f;

    public event Action<TextEdit> OnTextChanged;
    public event Action<TextEdit> OnEnterPressed;
    public event Action<TextEdit> OnFocusGained;
    public event Action<TextEdit> OnFocusLost;

    public TextEdit(SpriteFont font)
    {
        Frame = new Frame
        {
            BackgroundColor = new Color(40, 40, 60, 200),
            BorderColor = Color.Gray,
            BorderThickness = 2,
            Padding = new Vector2(10, 5)
        };

        Label = new Label(font, "") { TextColor = TextColor };
        AddChild(Label);

        InputManager.Instance.JustActionPressed += OnActionPressed;
    }

    private void OnActionPressed(string actionName)
    {
        if (!_isFocused) return;
        if (actionName == "Submit") OnEnterPressed?.Invoke(this);
        if (actionName == "Cancel") { _isFocused = false; OnFocusLost?.Invoke(this); }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (InputManager.Instance.JustMouseLeftPressed)
        {
            if (IsHovered) { _isFocused = true; OnFocusGained?.Invoke(this); }
            else if (_isFocused) { _isFocused = false; OnFocusLost?.Invoke(this); }
        }

        if (_isFocused)
        {
            _cursorTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_cursorTimer >= _cursorBlinkTime) { _cursorVisible = !_cursorVisible; _cursorTimer = 0f; }
            CaptureKeyboardInput();
        }
    }

    private void CaptureKeyboardInput()
    {
        var keyboard = Keyboard.GetState();
        var prevKeyboard = InputManager.Instance.PreviousState.KeyboardState;

        if (keyboard.IsKeyDown(Keys.Left) && prevKeyboard.IsKeyUp(Keys.Left)) _cursorPosition = Math.Max(0, _cursorPosition - 1);
        if (keyboard.IsKeyDown(Keys.Right) && prevKeyboard.IsKeyUp(Keys.Right)) _cursorPosition = Math.Min(Text.Length, _cursorPosition + 1);
        if (keyboard.IsKeyDown(Keys.Home) && prevKeyboard.IsKeyUp(Keys.Home)) _cursorPosition = 0;
        if (keyboard.IsKeyDown(Keys.End) && prevKeyboard.IsKeyUp(Keys.End)) _cursorPosition = Text.Length;

        if (keyboard.IsKeyDown(Keys.Back) && prevKeyboard.IsKeyUp(Keys.Back) && _cursorPosition > 0)
        {
            Text = Text.Remove(_cursorPosition - 1, 1);
            _cursorPosition--;
            OnTextChanged?.Invoke(this);
        }

        if (keyboard.IsKeyDown(Keys.Delete) && prevKeyboard.IsKeyUp(Keys.Delete) && _cursorPosition < Text.Length)
        {
            Text = Text.Remove(_cursorPosition, 1);
            OnTextChanged?.Invoke(this);
        }

        var pressedKeys = keyboard.GetPressedKeys();
        foreach (var key in pressedKeys)
        {
            if (prevKeyboard.IsKeyUp(key))
            {
                char? character = GetCharFromKey(key, keyboard);
                if (character.HasValue && Text.Length < MaxLength)
                {
                    var sb = new StringBuilder(Text);
                    sb.Insert(_cursorPosition, character.Value);
                    Text = sb.ToString();
                    _cursorPosition++;
                    OnTextChanged?.Invoke(this);
                }
            }
        }
    }

    private char? GetCharFromKey(Keys key, KeyboardState state)
    {
        if (key >= Keys.A && key <= Keys.Z)
        {
            char c = (char)('A' + (key - Keys.A));
            return state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift) ? c : char.ToLower(c);
        }

        if (key >= Keys.D0 && key <= Keys.D9) return (char)('0' + (key - Keys.D0));
        if (key >= Keys.NumPad0 && key <= Keys.NumPad9) return (char)('0' + (key - Keys.NumPad0));

        return key switch
        {
            Keys.Space => ' ',
            Keys.OemPeriod => '.',
            Keys.OemComma => ',',
            Keys.OemMinus => '-',
            Keys.OemPlus => '=',
            Keys.OemQuestion => '/',
            Keys.OemOpenBrackets => '[',
            Keys.OemCloseBrackets => ']',
            Keys.OemSemicolon => ';',
            Keys.OemQuotes => '\'',
            Keys.OemBackslash => '\\',
            Keys.OemTilde => '`',
            _ => null
        };
    }

    protected override void DrawUI(SpriteBatch spriteBatch)
    {
        Frame?.Draw(spriteBatch);

        string displayText = string.IsNullOrEmpty(Text) ? Placeholder : Text;
        Color displayColor = string.IsNullOrEmpty(Text) ? PlaceholderColor : TextColor;

        if (Label != null)
        {
            Label.Text = displayText;
            Label.TextColor = displayColor;
            Label.Draw(spriteBatch);
        }

        if (_isFocused && _cursorVisible)
        {
            var font = Label.Font;
            if (font != null)
            {
                string beforeCursor = Text.Substring(0, _cursorPosition);
                Vector2 cursorPos = Label.AbsolutePosition + new Vector2(font.MeasureString(beforeCursor).X, 0);
                spriteBatch.Draw(TextureHelper.Pixel, new Rectangle((int)cursorPos.X, (int)cursorPos.Y, 2, (int)font.LineSpacing), CursorColor);
            }
        }
    }

    public void SetText(string text)
    {
        Text = text ?? "";
        _cursorPosition = Text.Length;
        OnTextChanged?.Invoke(this);
    }

    public void Clear() => SetText("");

    public void Focus() { _isFocused = true; OnFocusGained?.Invoke(this); }
    public void Unfocus() { _isFocused = false; OnFocusLost?.Invoke(this); }
}
