namespace EngineSample.Core.GameLogic.Actors.Characters;

/// <summary>
///		Definición de un personaje
/// </summary>
public class CharacterDefinition(string name)
{
	public void AddExpression(string name, string asset, string? region)
	{
		Expressions.Add(new CharacterExpressionDefinition(name)
									{
										Sprite = new Bau.Libraries.BauGame.Engine.Entities.Common.Sprites.SpriteDefinition(asset, region)
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
	///		Expresiones del personaje
	/// </summary>
	public List<CharacterExpressionDefinition> Expressions { get; } = [];
}
