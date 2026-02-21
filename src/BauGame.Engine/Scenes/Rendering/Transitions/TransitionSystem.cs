using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering.Transitions;

public class TransitionSystem
{
	public enum TransitionType
    {
        Fade,
        WipeLeft,
        WipeRight,
        WipeUp,
        WipeDown,
        Circle,
        Pixelate
    }

    private readonly GraphicsDevice _device;
    private readonly SpriteBatch _spriteBatch;
        
    public bool IsTransitioning { get; private set; }
    public float Progress { get; private set; }
    public TransitionType CurrentType { get; private set; }
        
    private RenderTarget2D _fromTarget;
    private RenderTarget2D _toTarget;
    private float _duration;
    private Action _onComplete;

    public TransitionSystem(GraphicsDevice device, SpriteBatch spriteBatch)
    {
        _device = device;
        _spriteBatch = spriteBatch;
    }

    public void StartTransition(
        RenderTarget2D from, 
        RenderTarget2D to, 
        TransitionType type, 
        float duration,
        Action onComplete = null)
    {
        _fromTarget = from;
        _toTarget = to;
        CurrentType = type;
        _duration = duration;
        _onComplete = onComplete;
        Progress = 0f;
        IsTransitioning = true;
    }

    public void Update(GameTime gameTime)
    {
        if (IsTransitioning == false) return;

        Progress += (float)gameTime.ElapsedGameTime.TotalSeconds / _duration;
            
        if (Progress >= 1f)
        {
            Progress = 1f;
            IsTransitioning = false;
            _onComplete?.Invoke();
        }
    }

    public void Draw()
    {
        if (IsTransitioning == false) return;

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);

        switch (CurrentType)
        {
            case TransitionType.Fade:
                DrawFade();
                break;
            case TransitionType.WipeLeft:
                DrawWipe(new Vector2(-1, 0));
                break;
            case TransitionType.WipeRight:
                DrawWipe(new Vector2(1, 0));
                break;
            case TransitionType.WipeUp:
                DrawWipe(new Vector2(0, -1));
                break;
            case TransitionType.WipeDown:
                DrawWipe(new Vector2(0, 1));
                break;
            case TransitionType.Circle:
                DrawCircle();
                break;
            case TransitionType.Pixelate:
                DrawPixelate();
                break;
        }

        _spriteBatch.End();
    }

    private void DrawFade()
    {
        float fromAlpha = 1f - Progress;
        float toAlpha = Progress;

        _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White * fromAlpha);
        _spriteBatch.Draw(_toTarget, Vector2.Zero, Color.White * toAlpha);
    }

    private void DrawWipe(Vector2 direction)
    {
        Vector2 offset = direction * Progress * new Vector2(_fromTarget.Width, _fromTarget.Height);
            
        _spriteBatch.Draw(_fromTarget, offset, Color.White);
        _spriteBatch.Draw(_toTarget, offset - direction * new Vector2(_fromTarget.Width, _fromTarget.Height), Color.White);
    }

    private void DrawCircle()
    {
        _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
    }

    private void DrawPixelate()
    {
        _spriteBatch.Draw(_fromTarget, Vector2.Zero, Color.White);
    }
}
