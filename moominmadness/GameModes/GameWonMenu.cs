using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class GameWonMenu
    {
        private Texture2D texture;
        private Vector2 position, characterDrawPosition;

        private bool isPlayerOneRedButtonDown, isPlayerOneBlueButtonDown, isPlayerOneWhiteButtonDown;
        public bool isResumeGame, isCharacterSelect, isMainMenu;

        private List<Character> characters = new List<Character>();
        private int winnerCharacter;
        private bool isPlayerOneWinner;


        public GameWonMenu()
        {
            this.texture = TextureManager.winGameMenu;
            this.position = new Vector2(484, 250);
            this.characterDrawPosition = new Vector2(960, 600);
            SetCharacters();
            winnerCharacter = 20;
            isPlayerOneWinner = true;
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

            characters[winnerCharacter].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.99f);
            if (isPlayerOneWinner)
            {
                spriteBatch.DrawString(TextureManager.statsFont, "Player One", new Vector2(810, 320), Color.Black, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 1);
                characters[winnerCharacter].spriteEffect = SpriteEffects.None;
            }
            if (!isPlayerOneWinner)
            {
                spriteBatch.DrawString(TextureManager.statsFont, "Player Two", new Vector2(810, 320), Color.Black, 0, Vector2.Zero, 0.8f, SpriteEffects.None, 1);
                characters[winnerCharacter].spriteEffect = SpriteEffects.FlipHorizontally;
            }

            characters[winnerCharacter].Draw(spriteBatch);

        }

        public void GetWinner(bool isPlayerOne, int characterOne)
        {
            isPlayerOneWinner = isPlayerOne;
            winnerCharacter = characterOne;
        }

        private void SetCharacters()
        {
            characters.Add(new Moomin(TextureManager.moominTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Snorkmaiden(TextureManager.snorkmaidenTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Pappa(TextureManager.pappaTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Mamma(TextureManager.mammaTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Snork(TextureManager.snorkTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Hemulen(TextureManager.hemulenTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Snufkin(TextureManager.snufkinTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new LittleMy(TextureManager.littlemyTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Sniff(TextureManager.sniffTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new ThingumyAndBob(TextureManager.tofslanTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Stinky(TextureManager.stinkyTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Police(TextureManager.policeTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Fillyjonk(TextureManager.fillyjonkTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Ninny(TextureManager.ninnyTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Tooticky(TextureManager.tootickyTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Postman(TextureManager.thepostmanTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Muskrat(TextureManager.themuskratTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new HobGoblin(TextureManager.thehobgoblinTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Alicia(TextureManager.aliciaTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new Witch(TextureManager.thewitchTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            characters.Add(new TheGroke(TextureManager.thegrokeTexture, characterDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
        }
    }
}
