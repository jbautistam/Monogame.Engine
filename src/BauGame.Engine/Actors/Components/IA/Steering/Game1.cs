using Microsoft.Xna.Framework;

namespace Bau.Libraries.BauGame.Engine.Actors.Components.IA.Steering;

	public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<Agent> agents;
        private Texture2D pixel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            agents = new List<Agent>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Crear un pixel blanco para dibujar agentes
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            // Crear un agente con Seek y Wander
            var agent = new Agent(new Vector2(100, 100), pixel);
            agent.AddBehavior(new Seek(() => new Vector2(Mouse.GetState().X, Mouse.GetState().Y)), 1.0f);
            agent.AddBehavior(new Wander(), 0.5f);
            agents.Add(agent);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var agent in agents)
            {
                agent.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            foreach (var agent in agents)
            {
                agent.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
