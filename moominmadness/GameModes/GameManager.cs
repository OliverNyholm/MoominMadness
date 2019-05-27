using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace MoominMadness
{
    class GameManager
    {
        private Random random = new Random();

        private MainMenu mainMenu;
        private CharacterSelectMenu characterSelectMenu;
        private GamePlayManager gamePlayManager;
        private IngameMenu ingameMenu;
        private GameWonMenu gameWonMenu;


        private List<Song> songs = new List<Song>();
        private int currentSong;

        private enum GameStates
        {
            Start, CharacterSelect, Brawl, IngameMenu, GameWon
        };
        private GameStates gameStates;

        public GameManager()
        {
            mainMenu = new MainMenu();
            characterSelectMenu = new CharacterSelectMenu();
            gamePlayManager = new GamePlayManager();
            ingameMenu = new IngameMenu();
            gameWonMenu = new GameWonMenu();

            gameStates = GameStates.Brawl;


            songs.Add(AudioManager.musicOne);
            songs.Add(AudioManager.musicTwo);
            songs.Add(AudioManager.musicThree);
            songs.Add(AudioManager.musicFour);
            songs.Add(AudioManager.musicFive);
            songs.Add(AudioManager.musicSix);
            songs.Add(AudioManager.musicGrokeOne);
            songs.Add(AudioManager.musicGrokeTwo);

            if (AudioManager.sound)
            {
                MediaPlayer.Play(songs[1]);
                currentSong = 1;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (AudioManager.sound)
                CheckPlayNextSong();

            switch (gameStates)
            {
                case GameStates.Start:
                    mainMenu.Update(gameTime);

                    if (mainMenu.selectCharacter == true)
                    {
                        mainMenu.selectCharacter = false;
                        characterSelectMenu = new CharacterSelectMenu();
                        gameStates = GameStates.CharacterSelect;
                    }
                    break;

                case GameStates.CharacterSelect:
                    characterSelectMenu.Update(gameTime);

                    if (characterSelectMenu.startGame == true)
                    {
                        characterSelectMenu.startGame = false;
                        gamePlayManager.SetPlayerCharacters(characterSelectMenu.PlayerOneCharacter(), characterSelectMenu.PlayerTwoCharacter());
                        gamePlayManager.ResetRound(true);
                        gamePlayManager.ResetScore();
                        gameStates = GameStates.Brawl;
                    }
                    break;

                case GameStates.Brawl:
                    gamePlayManager.Update(gameTime);

                    if (gamePlayManager.isIngameMenu)
                    {
                        gamePlayManager.isIngameMenu = false;
                        gameStates = GameStates.IngameMenu;
                    }

                    if (gamePlayManager.isGameWonMenu)
                    {
                        gamePlayManager.isGameWonMenu = false;
                        gameWonMenu.GetWinner(gamePlayManager.isPlayerOneWinner, gamePlayManager.isPlayerOneWinner ? characterSelectMenu.PlayerOneCharacter() : characterSelectMenu.PlayerTwoCharacter());
                        gameStates = GameStates.GameWon;
                    }
                    break;

                case GameStates.IngameMenu:
                    ingameMenu.Update(gameTime);

                    if (ingameMenu.isResumeGame)
                    {
                        ingameMenu.isResumeGame = false;
                        //gamePlayManager.ResetRound(true);
                        gameStates = GameStates.Brawl;
                    }
                    if (ingameMenu.isCharacterSelect)
                    {
                        ingameMenu.isCharacterSelect = false;
                        characterSelectMenu = new CharacterSelectMenu();
                        gameStates = GameStates.CharacterSelect;
                    }
                    if (ingameMenu.isMainMenu)
                    {
                        ingameMenu.isMainMenu = false;
                        mainMenu = new MainMenu();
                        gameStates = GameStates.Start;
                    }
                    break;
                case GameStates.GameWon:
                    gameWonMenu.Update(gameTime);

                    if (gameWonMenu.isResumeGame)
                    {
                        gameWonMenu.isResumeGame = false;
                        gamePlayManager.ResetRound(true);
                        gameStates = GameStates.Brawl;
                    }
                    if (gameWonMenu.isCharacterSelect)
                    {
                        gameWonMenu.isCharacterSelect = false;
                        characterSelectMenu = new CharacterSelectMenu();
                        gameStates = GameStates.CharacterSelect;
                    }
                    if (gameWonMenu.isMainMenu)
                    {
                        gameWonMenu.isMainMenu = false;
                        mainMenu = new MainMenu();
                        gameStates = GameStates.Start;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (gameStates)
            {
                case GameStates.Start:
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameStates.CharacterSelect:
                    characterSelectMenu.Draw(spriteBatch);
                    break;
                case GameStates.Brawl:
                    gamePlayManager.Draw(spriteBatch);
                    break;
                case GameStates.IngameMenu:
                    gamePlayManager.Draw(spriteBatch);
                    ingameMenu.Draw(spriteBatch);
                    break;
                case GameStates.GameWon:
                    gamePlayManager.Draw(spriteBatch);
                    gameWonMenu.Draw(spriteBatch);
                    break;
            }
        }

        private void CheckPlayNextSong()
        {
            if (MediaPlayer.State != MediaState.Playing && MediaPlayer.PlayPosition.TotalSeconds == 0.0f)
            {
                int randomRangeLength = characterSelectMenu.grokeSelected ? songs.Count : songs.Count - 2;
                int randomRangeStart = characterSelectMenu.grokeSelected ? songs.Count - 2 : 0;

                int nextSong = random.Next(randomRangeStart, randomRangeLength);
                if (nextSong == currentSong)
                    nextSong = random.Next(randomRangeStart, randomRangeLength);

                MediaPlayer.Play(songs[nextSong]);
                currentSong = nextSong;
            }
        }
    }
}
