
namespace Bau.Libraries.BauGame.Engine.Core.Scenes.Physics;

/// <summary>
///		Configuración de las capas de físicas
/// </summary>
public class PhysicLayersRelation
{
	// Registros
	private record LayerRelation(int SourceId, int TargetId);

	/// <summary>
	///		Añade dos capas relacionadas
	/// </summary>
	public void AddRelation(int sourceId, int targetId)
	{
		// Intercambia los números de capas para normalizarlo
		(sourceId, targetId) = Normalize(sourceId, targetId);
		// Añade la relación de capa si no existía
		if (!Layers.Any(item => item.SourceId == sourceId && item.TargetId == targetId))
			Layers.Add(new LayerRelation(sourceId, targetId));
	}

	/// <summary>
	///		Comprueba si dos capas colisionan
	/// </summary>
	public bool IsColliding(int first, int second)
	{
		if (Layers.Count == 0 || first == second) // ... si no hay ninguna se considera que todas las capas colisionan y una capa colisiona consigo misma
			return true;
		else
		{
			// Normaliza los códigos de capa
			(first, second) = Normalize(first, second);
			// Comprueba las relaciones entre capas
			foreach (LayerRelation relation in Layers)
				if (relation.SourceId == first && relation.TargetId == second)
					return true;
			// Si llega hasta aquí es porque no colisionan
			return false;
		}
	}

	/// <summary>
	///		Normaliza los códigos de capa
	/// </summary>
	private (int sourceId, int targetId) Normalize(int sourceId, int targetId)
	{
		if (sourceId > targetId)
			return (targetId, sourceId);
		else
			return (sourceId, targetId);
	}

	/// <summary>
	///		Capas relacionadas
	/// </summary>
	private List<LayerRelation> Layers { get; } = [];
}