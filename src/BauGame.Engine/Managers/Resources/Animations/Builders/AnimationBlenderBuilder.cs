namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Animations.Builders;

/// <summary>
///		Generador para <see cref="AnimationBlender"/>
/// </summary>
public class AnimationBlenderBuilder
{
	/// <summary>
	///		Añade un grupo de reglas
	/// </summary>
	public AnimationBlenderBuilder WithGroup(string group)
	{
		// Añade el grupo
		Groups.Add(new AnimationBlenderGroupModel(group));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una animación
	/// </summary>
	public AnimationBlenderBuilder WithAnimation(string texture, string animation, bool loop)
	{
		// Añade la animación
		Groups[Groups.Count - 1].GroupRules.Add(new AnimationBlenderGroupRuleModel(texture, animation, loop));
		// Devuelve el generador
		return this;
	}

	/// <summary>
	///		Añade una regla
	/// </summary>
	public AnimationBlenderBuilder WithRule(string property, AnimationBlenderGroupRuleModel.ConditionOperator condition, bool value)
	{
		return WithRule(property, condition, value ? 1 : 0);
	}

	/// <summary>
	///		Añade una regla
	/// </summary>
	public AnimationBlenderBuilder WithRule(string property, AnimationBlenderGroupRuleModel.ConditionOperator condition, float value)
	{
		AnimationBlenderGroupModel group = Groups[Groups.Count - 1];

			// Añade la animación
			group.GroupRules[group.GroupRules.Count - 1].Rules.Add(new AnimationBlenderGroupRuleModel.Rule(property, condition, value));
			// Devuelve el generador
			return this;
	}

	/// <summary>
	///		Genera los grupos
	/// </summary>
	public List<AnimationBlenderGroupModel> Build() => Groups;

	/// <summary>
	///		Grupos generados
	/// </summary>
	public List<AnimationBlenderGroupModel> Groups { get; } = [];
}
