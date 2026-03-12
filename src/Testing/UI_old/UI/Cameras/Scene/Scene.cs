using Microsoft.Xna.Framework;
using GameEngine.Cameras;
using GameEngine.Math;
using GameEngine.Layers;
using UI.CharactersEngine.Transitions;
using GameEngine.Rendering.PostProcess;
using GameEngine.Transitions;
using GameEngine.Debugging;
using GameEngine.Optimization;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Scene
{
	public class Scene
    {
        public RectangleF WorldBounds { get; set; }
        
        public CameraDirector CameraDirector { get; }
        public LayerDirector LayerDirector { get; }
        public RenderTargetPool RenderTargetPool { get; }
        public PostProcessStack PostProcess { get; }
        public TransitionSystem TransitionSystem { get; }
        public SpatialHash SpatialHash { get; }
        public BehaviorDebugger Debugger { get; set; }

        public Scene(GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font, RectangleF worldBounds)
        {
            WorldBounds = worldBounds;
            
            CameraDirector = new CameraDirector(this, device.Viewport);
            LayerDirector = new LayerDirector();
            RenderTargetPool = new RenderTargetPool(device);
            PostProcess = new PostProcessStack(device, RenderTargetPool);
            TransitionSystem = new TransitionSystem(device, spriteBatch);
            SpatialHash = new SpatialHash(256f);
            Debugger = new BehaviorDebugger(spriteBatch, font, device);
        }

        public void Update(GameTime gameTime)
        {
            CameraDirector.Update(gameTime);
            
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            LayerDirector.Update(delta);
            
            TransitionSystem.Update(gameTime);
            
            SpatialHash.Clear();
            foreach (var layer in LayerDirector.Layers)
            {
                foreach (var actor in layer.Actors)
                {
                    if (actor is ICullable cullable)
                    {
                        SpatialHash.Insert(cullable);
                    }
                }
            }
        }

        public void CollectRenderCommands()
        {
            LayerDirector.CollectRenderCommands(CameraDirector);
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont defaultFont)
        {
            CameraDirector.Render(spriteBatch, defaultFont);
            
            Debugger.DrawCameraDebug(CameraDirector);
            Debugger.DrawLayerDebug(LayerDirector);
        }
    }
}