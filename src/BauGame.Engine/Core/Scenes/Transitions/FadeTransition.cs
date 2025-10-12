using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.Monogame.Engine.Domain.Scenes.Transitions;

public class FadeTransition : SceneTransitionBase
{
    private Color _fadeColor = Color.Black;
    private float _alphaOut = 0f;
    private float _alphaIn = 0f;

    public override void Update(GameTime gameTime)
    {
        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        float progress = MathHelper.Clamp(_elapsedTime / Duration, 0, 1);
        progress = EaseInOutCubic(progress);

        if (progress <= 0.5f)
        {
            _alphaOut = MathHelper.Lerp(0, 1, progress * 2);
            _alphaIn = 0;
        }
        else
        {
            _alphaOut = 1;
            _alphaIn = MathHelper.Lerp(0, 1, (progress - 0.5f) * 2);
        }

        if (progress >= 1f) IsComplete = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_currentSceneTarget != null)
            spriteBatch.Draw(_currentSceneTarget, Vector2.Zero, Color.White * (1 - _alphaOut));

        if (_nextSceneTarget != null)
            spriteBatch.Draw(_nextSceneTarget, Vector2.Zero, Color.White * _alphaIn);

        if (_alphaOut > 0 || _alphaIn < 1)
            spriteBatch.Draw(_currentSceneTarget, Vector2.Zero, _fadeColor * MathHelper.Lerp(_alphaOut, _alphaIn, 0.5f));
    }

    private float EaseInOutCubic(float x) => x < 0.5f ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
}

