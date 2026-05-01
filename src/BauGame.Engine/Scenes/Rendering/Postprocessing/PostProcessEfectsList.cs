using Bau.BauEngine.Managers;

namespace Bau.BauEngine.Scenes.Rendering.Postprocessing;

/// <summary>
///		Lista de efectos de postproceso
/// </summary>
public class PostProcessEfectsList(AbstractRenderingManager renderingManager) : Entities.Common.Collections.SecureList<AbstractPostProcessingEffect>
{
    /// <summary>
    ///     Añade un elemento a la lista
    /// </summary>
	protected override void Added(AbstractPostProcessingEffect item)
	{
	}

    /// <summary>
    ///     Arranca los elementos de la lista
    /// </summary>
	public void Start()
	{
        foreach (AbstractPostProcessingEffect effect in Enumerate())
            effect.Start();
	}

    /// <summary>
    ///     Actualiza cuando se ha eliminado un elemento (en este caso no hace nada)
    /// </summary>
	protected override void Removed(AbstractPostProcessingEffect item)
	{
	}

	/// <summary>
	///		Actualiza los datos de los <see cref="AbstractPostProcessingEffect"/> de la lista
	/// </summary>
	protected override void UpdateSelf(GameContext gameContext)
	{
        foreach (AbstractPostProcessingEffect effect in Enumerate()) 
            if (effect.Enabled) 
                effect.Update(gameContext);
			else
				MarkToDestroy(effect, TimeSpan.FromSeconds(2));
	}

	/// <summary>
	///     Manager de presentación
	/// </summary>
	public AbstractRenderingManager RenderingManager { get; } = renderingManager;
}