namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

/// <summary>
///		Regla de animación
/// </summary>
public class AnimationBlenderGroupModel(string group)
{
	/// <summary>
	///		Evalúa la regla contra las propiedades
	/// </summary>
	public AnimationBlenderGroupRuleModel? Evaluate(Actors.Components.Renderers.AnimatorBlenderProperties properties)
	{
		// Comprueba si se cumplen todas las reglas de alguno de los grupos de reglas
		foreach (AnimationBlenderGroupRuleModel groupRule in GroupRules)
			if (groupRule.Evaluate(properties))
				return groupRule;
		// Si ha llegado hasta aquí es porque no se cumple ninguna de las reglas
		return null;
	}

	/// <summary>
	///		Clave de la animación que se rige por esta regla
	/// </summary>
	public string Group { get; } = group;

	/// <summary>
	///		Reglas que se deben cumplir
	/// </summary>
	public List<AnimationBlenderGroupRuleModel> GroupRules { get; } = [];
}
