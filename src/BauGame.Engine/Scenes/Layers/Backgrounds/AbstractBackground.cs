using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///		Base de los fondos
/// </summary>
public abstract class AbstractBackground(string texture, int sortOrder)
{
	/// <summary>
	///		Inicia el fondo
	/// </summary>
	public void Start()
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
	///		Actualiza la capa (los actores se actualizan por separado)
	/// </summary>
	public abstract void UpdateLayer(GameTime gameTime);

	/// <summary>
	///		Dibuja la capa
	/// </summary>
	public abstract void DrawLayer(Cameras.Camera2D camera, GameTime gameTime);

	/// <summary>
	///		Finaliza la capa
	/// </summary>
	public abstract void End();
	
	/// <summary>
    ///     Nombre de la textura
    /// </summary>
    public string Texture { get; } = texture;

	/// <summary>
	///		Orden de dibujo de la capa
	/// </summary>
	public int SortOrder { get; set; } = sortOrder;

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