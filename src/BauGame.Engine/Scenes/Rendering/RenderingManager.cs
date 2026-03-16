namespace Bau.Libraries.BauGame.Engine.Scenes.Rendering;

/// <summary>
///		Manager para rendering
/// </summary>
public class RenderingManager
{
	public RenderingManager(AbstractScene scene)
	{
		Scene = scene;
		SpriteBatchController = new SpriteBatchController(this, GameEngine.Instance.MonogameServicesManager.GraphicsDeviceManager.GraphicsDevice);
	}

    /// <summary>
    ///     Comienza el dibujo del mundo
    /// </summary>
	public void BeginDrawWorld()
	{
        SpriteBatchController.Clear();
        SpriteBatchController.BeginDraw(Scene.Camera?.GetMatrixDrawWorld());
	}

    /// <summary>
    ///     Arranca el dibujo de la UI
    /// </summary>
	public void BeginDrawUI()
	{
        SpriteBatchController.BeginDraw(null);
	}

	/// <summary>
	///		Finaliza el dibujo
	/// </summary>
	public void End()
	{
		SpriteBatchController.End();
	}

	/// <summary>
	///		Escena
	/// </summary>
	public AbstractScene Scene { get; }

	/// <summary>
	///		Controlador de dibujo
	/// </summary>
	public SpriteBatchController SpriteBatchController { get; }
}