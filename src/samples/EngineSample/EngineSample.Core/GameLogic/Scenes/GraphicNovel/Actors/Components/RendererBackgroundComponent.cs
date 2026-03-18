using Microsoft.Xna.Framework;
using Bau.BauEngine.Managers;
using Bau.BauEngine.Actors.Components.Renderers;
using Bau.BauEngine.Entities.Common;

namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors.Components;

/// <summary>
///		Componente para representación de un fondo
/// </summary>
public class RendererBackgroundComponent(BackgroundActor actor) : AbstractRendererComponent(actor), Bau.BauEngine.Actors.Interfaces.IActorDrawable
{
	/// <summary>
	///		Inicia el componente
	/// </summary>
	protected override void StartSelf()
	{
	}

	/// <summary>
	///		Arranca la carga de los datos de la definición
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
	}

	/// <summary>
	///		Dibuja el actor
	/// </summary>
    protected override void DrawSelf(Bau.BauEngine.Scenes.Rendering.RenderingManager renderingManager, GameContext gameContext)
    {
        if (Actor is BackgroundActor background && Sprite is not null)
        {
            Size size = Sprite.GetSize();

                // Dibuja el fondo
                if (size.Width > 0 && size.Height > 0)
                {
                    float worldScreenWidth = renderingManager.Scene.Camera.ScreenViewport.Width / renderingManager.Scene.Camera.Zoom;
                    float worldScreenHeight = renderingManager.Scene.Camera.ScreenViewport.Height / renderingManager.Scene.Camera.Zoom;
                    Vector2 screenScale = new(worldScreenWidth / size.Width, worldScreenHeight / size.Height);
                    Vector2 backgroundPosition = Actor.Transform.Bounds.Location + renderingManager.Scene.Camera.Position - new Vector2(0.5f * worldScreenWidth, 0.5f * worldScreenHeight);

                        // Dibujamos el fondo escalado para cubrir toda la pantalla visible
                        //if (Actor.Transform.Rotation != 0)
                        //    renderingManager.SpriteRenderer.Draw(Sprite, backgroundPosition, Actor.Transform.Center, Scale * scale, Actor.Transform.Rotation, Color * Opacity);
                        //else
						renderingManager.SpriteRenderer.Draw(Sprite, backgroundPosition, Vector2.Zero, Scale * screenScale, Actor.Transform.Rotation, Color * Opacity);
                }
        }
    }

	/// <summary>
	///		Detiene el componente
	/// </summary>
	protected override void EndSelf()
	{
		// ... no hace nada, sólo implementa la interface
	}
}