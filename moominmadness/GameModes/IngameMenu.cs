using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class IngameMenu
    {
        private Texture2D texture;
        private Vector2 position;

        private bool isPlayerOneRedButtonDown, isPlayerOneBlueButtonDown, isPlayerOneWhiteButtonDown;
        public bool isResumeGame, isCharacterSelect, isMainMenu;


        public IngameMenu()
        {
            this.texture = TextureManager.ingameMenu;
            this.position = new Vector2(581, 329);
        }

        public void Update(GameTime gameTime)
        {
            //------------------- Resume Game ------------------------
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red) && !isPlayerOneRedButtonDown)
                isPlayerOneRedButtonDown = true;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red) && isPlayerOneRedButtonDown)
            {
                isResumeGame = true;
                isPlayerOneRedButtonDown = false;
            }

            //------------------- Go to Character Select------------------------
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue) && !isPlayerOneBlueButtonDown)
                isPlayerOneBlueButtonDown = true;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Blue) && isPlayerOneBlueButtonDown)
            {
                isCharacterSelect = true;
                isPlayerOneBlueButtonDown = false;
            }

            //------------------- Go to Main Menu ------------------------
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.B) && !isPlayerOneWhiteButtonDown)
                isPlayerOneWhiteButtonDown = true;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.B) && isPlayerOneWhiteButtonDown)
            {
                isMainMenu = true;
                isPlayerOneWhiteButtonDown = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }
    }
}
