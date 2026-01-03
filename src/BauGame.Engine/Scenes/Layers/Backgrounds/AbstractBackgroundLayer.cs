using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///		Base para las capas de fondos
/// </summary>
public abstract class AbstractBackgroundLayer(AbstractScene scene, string name, string texture, int sortOrder) : AbstractLayer(scene, name, LayerType.Background, sortOrder)
{
	/// <summary>
	///		Inicia el fondo
	/// </summary>
	protected override void StartLayer()
	{
        Background = GameEngine.Instance.ResourcesManager.TextureManager.Assets.Get(Texture);
	}

	/// <summary>
	///		Obtiene la región de la textura
	/// </summary>
	protected Managers.Resources.Textures.TextureRegion? GetTextureRegion(string? name)
	{
		// Obtiene la región adecuada
		if (Background is not null)
		{
			// Normaliza la cadena
			if (string.IsNullOrWhiteSpace(name))
				name = string.Empty;
			// Devuelve la región adecuada
			return Background.GetRegion(name);
		}
		// Si ha llegado hasta aquí es porque no ha encontrado nada
		return null;
	}
	
	/// <summary>
    ///     Nombre de la textura
    /// </summary>
    public string Texture { get; } = texture;

    /// <summary>
	///		Textura del fondo
	/// </summary>
    protected Managers.Resources.Textures.AbstractTexture? Background { get; private set; }

	/// <summary>
	///		Indica si se debe rotar la textura con la cámara
	/// </summary>
    public bool RotateWithCamera { get; set; }

	/// <summary>
	///		Color del fondo
	/// </summary>
    public Color Color { get; set; } = Color.White;

	/// <summary>
	///		Indica si se debe mostrar el fondo
	/// </summary>
	public bool Visible { get; set; } = true;
}