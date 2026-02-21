using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Managers.Resources.Textures;
using Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds.Effects;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///     Fondo dinámico
/// </summary>
public class DynamicBackgroundLayer(AbstractScene scene, string name, string texture, int sortOrder) : AbstractBackgroundLayer(scene, name, texture, sortOrder)
{
    /// <summary>
    ///     Modo de escalado
    /// </summary>
    public enum BackgroundAutoScaleMode
    {
        /// <summary>Escala la textura para que llene completamente el viewport (puede recortar los bordes)</summary>
        Fit,
        /// <summary>Escala la textura para que se muestre completa dentro del viewport (puede dejar bandas)</summary>
        Fill
    }
    // Variables privadas
    private TextureRegion? _region;
    private float _referenceZoom = -1;

    /// <summary>
    ///     Actualiza la capa para físicas
    /// </summary>
	protected override void UpdatePhysicsLayer(Managers.GameContext gameContext)
	{
	}

    /// <summary>
    ///     Actualiza la capa de fondo
    /// </summary>
	protected override void UpdateLayer(Managers.GameContext gameContext)
	{
        // Carga la región de la textura
        if (_region is null)
            _region = GetTextureRegion("background");
        // Ejecuta los efectos
        if (_region is not null)
            Effects.Update(gameContext);
	}

    /// <summary>
    ///     Dibuja la capa
    /// </summary>
    protected override void DrawLayer(Cameras.Camera2D camera, Managers.GameContext gameContext)
    {
        if (_region is not null)
        {
            BackgroundState state = new();

                // Calcula el zoom de referencia
                ComputeReferenceZoom(camera);
                // Calcula el estado
                foreach (AbstractBackgroundEffect effect in Effects.Enumerate())
                    state = state.Combine(effect.State);
                // Dibuja el fondo
                camera.SpriteBatchController.Draw(_region.Texture, /* camera.ViewPortCenter + */ state.ScreenOffset, null, state.ViewCenter, 
                                                  state.Zoom * _referenceZoom, 
                                                  Microsoft.Xna.Framework.Graphics.SpriteEffects.None, state.Tint, 
                                                  state.Rotation);
        }
    }

    /// <summary>
    ///     Calcula el zoom de referencia
    /// </summary>
    private void ComputeReferenceZoom(Cameras.Camera2D camera)
    {
        if (_region is not null && !_region.Region.IsEmpty && _referenceZoom < 0)
        {
            float textureWidth = _region.Region.Width, textureHeight = _region.Region.Height;
            float zoomX = camera.ScreenViewport.Width / textureWidth, zoomY = camera.ScreenViewport.Height / textureHeight;

                // Calcula el zoom de refecencia
                if (AutoScaleMode == BackgroundAutoScaleMode.Fit)
                    _referenceZoom = Math.Max(zoomX, zoomY);
                else
                    _referenceZoom = Math.Min(zoomX, zoomY);
                // Centro de la vista
                BaseState.ViewCenter = new Vector2(0.5f * textureWidth, 0.5f * textureHeight);
        }
    }

    /// <summary>
    ///     Finaliza el dibujo del fondo
    /// </summary>
	protected override void EndLayer()
	{
	}

    /// <summary>
    ///     Estado del fondo
    /// </summary>
    public BackgroundState BaseState { get; set; } = new();

    /// <summary>
    ///     Modo de escalado
    /// </summary>
    public BackgroundAutoScaleMode AutoScaleMode { get; set; } = BackgroundAutoScaleMode.Fit;

    /// <summary>
    ///     Efectos
    /// </summary>
    public EffectsList Effects { get; } = new();
}