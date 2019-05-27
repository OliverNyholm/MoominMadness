using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{

    public class Game1 : Game
    {
        #region ArcadeStuff
#if (!ARCADE)
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
#else
        public override string GameDisplayName { get { return "MoominMadness"; } }
#endif
        #endregion

        public Game1()
        {
            #region ArcadeStuff
#if (!ARCADE)
            graphics = new GraphicsDeviceManager(this);
#endif
            #endregion
        }
        GameManager gameManager;

        protected override void Initialize()
        {
            Content.RootDirectory = "Content";


            base.Initialize();
        }


        protected override void LoadContent()
        {
            #region ArcadeStuff
#if (!ARCADE)
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
#endif
            #endregion

            TextureManager.LoadContent(Content);
            AudioManager.LoadContent(Content);
            gameManager = new GameManager();
        }


        protected override void Update(GameTime gameTime)
        {
            #region ArcadeStuff
#if (!ARCADE)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
#endif
            #endregion
            // TODO: Add your update logic here

            gameManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            gameManager.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
