using Microsoft.Xna.Framework;
using Bau.Libraries.BauGame.Engine.Managers;

namespace Bau.Libraries.BauGame.Engine.Scenes.Layers.Backgrounds;

/// <summary>
///		Base para las capas de fondos
/// </summary>
public abstract class AbstractBackgroundLayer : AbstractLayer
{
	public AbstractBackgroundLayer(AbstractScene scene, string name, string asset, string? region, int sortOrder) : base(scene, name, LayerType.Background, sortOrder)
	{
		Sprite = new Entities.Common.Sprites.SpriteDefinition(asset, region);
	}

	/// <summary>
	///		Inicia el fondo
	/// </summary>
	protected override void StartLayer()
	{
		Sprite.LoadAsset(Scene);
	}

	/// <summary>
	///		Actualiza los datos de la capa
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
		Sprite.Update(gameContext);
	}

	/// <summary>
	///		Actualiza la capa
	/// </summary>
	protected abstract void UpdateLayer(GameContext gameContext);
	
	/// <summary>
    ///     Nombre de la textura
    /// </summary>
    public Entities.Common.Sprites.SpriteDefinition Sprite { get; }

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