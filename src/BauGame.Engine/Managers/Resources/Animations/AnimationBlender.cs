namespace Bau.Libraries.BauGame.Engine.Managers.Resources.Animations;

/// <summary>
///		Manager para animaciones
/// </summary>
public class AnimationBlender
{
	/// <summary>
	///		Añade una serie de grupos de reglas de animación
	/// </summary>
	public void Add(Builders.AnimationBlenderBuilder builder)
	{
		AddRange(builder.Build());
	}

	/// <summary>
	///		Añade un grupo de reglas de animación
	/// </summary>
	public void Add(AnimationBlenderGroupModel group)
	{
		GroupRules.Add(group.Group, group);
	}

	/// <summary>
	///		Añade una serie de grupos de reglas de animación
	/// </summary>
	public void AddRange(List<AnimationBlenderGroupModel> groups)
	{
		foreach (AnimationBlenderGroupModel group in groups)
			GroupRules.Add(group.Group, group);
	}

	/// <summary>
	///		Evalúa las reglas definidas con una serie de propiedades para comprobar cuál se cumple
	/// </summary>
	public AnimationBlenderGroupRuleModel? EvaluateRules(Actors.Components.Renderers.AnimatorBlenderProperties properties)
	{
		return GroupRules.Get(properties.Group)?.Evaluate(properties);
	}

	/// <summary>
	///		Reglas de animación
	/// </summary>
	public Base.DictionaryModel<AnimationBlenderGroupModel> GroupRules { get; } = new();
}
