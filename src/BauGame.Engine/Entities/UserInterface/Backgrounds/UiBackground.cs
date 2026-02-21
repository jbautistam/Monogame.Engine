using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Entities.UserInterface.Backgrounds;

/// <summary>
///		Componente de fondo
/// </summary>
public class UiBackground(Styles.UiStyle style) : UiAbstractBackground(style)
{
	/// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(Managers.GameContext gameContext)
	{
        if (!IsInitialized)
        {
            // Carga la textura
            if (!string.IsNullOrWhiteSpace(Texture))
                Texture2D = Style.Layer.Scene.LoadSceneAsset<Texture2D>(Texture);
            // Indica que ya está inicializado
           IsInitialized = true;
        }
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, Rectangle position, Managers.GameContext gameContext)
	{
		if (Texture2D is not null)
			camera.SpriteBatchController.Draw(Texture2D, position, Color * Opacity);
	}

	/// <summary>
	///		Textura
	/// </summary>
	public string? Texture { get; set; }

	/// <summary>
	///		Textura
	/// </summary>
	private Texture2D? Texture2D { get; set; }
}