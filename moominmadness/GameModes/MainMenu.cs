using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class MainMenu
    {
        public bool selectCharacter;

        private Vector2 startGameButtonPos, soundButtonPos, attackButtonPos, specialAbilityButtonPos, ingameMenuButtPos;
        private Vector2 textOffset;

        private bool isNewGameButtonPressedPlayerOne, isNewGameButtonPressedPlayerTwo;

        public MainMenu()
        {
            selectCharacter = false;

            startGameButtonPos = new Vector2(400, 400);
            soundButtonPos = new Vector2(1100, 400);
            attackButtonPos = new Vector2(500, 800);
            specialAbilityButtonPos = new Vector2(890, 800);
            ingameMenuButtPos = new Vector2(1280, 800);
            textOffset = new Vector2(210, 40);
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red)) //Just to prevent selecting character automatically next menu
                isNewGameButtonPressedPlayerOne = true;

            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Red))
                isNewGameButtonPressedPlayerTwo = true;

            if ((InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Red) && isNewGameButtonPressedPlayerOne) || (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Red) && isNewGameButtonPressedPlayerTwo))
                selectCharacter = true;


            //if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red) || InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Yellow))
            //{
            //    //if (AudioManager.sound)
            //    //    AudioManager.sound = false;
            //    //else
            //    //    AudioManager.sound = true;
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.mainMenu, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
    }
}
