using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class CharacterSelectMenu
    {
        public bool grokeSelected;
        public bool startGame;
        private bool isPlayerOneReady, isPlayerTwoReady, isPlayerOneButtonDown, isPlayerTwoButtonDown;

        private List<Character> playerOneCharacters = new List<Character>();
        private List<Character> playerTwoCharacters = new List<Character>();
        private Character playerOne, playerTwo;
        private Vector2 hideCharacterPosition, playerOneDrawPosition, playerTwoDrawPosition;
        private Vector2 playerOneFramePosition, playerTwoFramePosition;
        private Vector2 playerOneNamePosition, playerOneInfoPosition, playerTwoNamePosition, playerTwoInfoPosition;
        private int playerOnePosition, playerTwoPosition;
        private int playerOneMovePosition, playerTwoMovePosition;

        public CharacterSelectMenu()
        {
            playerOneMovePosition = playerTwoMovePosition = 0;
            playerOnePosition = 0;
            playerTwoPosition = 1;


            playerOneFramePosition = new Vector2(631, 256);
            playerTwoFramePosition = new Vector2(743, 256);
            hideCharacterPosition = new Vector2(-200, -200);
            playerOneDrawPosition = new Vector2(220, 460);
            playerTwoDrawPosition = new Vector2(1700, 460);

            playerOneNamePosition = new Vector2(180, 800);
            playerOneInfoPosition = new Vector2(220, 1040);
            playerTwoNamePosition = new Vector2(1650, 800);
            playerTwoInfoPosition = new Vector2(1690, 1040);

            LoadPlayerOneCharacters();
            LoadPlayerTwoCharacters();
            playerOne = playerOneCharacters[0];
            playerTwo = playerTwoCharacters[1];
        }

        public void Update(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red) && isPlayerOneReady && isPlayerTwoReady)
            {
                startGame = true;
                if (AudioManager.sound)
                    AudioManager.gameStartSound.Play();
            }

            PlayerOneManager();
            PlayerTwoManager();

            if (playerOnePosition <= playerOneCharacters.Count - 1)
                playerOneCharacters[playerOnePosition].Update(gameTime);

            if (playerTwoPosition <= playerTwoCharacters.Count - 1)
                playerTwoCharacters[playerTwoPosition].Update(gameTime);


            if (playerOnePosition == 20 || playerTwoPosition == 20)
                grokeSelected = true;
            else
                grokeSelected = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw Background Image
            spriteBatch.Draw(TextureManager.characterSelectMenu, Vector2.Zero, Color.White);

            //Draw Character Frame Positions
            spriteBatch.Draw(TextureManager.characterSelectFrame, playerOneFramePosition, Color.Blue);
            spriteBatch.Draw(TextureManager.characterSelectFrame, playerTwoFramePosition, Color.Red);

            //Draw Character Name and Info
                                     //--------------------Player 1 Info ------------------
            float nameSize = 2 - (TextureManager.specialAbilityFont.MeasureString(playerOne.characterName).X / 100);
            nameSize = nameSize < 0.55f ? 0.55f : nameSize;
            Vector2 centeredName = TextureManager.specialAbilityFont.MeasureString(playerOne.characterName) * nameSize;
            spriteBatch.DrawString(TextureManager.statsFont, playerOne.characterName, playerOneNamePosition - centeredName, Color.Black, 0, Vector2.Zero, nameSize, SpriteEffects.None, 1);

            Vector2 centeredInfo = TextureManager.specialAbilityFont.MeasureString(playerOne.specialAbilityInfo);
            spriteBatch.DrawString(TextureManager.specialAbilityFont, playerOne.specialAbilityInfo, playerOneInfoPosition - centeredInfo / 2, Color.DarkSlateBlue);

                                     //--------------------Player 2 Info ------------------
            nameSize = 2 - (TextureManager.specialAbilityFont.MeasureString(playerTwo.characterName).X / 100);
            nameSize = nameSize < 0.55f ? 0.55f : nameSize;
            centeredName = TextureManager.specialAbilityFont.MeasureString(playerTwo.characterName) * nameSize;
            spriteBatch.DrawString(TextureManager.statsFont, playerTwo.characterName, playerTwoNamePosition - centeredName, Color.Black, 0, Vector2.Zero, nameSize, SpriteEffects.None, 1);

            centeredInfo = TextureManager.specialAbilityFont.MeasureString(playerTwo.specialAbilityInfo);
            spriteBatch.DrawString(TextureManager.specialAbilityFont, playerTwo.specialAbilityInfo, playerTwoInfoPosition - centeredInfo / 2, Color.DarkSlateBlue);


            //Draw Characters
            if (playerOnePosition <= playerOneCharacters.Count - 1)
                playerOneCharacters[playerOnePosition].Draw(spriteBatch);

            if (playerTwoPosition <= playerTwoCharacters.Count - 1)
                playerTwoCharacters[playerTwoPosition].Draw(spriteBatch);
        }

        private void PlayerOneManager()
        {
            int oldPosition = playerOnePosition;

            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red) && !isPlayerOneButtonDown)
                isPlayerOneButtonDown = true;
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Red) && isPlayerOneButtonDown && playerOnePosition <= playerOneCharacters.Count - 1 && !isPlayerOneReady)
            {
                playerOne.isMoving = true;
                isPlayerOneReady = true;
                isPlayerOneButtonDown = false;
                if (AudioManager.sound)
                    playerOne.takeDamageSound.Play();
            }

            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue) && !isPlayerOneButtonDown)
                isPlayerOneButtonDown = true;
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Blue) && isPlayerOneButtonDown)
            {
                playerOne.isMoving = false;
                isPlayerOneReady = false;
                isPlayerOneButtonDown = false;
            }

            if (isPlayerOneReady)
                return;

            #region Move Up
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Up) && playerOneMovePosition == 0)
                playerOneMovePosition = 1;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Up) && playerOneMovePosition == 1)
            {
                if (playerOnePosition >= 6 && playerOnePosition <= 17)
                {
                    playerOneFramePosition.Y -= 112;
                    playerOnePosition -= 6;
                }
                else if (playerOnePosition == 18)
                {
                    playerOneFramePosition -= new Vector2(112 / 2, 112);
                    playerOnePosition = 13;
                }
                else if (playerOnePosition == 19)
                {
                    playerOneFramePosition -= new Vector2(112 / 2, 112);
                    playerOnePosition = 14;
                }
                else if (playerOnePosition == 20)
                {
                    playerOneFramePosition -= new Vector2(112 / 2, 112);
                    playerOnePosition = 15;
                }


                playerOneMovePosition = 0;
            }
            #endregion

            #region Move Down
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Down) && playerOneMovePosition == 0)
                playerOneMovePosition = 2;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Down) && playerOneMovePosition == 2)
            {
                if (playerOnePosition < 12)
                {
                    playerOneFramePosition.Y += 112;
                    playerOnePosition += 6;
                }
                else if (playerOnePosition == 12)
                {
                    playerOneFramePosition += new Vector2(112 + 112 / 2, 112);
                    playerOnePosition = 18;
                }
                else if (playerOnePosition == 13)
                {
                    playerOneFramePosition += new Vector2(112 / 2, 112);
                    playerOnePosition = 18;
                }
                else if (playerOnePosition == 14)
                {
                    playerOneFramePosition += new Vector2(112 / 2, 112);
                    playerOnePosition = 19;
                }
                else if (playerOnePosition == 15)
                {
                    playerOneFramePosition += new Vector2(112 / 2, 112);
                    playerOnePosition = 20;
                }
                else if (playerOnePosition == 16)
                {
                    playerOneFramePosition.Y += 112;
                    playerOneFramePosition.X -= 112 / 2;
                    playerOnePosition = 20;
                }
                else if (playerOnePosition == 17)
                {
                    playerOneFramePosition.Y += 112;
                    playerOneFramePosition.X -= 112 + 112 / 2;
                    playerOnePosition = 20;
                }

                playerOneMovePosition = 0;
            }
            #endregion

            #region Move Right
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Right) && playerOneMovePosition == 0)
                playerOneMovePosition = 3;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Right) && playerOneMovePosition == 3)
            {
                if (playerOnePosition != 5 && playerOnePosition != 11 && playerOnePosition != 17 && playerOnePosition != 20)
                {
                    playerOneFramePosition.X += 112;
                    playerOnePosition++;
                }

                playerOneMovePosition = 0;
            }
            #endregion

            #region Move Left
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.Left) && playerOneMovePosition == 0)
                playerOneMovePosition = 4;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.Left) && playerOneMovePosition == 4)
            {
                if (playerOnePosition != 0 && playerOnePosition != 6 && playerOnePosition != 12 && playerOnePosition != 18)
                {
                    playerOneFramePosition.X -= 112;
                    playerOnePosition--;

                }

                playerOneMovePosition = 0;
            }
            #endregion

            if (oldPosition != playerOnePosition && playerOnePosition <= playerOneCharacters.Count - 1)
                playerOne = playerOneCharacters[playerOnePosition];

        }

        private void PlayerTwoManager()
        {
            int oldPosition = playerTwoPosition;

            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Red) && !isPlayerTwoButtonDown)
                isPlayerTwoButtonDown = true;
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Red) && isPlayerTwoButtonDown && playerTwoPosition <= playerTwoCharacters.Count - 1 && !isPlayerTwoReady)
            {
                playerTwo.isMoving = true;
                isPlayerTwoReady = true;
                isPlayerTwoButtonDown = false;
                if (AudioManager.sound)
                    playerTwo.takeDamageSound.Play();
            }

            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Blue) && !isPlayerTwoButtonDown)
                isPlayerTwoButtonDown = true;
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Blue) && isPlayerTwoButtonDown)
            {
                playerTwo.isMoving = false;
                isPlayerTwoReady = false;
                isPlayerTwoButtonDown = false;
            }

            if (isPlayerTwoReady)
                return;

            #region Move Up
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Up) && playerTwoMovePosition == 0)
                playerTwoMovePosition = 1;
            if (InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Up) && playerTwoMovePosition == 1)
            {
                if (playerTwoPosition >= 6 && playerTwoPosition <= 17)
                {
                    playerTwoFramePosition.Y -= 112;
                    playerTwoPosition -= 6;
                }
                else if (playerTwoPosition == 18)
                {
                    playerTwoFramePosition -= new Vector2(112 / 2, 112);
                    playerTwoPosition = 13;
                }
                else if (playerTwoPosition == 19)
                {
                    playerTwoFramePosition -= new Vector2(112 / 2, 112);
                    playerTwoPosition = 14;
                }
                else if (playerTwoPosition == 20)
                {
                    playerTwoFramePosition -= new Vector2(112 / 2, 112);
                    playerTwoPosition = 15;
                }


                playerTwoMovePosition = 0;
            }
            #endregion

            #region Move Down
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Down) && playerTwoMovePosition == 0)
                playerTwoMovePosition = 2;
            if (InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Down) && playerTwoMovePosition == 2)
            {
                if (playerTwoPosition < 12)
                {
                    playerTwoFramePosition.Y += 112;
                    playerTwoPosition += 6;
                }
                else if (playerTwoPosition == 12)
                {
                    playerTwoFramePosition += new Vector2(112 + 112 / 2, 112);
                    playerTwoPosition = 18;
                }
                else if (playerTwoPosition == 13)
                {
                    playerTwoFramePosition += new Vector2(112 / 2, 112);
                    playerTwoPosition = 18;
                }
                else if (playerTwoPosition == 14)
                {
                    playerTwoFramePosition += new Vector2(112 / 2, 112);
                    playerTwoPosition = 19;
                }
                else if (playerTwoPosition == 15)
                {
                    playerTwoFramePosition += new Vector2(112 / 2, 112);
                    playerTwoPosition = 20;
                }
                else if (playerTwoPosition == 16)
                {
                    playerTwoFramePosition.Y += 112;
                    playerTwoFramePosition.X -= 112 / 2;
                    playerTwoPosition = 20;
                }
                else if (playerTwoPosition == 17)
                {
                    playerTwoFramePosition.Y += 112;
                    playerTwoFramePosition.X -= 112 + 112 / 2;
                    playerTwoPosition = 20;
                }

                playerTwoMovePosition = 0;
            }
            #endregion

            #region Move Right
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Right) && playerTwoMovePosition == 0)
                playerTwoMovePosition = 3;
            if (InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Right) && playerTwoMovePosition == 3)
            {
                if (playerTwoPosition != 5 && playerTwoPosition != 11 && playerTwoPosition != 17 && playerTwoPosition != 20)
                {
                    playerTwoFramePosition.X += 112;
                    playerTwoPosition++;
                }

                playerTwoMovePosition = 0;
            }
            #endregion

            #region Move Left
            if (InputHandler.IsButtonDown(PlayerIndex.Two, PlayerInput.Left) && playerTwoMovePosition == 0)
                playerTwoMovePosition = 4;
            if (InputHandler.IsButtonUp(PlayerIndex.Two, PlayerInput.Left) && playerTwoMovePosition == 4)
            {
                if (playerTwoPosition != 0 && playerTwoPosition != 6 && playerTwoPosition != 12 && playerTwoPosition != 18)
                {
                    playerTwoFramePosition.X -= 112;
                    playerTwoPosition--;

                }

                playerTwoMovePosition = 0;
            }
            #endregion

            if (oldPosition != playerTwoPosition && playerTwoPosition <= playerTwoCharacters.Count - 1)
                playerTwo = playerTwoCharacters[playerTwoPosition];
        }

        private void LoadPlayerOneCharacters()
        {
            playerOneCharacters.Add(new Moomin(TextureManager.moominTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Snorkmaiden(TextureManager.snorkmaidenTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Pappa(TextureManager.pappaTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Mamma(TextureManager.mammaTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Snork(TextureManager.snorkTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Hemulen(TextureManager.hemulenTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Snufkin(TextureManager.snufkinTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new LittleMy(TextureManager.littlemyTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Sniff(TextureManager.sniffTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new ThingumyAndBob(TextureManager.tofslanTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Stinky(TextureManager.stinkyTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Police(TextureManager.policeTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Fillyjonk(TextureManager.fillyjonkTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Ninny(TextureManager.ninnyTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Tooticky(TextureManager.tootickyTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Postman(TextureManager.thepostmanTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Muskrat(TextureManager.themuskratTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new HobGoblin(TextureManager.thehobgoblinTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Alicia(TextureManager.aliciaTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new Witch(TextureManager.thewitchTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
            playerOneCharacters.Add(new TheGroke(TextureManager.thegrokeTexture, playerOneDrawPosition, PlayerIndex.One, SpriteEffects.None, true));
        }

        private void LoadPlayerTwoCharacters()
        {
            playerTwoCharacters.Add(new Moomin(TextureManager.moominTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Snorkmaiden(TextureManager.snorkmaidenTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Pappa(TextureManager.pappaTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Mamma(TextureManager.mammaTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Snork(TextureManager.snorkTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Hemulen(TextureManager.hemulenTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Snufkin(TextureManager.snufkinTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new LittleMy(TextureManager.littlemyTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Sniff(TextureManager.sniffTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new ThingumyAndBob(TextureManager.tofslanTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Stinky(TextureManager.stinkyTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Police(TextureManager.policeTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Fillyjonk(TextureManager.fillyjonkTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Ninny(TextureManager.ninnyTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Tooticky(TextureManager.tootickyTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Postman(TextureManager.thepostmanTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Muskrat(TextureManager.themuskratTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new HobGoblin(TextureManager.thehobgoblinTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Alicia(TextureManager.aliciaTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new Witch(TextureManager.thewitchTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
            playerTwoCharacters.Add(new TheGroke(TextureManager.thegrokeTexture, playerTwoDrawPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, true));
        }

        public int PlayerOneCharacter()
        {
            return playerOnePosition;
        }

        public int PlayerTwoCharacter()
        {
            return playerTwoPosition;
        }
    }
}
