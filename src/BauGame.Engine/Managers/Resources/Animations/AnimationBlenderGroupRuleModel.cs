namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

/// <summary>
///		Regla de animación
/// </summary>
public class AnimationBlenderGroupRuleModel(string texture, string animation, bool loop)
{
	// Enumerados públicos
	/// <summary>
	///		Operador de condición con el que se evalúa la regla
	/// </summary>
	public enum ConditionOperator
	{
		/// <summary>Mayor que</summary>
		Greater,
		/// <summary>Mayor o igual que</summary>
		GreaterOrEqual,
		/// <summary>Menor que</summary>
		Less,
		/// <summary>Menor o igual que</summary>
		LessOrEqual,
		/// <summary>Igual que</summary>
		Equal,
		/// <summary>Distinto que</summary>
		Distinct
	}
	// Datos de una regla
	public record Rule(string Property, ConditionOperator Condition, float Value);

	/// <summary>
	///		Evalúa la regla contra las propiedades
	/// </summary>
	public bool Evaluate(Actors.Components.Renderers.AnimatorBlenderProperties properties)
	{
		bool validated = true;

			// Comprueba si se cumplen todas las reglas
			foreach (Rule rule in Rules)
				if (validated)
					validated = Evaluate(rule, properties);
			// Devuelve el valor que indica si todas las reglas se cumplen
			return validated;
	}

	/// <summary>
	///		Evalúa una regla contra una serie de propiedades
	/// </summary>
	private bool Evaluate(Rule rule, Actors.Components.Renderers.AnimatorBlenderProperties properties)
	{
		float? value = properties.Properties.Get(rule.Property);

			// Comprueba la propiedad
			if (value is not null)
				switch (rule.Condition)
				{
					case ConditionOperator.Equal:
						return rule.Value == value;
					case ConditionOperator.Distinct:
						return rule.Value != value;
					case ConditionOperator.Greater:
						return rule.Value > value;
					case ConditionOperator.GreaterOrEqual:
						return rule.Value >= value;
					case ConditionOperator.Less:
						return rule.Value < value;
					case ConditionOperator.LessOrEqual:
						return rule.Value <= value;
				}
			// Si ha llegado hasta aquí es porque no se ha encontrado la propiedad en la regla
			return false;
	}

	/// <summary>
	///		Textura sobre la que se define la animación
	/// </summary>
	public string Texture { get; } = texture;

	/// <summary>
	///		Clave de la animación que se rige por esta regla
	/// </summary>
	public string Animation { get; } = animation;

	/// <summary>
	///		Indica si la animación se muestra en bucle
	/// </summary>
	public bool Loop { get; } = loop;

	/// <summary>
	///		Reglas que se deben cumplir
	/// </summary>
	public List<Rule> Rules { get; } = [];
}
