using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.UserInterface;

/// <summary>
///		Componente de fondo
/// </summary>
public class UiBackground(UserInterfaceLayer layer, UiPosition positions) : UiElement(layer, positions)
{
	// Variables privadas
	private bool _isInitialized;

	/// <summary>
	///		Calcula los límites del control
	/// </summary>
	protected override void ComputeScreenComponentBounds() {}

	/// <summary>
	///		Actualiza el control
	/// </summary>
	public override void Update(GameTime gameTime)
	{
        if (!_isInitialized)
        {
            // Carga la textura
            if (!string.IsNullOrWhiteSpace(Texture))
                Texture2D = Layer.Scene.LoadSceneAsset<Texture2D>(Texture);
            // Indica que ya está inicializado
            _isInitialized = true;
        }
	}

	/// <summary>
	///		Dibuja el control
	/// </summary>
	public override void Draw(Camera2D camera, GameTime gameTime)
	{
		if (Texture2D is not null)
			camera.SpriteBatchController.Draw(Texture2D, Position.ScreenPaddedBounds, Color * Opacity);
	}

	/// <summary>
	///		Textura
	/// </summary>
	public string? Texture { get; set; }

	/// <summary>
	///		Textura
	/// </summary>
	private Texture2D? Texture2D { get; set; }

	/// <summary>
	///		Color del fondo
	/// </summary>
	public Color Color { get; set; } = Color.White;
}