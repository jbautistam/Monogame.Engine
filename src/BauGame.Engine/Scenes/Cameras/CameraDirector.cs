using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Bau.Libraries.BauGame.Engine.Scenes.Cameras.Definitions;

namespace Bau.Libraries.BauGame.Engine.Scenes.Cameras;

/// <summary>
///     Director de cámaras de una escena
/// </summary>
public class CameraDirector(AbstractScene scene)
{
    /// <summary>
    ///     Crea una cámara de mundo
    /// </summary>
    public WorldCamera CreateWorldCamera(string name, Vector2 origin, CameraViewport cameraViewport, int zIndex)
    {
        WorldCamera camera = new(this, name, origin, cameraViewport, zIndex);

            // Añade la cámara
            Cameras.Add(camera);
            // Devuelve la cámara
            return camera;
    }

    /// <summary>
    ///     Crea una cámara de mundo
    /// </summary>
    public WorldCamera CreateWorldCamera(string name, Vector2 origin, int zIndex)
    {
        return CreateWorldCamera(name, origin, 
                                 new CameraViewport(Scene.WorldDefinition.WorldBounds.X, Scene.WorldDefinition.WorldBounds.Y,
                                                    Scene.WorldDefinition.WorldBounds.Width, Scene.WorldDefinition.WorldBounds.Height), 
                                 zIndex);
    }

    /// <summary>
    ///     Crea una cámara relativa con coordenadas 0 a 1
    /// </summary>
    public ScreenCamera CreateScreenCamera(string name, Vector2 origin, CameraViewport cameraViewport, int zIndex)
    {
        ScreenCamera camera = new(this, name, origin, cameraViewport, zIndex);

            // Añade la cámara
            Cameras.Add(camera);
            // Devuelve la cámara creada
            return camera;
    }

    /// <summary>
    ///     Crea una cámara relativa con coordenadas 0 a 1 que ocupa toda la pantalla
    /// </summary>
    public ScreenCamera CreateScreenCamera(string name, Vector2 origin, int zIndex)
    {
        return CreateScreenCamera(name, origin, new CameraViewport(0, 0, 1, 1), zIndex);
    }

    /// <summary>
    ///     Crea una cámara híbrida
    /// </summary>
    public HybridCamera CreateHybridCamera(string name, Vector2 origin, float pixelsPerUnit, CameraViewport cameraViewport, int zIndex)
    {
        HybridCamera camera = new(this, name, origin, cameraViewport, zIndex)
                                    {
                                        PixelsPerUnit = pixelsPerUnit
                                    };

            // Añade la cámara
            Cameras.Add(camera);
            // Devuelve la cámara creada
            return camera;
    }

    /// <summary>
    ///     Actualiza las cámaras
    /// </summary>
    public void Update(Managers.GameContext gameContext)
    {
        foreach (AbstractCameraBase camera in Cameras)
            camera.Update(gameContext);
    }

/*
    /// <summary>
    ///     Renderiza los comandos sobre la cámara
    /// </summary>
    public void Render(GraphicsDevice device)
    {
        Controllers.SpriteBatchManager spriteBatchManager = new(device);

        // Prepara las texturas comunes
        PrepareCommonTextures(device);
        // Dibuja la pantalla
        if (Scene.Camera is not null)
        {
            // Limpia la pantalla
            spriteBatchManager.Clear();
		    // Comienza el dibujo con la matriz de mundo
            spriteBatchManager.BeginDraw(Scene.Camera.GetMatrixDrawWorld());
            // Si realmente tenemos algo donde dibujar ...
            if (spriteBatchManager.SpriteBatch is not null)
            {
                // Dibuja las capas de fondo / partida
                foreach (Layers.AbstractLayer layer in Scene.LayerManager.Layers)
                    if (layer.Enabled && layer.Type != Layers.AbstractLayer.LayerType.UserInterface)
                        layer.CommandsQueue.Execute(this, spriteBatchManager.SpriteBatch);
		        // Dibuja la capa de log
                // TODO: falta el dibujo del log
                /// GameEngine.Instance.DebugManager.DrawLogFigures(gameContext, camera);
		        // Comienza el dibujo de la interface de usuario
                spriteBatchManager.BeginDraw(null);
                // Dibuja las capas de interface de usuario
                foreach (Layers.AbstractLayer layer in Scene.LayerManager.Layers)
                    if (layer.Enabled && layer.Type == Layers.AbstractLayer.LayerType.UserInterface)
                        layer.CommandsQueue.Execute(this, spriteBatchManager.SpriteBatch);
                // Dibuja la capa de log
                // TODO: falta el dibujo del log
                //GameEngine.Instance.DebugManager.DrawLogStrings(gameContext, camera);
            }
		    // Finaliza el dibujo
            spriteBatchManager.End();
        }
    }

	/// <summary>
	///		Prepara las texturas comunes
	/// </summary>
	private void PrepareCommonTextures(GraphicsDevice device)
	{
		if (WhitePixel is null)
		{
			WhitePixel = new Texture2D(device, 1, 1);
			WhitePixel.SetData([ Color.White ]);
		}
	}
*/

    /// <summary>
    ///     Escena a la que se asocia el director de cámaras
    /// </summary>
    public AbstractScene Scene { get; } = scene;

    /// <summary>
    ///     Cámaras asociadas
    /// </summary>
    public List<AbstractCameraBase> Cameras { get; } = [];

    /// <summary>
    ///     Textura para un pixel blanco (se utiliza para dibujar rectángulos o líneas)
    /// </summary>
    public Texture2D? WhitePixel { get; private set; }
}
