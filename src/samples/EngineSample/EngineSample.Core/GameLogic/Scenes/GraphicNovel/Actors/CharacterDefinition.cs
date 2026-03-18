namespace EngineSample.Core.GameLogic.Scenes.GraphicNovel.Actors;

/// <summary>
///		Definición de un personaje
/// </summary>
public class CharacterDefinition(string name, CharacterDefinition.CharacterType type, int logicalLayer, int logicalZOrder)
{
	/// <summary>
	///		Tipo de actor
	/// </summary>
	public enum CharacterType
	{
		/// <summary>Fondo</summary>
		Background,
		/// <summary>Personaje</summary>
		Character
	}

	/// <summary>
	///		Añade una expresión al personaje
	/// </summary>
	public void AddExpression(string name, string asset, string? region)
	{
		Expressions.Add(new CharacterExpressionDefinition(name)
									{
										Sprite = new Bau.BauEngine.Entities.Sprites.SpriteDefinition(asset, region)
									}
						);
	}

	/// <summary>
	///		Obtiene la expresión
	/// </summary>
	public CharacterExpressionDefinition? GetExpression(string expression)
	{
		CharacterExpressionDefinition? definition = Expressions.FirstOrDefault(item => item.Name.Equals(expression, StringComparison.CurrentCultureIgnoreCase));

			// Si no ha encontrado la expresión, obtiene la predeterminada
			if (definition is null)
				definition = Expressions.FirstOrDefault(item => item.Name.Equals(CharacterExpressionDefinition.DefaultType, StringComparison.CurrentCultureIgnoreCase));
			// Devuelve la definición
			return definition;
	}

	/// <summary>
	///		Nombre del personaje
	/// </summary>
	public string Name { get; } = name;

	/// <summary>
	///		Tipo de personaje
	/// </summary>
	public CharacterType Type { get; } = type;

	/// <summary>
	///		Capa lógica
	/// </summary>
	public int LogicalLayer { get; set; } = logicalLayer;

	/// <summary>
	///		ZOrder lógico
	/// </summary>
	public int LogicalZOrder { get; set; } = logicalZOrder;

	/// <summary>
	///		ZOrder calculado
	/// </summary>
	public int ZOrder => 1_000 * LogicalLayer + LogicalZOrder;

	/// <summary>
	///		Expresiones del personaje
	/// </summary>
	public List<CharacterExpressionDefinition> Expressions { get; } = [];
}
