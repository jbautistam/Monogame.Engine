using Bau.BauEngine.Scenes.Rendering.Postprocessing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bau.BauEngine.Scenes.Rendering.New;

public class RenderManager
{
    private GraphicsDevice _gd;
    private SpriteBatch _sb;
    private RenderTarget2D _worldTarget;
    private RenderTarget2D _tempA, _tempB;
    private RenderTarget2D _finalOutput;
    private List<IPostProcessingEffect> _postEffects;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public SpriteBatch SpriteBatch => _sb;

    public RenderManager(GraphicsDevice gd, int width, int height)
    {
        _gd = gd;
        _sb = new SpriteBatch(gd);
        Width = width; 
        Height = height;
        _worldTarget = new RenderTarget2D(gd, width, height);
        _tempA = new RenderTarget2D(gd, width, height);
        _tempB = new RenderTarget2D(gd, width, height);
        _postEffects = new List<IPostProcessingEffect>();
    }

    public void BeginDrawWorld()
    {
        _gd.SetRenderTarget(_worldTarget);
        _gd.Clear(Color.Transparent);
        _sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
    }

    public void EndDrawWorld() => _sb.End();

    // Dibujar la UI siempre encima
    public void DrawUI(Action<SpriteBatch> uiDrawAction)
    {
        _sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
        uiDrawAction(_sb);
        _sb.End();
    }

    public void SetPostProcessingEffects(params IPostProcessingEffect[] effects)
    {
        _postEffects.Clear();
        _postEffects.AddRange(effects);
    }

    public void RenderScene()
    {
        if (_postEffects.Count == 0)
        {
            _finalOutput = _worldTarget;
            return;
        }

        RenderTarget2D source = _worldTarget;
        RenderTarget2D destination = _tempA;

        for (int i = 0; i < _postEffects.Count; i++)
        {
            _gd.SetRenderTarget(destination);
            _gd.Clear(Color.Transparent);
            _sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _postEffects[i].Apply(source, _sb, _gd);
            _sb.End();

            var swap = source;
            source = destination;
            destination = swap;
        }
        _finalOutput = source;
    }

    public Texture2D GetFinalTexture() => _finalOutput;

    /// <summary>
    ///     Actualiza el tamaño
    /// </summary>
    public void UpdateSize(int width, int height)
    {
        if (width != Width || height != Height)
        {
            // Cambia los parámetros
            Width = width; 
            Height = height;
            // Libera la memoria
            _worldTarget?.Dispose();
            _tempA?.Dispose();
            _tempB?.Dispose();
            // Crea los nuevos objetos de presentación
            _worldTarget = new RenderTarget2D(_gd, width, height);
            _tempA = new RenderTarget2D(_gd, width, height);
            _tempB = new RenderTarget2D(_gd, width, height);
        }
    }

    /// <summary>
    ///     Libera la memoria
    /// </summary>
    public void Dispose()
    {
        _worldTarget?.Dispose();
        _tempA?.Dispose();
        _tempB?.Dispose();
        _sb?.Dispose();
    }
}