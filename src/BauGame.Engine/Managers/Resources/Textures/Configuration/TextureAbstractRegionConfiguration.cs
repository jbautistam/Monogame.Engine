namespace Bau.BauEngine.Managers.Resources.Textures.Configuration;

/// <summary>
///		Configuración abstracta de una región para una textura
/// </summary>
public abstract class TextureAbstractRegionConfiguration(string name)
{
	/// <summary>
	///		Obtiene el rectángulo dentro de la textura
	/// </summary>
	public abstract Microsoft.Xna.Framework.Rectangle GetBounds(Microsoft.Xna.Framework.Graphics.Texture2D texture);

	/// <summary>
	///		Identificador
	/// </summary>
	public string Name { get; } = name;
}