using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MoominMadness
{
    class GamePlayManager
    {
        private Vector2 playerOneStartPosition, playerTwoStartPosition;
        private Character playerOne, playerTwo;
        private int playerOneCharacterIndex, playerTwoCharacterIndex;
        private List<Blood> blood = new List<Blood>();
        private List<Hattifatteners> hattifnatters = new List<Hattifatteners>();
        private List<FilijonkChild> children = new List<FilijonkChild>();
        private int playerOneMatchScore, playerOneGameScore, playerOneWinStreak, playerTwoMatchScore, playerTwoGameScore, playerTwoWinStreak;
        private int winPoint;

        private double roundTime, roundTimeReset, startTime, startTimeReset, roundEndDelayTime, roundEndDelayTimeReset;
        private bool isRoundEnd;
        public static bool isGameStarted;

        private Rectangle topHattifnattersHitbox, bottomHattifnattersHitbox;
        private Vector2 topHattifnatterSpawnPos, bottomHattifnatterSpawnPos;
        private int hattifnattersCount;
        private bool isNextHattifnatterSpawnRightOffset;

        private bool isPlayerOneIngameMenuButtonDown;
        public bool isIngameMenu;
        public bool isGameWonMenu, isPlayerOneWinner;

        public GamePlayManager()
        {
            playerOneStartPosition = new Vector2(320, 720);
            playerTwoStartPosition = new Vector2(1600, 720);

            playerOneCharacterIndex = 9;
            playerTwoCharacterIndex = 16;
            SetPlayerCharacters(playerOneCharacterIndex, playerTwoCharacterIndex);

            playerOneMatchScore = playerOneGameScore = playerOneWinStreak = 0;
            playerTwoMatchScore = playerTwoGameScore = playerTwoWinStreak = 0;
            winPoint = 3;

            roundTime = roundTimeReset = 20;
            startTime = startTimeReset = 4;
            roundEndDelayTime = roundEndDelayTimeReset = 2;
            isGameStarted = false;

            topHattifnatterSpawnPos = new Vector2(0, 260);
            bottomHattifnatterSpawnPos = new Vector2(0, 1080);
        }

        public void Update(GameTime gameTime)
        {

            TimeLogic(gameTime);
            GameLogic();
            TakeDamage();

            if (roundTime <= 1)
                ManageHattifnatters();



            playerOne.Update(gameTime);
            playerTwo.Update(gameTime);
            PlayerSpecialAbilities();

            foreach (Blood blood in blood)
                blood.Update(gameTime);

            foreach (Hattifatteners hattifnatter in hattifnatters)
                hattifnatter.Update(gameTime);

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);

                if (children[i].isExploded)
                {
                    SpawnBlood(children[i].position, children[i].position);
                    SpawnBlood(children[i].position + new Vector2(15, 15), children[i].position + new Vector2(15, 15));
                    SpawnBlood(children[i].position + new Vector2(-15, 15), children[i].position + new Vector2(-15, 15));
                    SpawnBlood(children[i].position + new Vector2(15, -15), children[i].position + new Vector2(15, -15));
                    SpawnBlood(children[i].position + new Vector2(-15, -15), children[i].position + new Vector2(-15, -15));
                }

                if (children[i].canBeRemoved)
                    children.Remove(children[i]);
            }

            //------------------- Go to Ingame Menu ------------------------
            if (InputHandler.IsButtonDown(PlayerIndex.One, PlayerInput.B) && !isPlayerOneIngameMenuButtonDown)
                isPlayerOneIngameMenuButtonDown = true;
            if (InputHandler.IsButtonUp(PlayerIndex.One, PlayerInput.B) && isPlayerOneIngameMenuButtonDown)
            {
                isIngameMenu = true;
                //isGameStarted = false;
                isPlayerOneIngameMenuButtonDown = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.defaultMap, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
            DrawStats(spriteBatch);
            DrawTimers(spriteBatch);

            //spriteBatch.Draw(TextureManager.hattifnatters, topHattifnattersHitbox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0.2f);
            //spriteBatch.Draw(TextureManager.hattifnatters, bottomHattifnattersHitbox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, 0.2f);

            foreach (Blood blood in blood)
                blood.Draw(spriteBatch);

            foreach (Hattifatteners hattifnatter in hattifnatters)
                hattifnatter.Draw(spriteBatch);

            foreach (FilijonkChild child in children)
                child.Draw(spriteBatch);

            //Draw gamecharacters
            playerOne.Draw(spriteBatch);
            playerTwo.Draw(spriteBatch);
        }

        private void DrawStats(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.statsBar, Vector2.Zero, Color.White);
            playerOne.DrawStatsCharacter(spriteBatch, new Vector2(-60, -1));
            playerTwo.DrawStatsCharacter(spriteBatch, new Vector2(1730, -1));

            //Player 1 health. 
            spriteBatch.Draw(TextureManager.healthBar, new Rectangle(196, 44, (int)(691 * (1 - playerOne.percentHealth)), 111), null, new Color(119, 15, 0), 0, Vector2.Zero, SpriteEffects.None, 0.001f);
            spriteBatch.Draw(TextureManager.healthBar, new Rectangle(197, 160, (int)(260 * (1 - playerOne.percentageCooldown)), 35), null, new Color(255, 171, 2), 0, Vector2.Zero, SpriteEffects.None, 0.001f);

            //Player 2 health
            spriteBatch.Draw(TextureManager.healthBar, new Rectangle(1033, 45, (int)(690 * playerTwo.percentHealth), 110), null, Color.Green, 0, Vector2.Zero, SpriteEffects.None, 0.001f);
            spriteBatch.Draw(TextureManager.healthBar, new Rectangle(1463, 160, (int)(260 * playerTwo.percentageCooldown), 35), null, Color.DimGray, 0, Vector2.Zero, SpriteEffects.None, 0.001f); //Special Ability Cooldown

            //Player 1 Score
            spriteBatch.DrawString(TextureManager.statsFont, playerOneMatchScore.ToString(), new Vector2(905, 12), Color.Black);
            spriteBatch.DrawString(TextureManager.statsFont, playerOneWinStreak.ToString(), new Vector2(395, 0), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.001f);
            spriteBatch.DrawString(TextureManager.statsFont, playerOneGameScore.ToString(), new Vector2(580, 0), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.001f);


            ////Player 2 Score
            spriteBatch.DrawString(TextureManager.statsFont, playerTwoMatchScore.ToString(), new Vector2(973, 12), Color.Black);
            spriteBatch.DrawString(TextureManager.statsFont, playerTwoWinStreak.ToString(), new Vector2(1483, 0), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.001f);
            spriteBatch.DrawString(TextureManager.statsFont, playerTwoGameScore.ToString(), new Vector2(1305, 0), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.001f);
        }

        private void DrawTimers(SpriteBatch spriteBatch)
        {
            //RoundTimer
            int intRoundTimer = (int)roundTime;
            if (intRoundTimer >= 10)
                spriteBatch.DrawString(TextureManager.statsFont, intRoundTimer.ToString(), new Vector2(913, 80), Color.Black);
            else if (intRoundTimer >= 1 && intRoundTimer < 10)
                spriteBatch.DrawString(TextureManager.statsFont, intRoundTimer.ToString(), new Vector2(935, 80), Color.Black);
            else
                spriteBatch.DrawString(TextureManager.statsFont, "OT", new Vector2(910, 80), Color.Black);

            //StartTimer
            int intStartTimer = (int)startTime;
            if (!isGameStarted)
                spriteBatch.DrawString(TextureManager.statsFont, intStartTimer.ToString(), new Vector2(890, 550), Color.Black, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
        }

        private void TakeDamage()
        {
            #region Comet Damage from The Muskrat
            //Player one takes damage from players two's bullets
            if (playerTwo is Muskrat && playerTwo.specialAbilityActive)
            {
                for (int i = 0; i < ((Muskrat)playerTwo).comets.Count; i++)
                {
                    if (playerOne.hitbox.Intersects(((Muskrat)playerTwo).comets[i].hitbox) ||
                        playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobHitbox.Intersects(((Muskrat)playerTwo).comets[i].hitbox))
                    {
                        playerOne.TakeDamage(0.06f);
                    }
                }
            }
            //Player two takes damage from players one's bullets
            if (playerOne is Muskrat && playerOne.specialAbilityActive)
            {
                for (int i = 0; i < ((Muskrat)playerOne).comets.Count; i++)
                {
                    if (playerTwo.hitbox.Intersects(((Muskrat)playerOne).comets[i].hitbox) ||
                        playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobHitbox.Intersects(((Muskrat)playerOne).comets[i].hitbox))
                    {
                        playerTwo.TakeDamage(0.06f);
                    }
                }
            }
            #endregion

            #region Bullets Damage For Pappa
            //Player one takes damage from players two's bullets
            for (int i = 0; i < playerTwo.bullets.Count; i++)
            {
                if (playerOne.hitbox.Intersects(playerTwo.bullets[i].hitbox) ||
                    playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobHitbox.Intersects(playerTwo.bullets[i].hitbox))
                {
                    if (playerOne is HobGoblin && playerOne.specialAbilityActive)
                    {
                        ((HobGoblin)playerOne).SpawnCounterBullet(playerTwo.bullets[i]);
                        playerTwo.bullets.RemoveAt(i);
                        break;
                    }

                    if (playerOne.TakeDamage(2))
                    {
                        SpawnBlood(playerTwo.bullets[i].position, new Vector2(playerOne.feetHitbox.X, playerOne.feetHitbox.Y));
                        if(playerOne is ThingumyAndBob)
                            SpawnBlood(playerTwo.bullets[i].position, new Vector2(((ThingumyAndBob)playerOne).bobFeetHitbox.X, ((ThingumyAndBob)playerOne).bobFeetHitbox.Y));
                    }
                    playerTwo.bullets.RemoveAt(i);
                }
            }
            //Player two takes damage from players one's bullets
            for (int i = 0; i < playerOne.bullets.Count; i++)
            {
                if (playerTwo.hitbox.Intersects(playerOne.bullets[i].hitbox) ||
                    playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobHitbox.Intersects(playerOne.bullets[i].hitbox)
                    )
                {
                    if (playerTwo is HobGoblin && playerTwo.specialAbilityActive)
                    {
                        ((HobGoblin)playerTwo).SpawnCounterBullet(playerOne.bullets[i]);
                        playerOne.bullets.RemoveAt(i);
                        break;
                    }

                    if (playerTwo.TakeDamage(2))
                    {
                        SpawnBlood(playerOne.bullets[i].position, new Vector2(playerTwo.feetHitbox.X, playerTwo.feetHitbox.Y));
                        if (playerTwo is ThingumyAndBob)
                            SpawnBlood(playerOne.bullets[i].position, new Vector2(((ThingumyAndBob)playerTwo).bobFeetHitbox.X, ((ThingumyAndBob)playerTwo).bobFeetHitbox.Y));
                    }
                    playerOne.bullets.RemoveAt(i);
                }
            }
            #endregion

            #region Grenade Damage
            //Player one takes damage from players two's grenades if hitbox collides and it's within their shadows or if hitbox collides and has exploded
            for (int i = 0; i < playerTwo.grenades.Count; i++)
            {
                if (playerOne.hitbox.Intersects(playerTwo.grenades[i].hitbox) && playerOne.feetHitbox.Intersects(playerTwo.grenades[i].shadowHitbox)
                    || playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobHitbox.Intersects(playerTwo.grenades[i].hitbox) && ((ThingumyAndBob)playerOne).feetHitbox.Intersects(playerTwo.grenades[i].shadowHitbox))
                {
                    if (playerOne is HobGoblin && playerOne.specialAbilityActive)
                    {
                        ((HobGoblin)playerOne).SpawnCounterGrenade(playerTwo.grenades[i]);
                        playerTwo.grenades.RemoveAt(i);
                        break;
                    }

                    if (playerOne.TakeDamage(3))
                    {
                        SpawnBlood(playerTwo.grenades[i].position, new Vector2(playerOne.feetHitbox.X, playerOne.feetHitbox.Y));
                        if (playerOne is ThingumyAndBob)
                            SpawnBlood(playerTwo.grenades[i].position, new Vector2(((ThingumyAndBob)playerOne).bobFeetHitbox.X, ((ThingumyAndBob)playerOne).bobFeetHitbox.Y));
                    }

                    if (playerTwo.grenades[i].isAliciaSpecial)
                        playerTwo.SpawnBubblePatch(i);
                    else
                    {
                        playerTwo.SpawnExplosion(i);
                        playerTwo.explosions[playerTwo.explosions.Count - 1].hasDamagedPlayer = true;
                    }

                    playerTwo.grenades.RemoveAt(i);
                }
            }
            //Player two takes damage from players one's grenades if hitbox collides and it's within their shadows or if hitbox collides and has exploded
            for (int i = 0; i < playerOne.grenades.Count; i++)
            {
                if (playerTwo.hitbox.Intersects(playerOne.grenades[i].hitbox) && playerTwo.feetHitbox.Intersects(playerOne.grenades[i].shadowHitbox)
                    || playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobHitbox.Intersects(playerOne.grenades[i].hitbox) && ((ThingumyAndBob)playerTwo).feetHitbox.Intersects(playerOne.grenades[i].shadowHitbox))
                {
                    if (playerTwo is HobGoblin && playerTwo.specialAbilityActive)
                    {
                        ((HobGoblin)playerTwo).SpawnCounterGrenade(playerOne.grenades[i]);
                        playerOne.grenades.RemoveAt(i);
                        break;
                    }


                    if (playerTwo.TakeDamage(3))
                    {
                        SpawnBlood(playerOne.grenades[i].position, new Vector2(playerTwo.feetHitbox.X, playerTwo.feetHitbox.Y));
                        if (playerTwo is ThingumyAndBob)
                            SpawnBlood(playerOne.grenades[i].position, new Vector2(((ThingumyAndBob)playerTwo).bobFeetHitbox.X, ((ThingumyAndBob)playerTwo).bobFeetHitbox.Y));
                    }

                    if (playerOne.grenades[i].isAliciaSpecial)
                        playerOne.SpawnBubblePatch(i);
                    else
                    {
                        playerOne.SpawnExplosion(i);
                        playerOne.explosions[playerOne.explosions.Count - 1].hasDamagedPlayer = true;
                    }

                    playerOne.grenades.RemoveAt(i);
                }
            }
            #endregion

            #region Explosion Damage
            for (int i = 0; i < playerTwo.explosions.Count; i++)
            {
                if (playerOne.feetHitbox.Intersects(playerTwo.explosions[i].hitbox) && !playerTwo.explosions[i].hasDamagedPlayer ||
                   playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobFeetHitbox.Intersects(playerTwo.explosions[i].hitbox) && !playerTwo.explosions[i].hasDamagedPlayer)

                {
                    if (playerOne.TakeDamage(2))
                    {
                        SpawnBlood(playerOne.position, new Vector2(playerOne.feetHitbox.X, playerOne.feetHitbox.Y));
                        if (playerOne is ThingumyAndBob)
                            SpawnBlood(playerTwo.explosions[i].position, new Vector2(((ThingumyAndBob)playerOne).bobFeetHitbox.X, ((ThingumyAndBob)playerOne).bobFeetHitbox.Y));
                    }
                    playerTwo.explosions[i].hasDamagedPlayer = true;
                }

                for (int j = 0; j < children.Count; j++)
                {
                    if (children[j].feetHitbox.Intersects(playerTwo.explosions[i].hitbox))
                        children[j].isExploded = true;
                }
            }
            for (int i = 0; i < playerOne.explosions.Count; i++)
            {
                if (playerTwo.feetHitbox.Intersects(playerOne.explosions[i].hitbox) && !playerOne.explosions[i].hasDamagedPlayer ||
                    playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobFeetHitbox.Intersects(playerOne.explosions[i].hitbox) && !playerOne.explosions[i].hasDamagedPlayer)
                {
                    if (playerTwo.TakeDamage(2))
                    {
                        SpawnBlood(playerTwo.position, new Vector2(playerTwo.feetHitbox.X, playerTwo.feetHitbox.Y));
                        if (playerTwo is ThingumyAndBob)
                            SpawnBlood(playerOne.explosions[i].position, new Vector2(((ThingumyAndBob)playerTwo).bobFeetHitbox.X, ((ThingumyAndBob)playerTwo).bobFeetHitbox.Y));
                    }
                    playerOne.explosions[i].hasDamagedPlayer = true;
                }

                for (int j = 0; j < children.Count; j++)
                {
                    if (children[j].feetHitbox.Intersects(playerOne.explosions[i].hitbox))
                        children[j].isExploded = true;
                }
            }

            //------------- Fillyjonks childrens explosions Player one
            for (int j = 0; j < children.Count; j++)
            {
                for (int i = 0; i < children[j].explosions.Count; i++)
                {
                    if (playerTwo.feetHitbox.Intersects(children[j].explosions[i].hitbox) && !children[j].explosions[i].hasDamagedPlayer)
                    {
                        if (playerTwo.TakeDamage(4))
                            SpawnBlood(playerTwo.position, new Vector2(playerTwo.feetHitbox.X, playerTwo.feetHitbox.Y));
                        children[j].explosions[i].hasDamagedPlayer = true;
                    }
                }
            }

            //------------- Fillyjonks childrens explosions Player one
            for (int j = 0; j < children.Count; j++)
            {
                for (int i = 0; i < children[j].explosions.Count; i++)
                {
                    if (playerOne.feetHitbox.Intersects(children[j].explosions[i].hitbox) && !children[j].explosions[i].hasDamagedPlayer)
                    {
                        if (playerOne.TakeDamage(4))
                            SpawnBlood(playerOne.position, new Vector2(playerOne.feetHitbox.X, playerOne.feetHitbox.Y));
                        children[j].explosions[i].hasDamagedPlayer = true;
                    }
                }
            }
            #endregion;

            #region BubblePatch Damage
            for (int i = 0; i < playerTwo.bubblePatch.Count; i++)
            {
                if (playerOne.feetHitbox.Intersects(playerTwo.bubblePatch[i].hitbox) ||
                    playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobFeetHitbox.Intersects(playerTwo.bubblePatch[i].hitbox))
                {
                    playerOne.TakeDamage(0.03f);
                }
                if (playerTwo.feetHitbox.Intersects(playerTwo.bubblePatch[i].hitbox) ||
                    playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobFeetHitbox.Intersects(playerTwo.bubblePatch[i].hitbox))
                {
                    playerTwo.TakeDamage(0.03f);
                }
            }
            for (int i = 0; i < playerOne.bubblePatch.Count; i++)
            {
                if (playerTwo.feetHitbox.Intersects(playerOne.bubblePatch[i].hitbox) ||
                    playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobFeetHitbox.Intersects(playerOne.bubblePatch[i].hitbox))
                {
                    playerTwo.TakeDamage(0.03f);
                }
                if (playerOne.feetHitbox.Intersects(playerOne.bubblePatch[i].hitbox) ||
                    playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobFeetHitbox.Intersects(playerOne.bubblePatch[i].hitbox))
                {
                    playerOne.TakeDamage(0.03f);
                }
            }
            #endregion;

            #region Hattifnatter Damage
            if (playerOne.feetHitbox.Intersects(topHattifnattersHitbox) || playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobFeetHitbox.Intersects(topHattifnattersHitbox)
                || playerOne.feetHitbox.Intersects(bottomHattifnattersHitbox) || playerOne is ThingumyAndBob && playerOne.specialAbilityActive && ((ThingumyAndBob)playerOne).bobFeetHitbox.Intersects(bottomHattifnattersHitbox))
                playerOne.TakeDamage(0.05f);

            if (playerTwo.feetHitbox.Intersects(topHattifnattersHitbox) || playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobFeetHitbox.Intersects(topHattifnattersHitbox)
                || playerTwo.feetHitbox.Intersects(bottomHattifnattersHitbox) || playerTwo is ThingumyAndBob && playerTwo.specialAbilityActive && ((ThingumyAndBob)playerTwo).bobFeetHitbox.Intersects(bottomHattifnattersHitbox))
                playerTwo.TakeDamage(0.05f);
            #endregion
        }

        private void PlayerSpecialAbilities()
        {
            #region ============ Mammas special ability player one ============
            if (playerOne is Mamma && ((Mamma)playerOne).isEnemyFed && !playerTwo.isBloated)
            {
                playerTwo.isBloated = true;
                playerTwo.IncreasePlayerSizeBy(2);
            }
            else if (playerOne is Mamma && !((Mamma)playerOne).isEnemyFed && playerTwo.isBloated)
            {
                playerTwo.isBloated = false;
                playerTwo.IncreasePlayerSizeBy(0.5f);
            }
            // ============ Mammas special ability player two ============
            if (playerTwo is Mamma && ((Mamma)playerTwo).isEnemyFed && !playerOne.isBloated)
            {
                playerOne.isBloated = true;
                playerOne.IncreasePlayerSizeBy(2);
            }
            else if (playerTwo is Mamma && !((Mamma)playerTwo).isEnemyFed && playerOne.isBloated)
            {
                playerOne.isBloated = false;
                playerOne.IncreasePlayerSizeBy(0.5f);
            }

            #endregion

            #region ============ Fillyjonk special ability player one ============
            if (playerOne is Fillyjonk && ((Fillyjonk)playerOne).isChildrenCalled)
            {
                children.Add(new FilijonkChild(new Vector2(-50, 260), 1, ref playerTwo));
                children.Add(new FilijonkChild(new Vector2(-100, 650), 0.8f, ref playerTwo));
                children.Add(new FilijonkChild(new Vector2(-50, 1080), 0.5f, ref playerTwo));
                ((Fillyjonk)playerOne).isChildrenCalled = false;
            }
            // ============ Fillyjonk special ability player two ============
            if (playerTwo is Fillyjonk && ((Fillyjonk)playerTwo).isChildrenCalled)
            {
                children.Add(new FilijonkChild(new Vector2(2000, 260), 1, ref playerOne));
                children.Add(new FilijonkChild(new Vector2(2050, 650), 0.8f, ref playerOne));
                children.Add(new FilijonkChild(new Vector2(2000, 1080), 0.5f, ref playerOne));
                ((Fillyjonk)playerTwo).isChildrenCalled = false;
            }
            #endregion

            #region ============ Stinky special ability player one ============
            if (playerOne is Stinky && ((Stinky)playerOne).isNaughty && !playerTwo.isInvertedMovement)
                playerTwo.isInvertedMovement = true;

            else if (playerOne is Stinky && !((Stinky)playerOne).isNaughty && playerTwo.isInvertedMovement)
                playerTwo.isInvertedMovement = false;

            // ============ Stinky special ability player one ============
            if (playerTwo is Stinky && ((Stinky)playerTwo).isNaughty && !playerOne.isInvertedMovement)
                playerOne.isInvertedMovement = true;

            else if (playerTwo is Stinky && !((Stinky)playerTwo).isNaughty && playerOne.isInvertedMovement)
                playerOne.isInvertedMovement = false;
            #endregion

            #region ============ The Groke's special ability player one ============
            if (playerOne is TheGroke && ((TheGroke)playerOne).isSlowingEnemy && !playerTwo.isFreezing)
                playerTwo.isFreezing = true;

            else if (playerOne is TheGroke && !((TheGroke)playerOne).isSlowingEnemy && playerTwo.isFreezing)
                playerTwo.isFreezing = false;

            // ============ The Groke's special ability player Two ============
            if (playerTwo is TheGroke && ((TheGroke)playerTwo).isSlowingEnemy && !playerOne.isFreezing)
                playerOne.isFreezing = true;

            else if (playerTwo is TheGroke && !((TheGroke)playerTwo).isSlowingEnemy && playerOne.isFreezing)
                playerOne.isFreezing = false;
            #endregion

            #region ============ The Witch's special ability player One ============
            if (playerOne is Witch && ((Witch)playerOne).canSpawnCage)
                ((Witch)playerOne).SpawnKettle(playerTwo);

            // ============ The Witch's special ability player Two ============
            if (playerTwo is Witch && ((Witch)playerTwo).canSpawnCage)
                ((Witch)playerTwo).SpawnKettle(playerOne);
            #endregion

            #region ============ Snufkins's special ability player One ============
            if (playerOne is Snufkin && ((Snufkin)playerOne).canGetEnemy)
                ((Snufkin)playerOne).SetEnemy(playerTwo, false);

            // ============ Snufkins's special ability player Two ============
            if (playerTwo is Snufkin && ((Snufkin)playerTwo).canGetEnemy)
                ((Snufkin)playerTwo).SetEnemy(playerOne, true);

            #endregion

            #region ============ Police special ability player one ============
            if (playerOne is Police && ((Police)playerOne).isArresting && playerTwo.isAbleToShoot)
                playerTwo.isAbleToShoot = false;

            else if (playerOne is Police && !((Police)playerOne).isArresting && !playerTwo.isAbleToShoot)
                playerTwo.isAbleToShoot = true;

            // ============ Police special ability player one ============
            if (playerTwo is Police && ((Police)playerTwo).isArresting && playerOne.isAbleToShoot)
                playerOne.isAbleToShoot = false;

            else if (playerTwo is Police && !((Police)playerTwo).isArresting && !playerOne.isAbleToShoot)
                playerOne.isAbleToShoot = true;
            #endregion

            #region ============ Sniff special ability player one ============
            if (playerOne is Sniff && ((Sniff)playerOne).hasCalledBombPlane)
                ((Sniff)playerOne).SpawnBombPlane(true, ref playerOne.grenades, ref playerTwo.grenades);

            // ============ Police special ability player one ============
            if (playerTwo is Sniff && ((Sniff)playerTwo).hasCalledBombPlane)
                ((Sniff)playerTwo).SpawnBombPlane(false, ref playerOne.grenades, ref playerTwo.grenades);
            #endregion

            #region ============ The Muskrat's special ability player one ============
            if (playerOne is Muskrat && ((Muskrat)playerOne).isSpawningComet)
                ((Muskrat)playerOne).SpawnComet(true, playerTwo.position);

            // ============ Police special ability player one ============
            if (playerTwo is Muskrat && ((Muskrat)playerTwo).isSpawningComet)
                ((Muskrat)playerTwo).SpawnComet(false, playerOne.position);
            #endregion
        }

        private void SpawnBlood(Vector2 impactPosition, Vector2 groundPosition)
        {

            Random random = new Random();
            int randomXDirection = random.Next(200) - 100;
            float randomSize = random.Next(5, 12) * 0.1f;
            blood.Add(new Blood(impactPosition, groundPosition, new Vector2(randomXDirection, 5), randomSize));
            randomXDirection = random.Next(200) - 100;
            randomSize = random.Next(5, 12) * 0.1f;
            blood.Add(new Blood(impactPosition, groundPosition, new Vector2(randomXDirection, 5), randomSize));
            randomXDirection = random.Next(200) - 100;
            randomSize = random.Next(5, 12) * 0.1f;
            blood.Add(new Blood(impactPosition, groundPosition, new Vector2(randomXDirection, 5), randomSize));
        }

        private void GameLogic()
        {
            //-------------------- if player two wins the round ----------------------
            if (playerOne.health <= 0)
            {
                if (!playerOne.isDead)
                {
                    SpawnBlood(playerOne.position, new Vector2(playerOne.feetHitbox.X, playerOne.feetHitbox.Y));
                    playerOne.isDead = true;
                    playerOne.health = 0;
                }

                if (!isRoundEnd)
                {
                    playerTwoMatchScore++;
                    isRoundEnd = true;
                }
            } //-------------------- if player one wins the round ----------------------
            else if (playerTwo.health <= 0)
            {
                if (!playerTwo.isDead)
                {
                    SpawnBlood(playerTwo.position, new Vector2(playerTwo.feetHitbox.X, playerTwo.feetHitbox.Y));
                    playerTwo.isDead = true;
                    playerTwo.health = 0;
                }

                if (!isRoundEnd)
                {
                    playerOneMatchScore++;
                    isRoundEnd = true;
                }
            }

            if (roundEndDelayTime <= 0)
                ResetRound(false);

            //-------------------- if player one wins the game ----------------------
            if (playerOneMatchScore == winPoint && roundEndDelayTime <= 0.5f)
            {
                if (playerOneWinStreak == 0)
                    playerTwoWinStreak = 0;

                playerOneGameScore++;
                playerOneWinStreak++;
                playerOneMatchScore = 0;
                playerTwoMatchScore = 0;

                //ResetRound(true);
                isPlayerOneWinner = true;
                isGameWonMenu = true;//Open menu to play again, change character or go to main menu

                if (AudioManager.sound)
                    AudioManager.gameStartSound.Play();
            }

            //-------------------- if player two wins the game ----------------------
            if (playerTwoMatchScore == winPoint && roundEndDelayTime <= 0.5f)
            {
                if (playerTwoWinStreak == 0)
                    playerOneWinStreak = 0;

                playerTwoGameScore++;
                playerTwoWinStreak++;
                playerTwoMatchScore = 0;
                playerOneMatchScore = 0;

                //ResetRound(true);
                isPlayerOneWinner = false;
                isGameWonMenu = true;//Open menu to play again, change character or go to main menu

                if (AudioManager.sound)
                    AudioManager.gameStartSound.Play();
            }
        }

        public void ResetScore()
        {
            playerTwoMatchScore = 0;
            playerOneMatchScore = 0;
        }

        private void TimeLogic(GameTime gameTime)
        {
            if (!isGameStarted)
                startTime -= gameTime.ElapsedGameTime.TotalSeconds;
            else if (isGameStarted && !isRoundEnd)
            {
                startTime = startTimeReset;
                roundTime -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (isGameStarted && isRoundEnd)
                roundEndDelayTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (startTime <= 1) //1 cause int 
            {
                isGameStarted = true;
            }
        }

        public void ResetRound(bool clearBlood)
        {
            SetPlayerCharacters(playerOneCharacterIndex, playerTwoCharacterIndex);

            children.Clear();
            hattifnatters.Clear();
            hattifnattersCount = 0;
            isNextHattifnatterSpawnRightOffset = false;
            topHattifnattersHitbox = new Rectangle(0, 0, 0, 0);
            bottomHattifnattersHitbox = new Rectangle(0, 0, 0, 0);

            roundTime = roundTimeReset;
            startTime = startTimeReset;
            roundEndDelayTime = roundEndDelayTimeReset;

            if (clearBlood)
                blood.Clear();

            isRoundEnd = false;
            isGameStarted = false;
        }

        public void SetPlayerCharacters(int playerOneIndex, int playerTwoIndex)
        {
            playerOneCharacterIndex = playerOneIndex;
            playerTwoCharacterIndex = playerTwoIndex;

            playerOne = GetPlayerOneCharacters(playerOneCharacterIndex);
            playerTwo = GetPlayerTwoCharacters(playerTwoCharacterIndex);
        }

        private Character GetPlayerOneCharacters(int playerIndex)
        {
            if (playerIndex == 0)
                return new Moomin(TextureManager.moominTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 1)
                return new Snorkmaiden(TextureManager.snorkmaidenTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 2)
                return new Pappa(TextureManager.pappaTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 3)
                return new Mamma(TextureManager.mammaTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 4)
                return new Snork(TextureManager.snorkTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 5)
                return new Hemulen(TextureManager.hemulenTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 6)
                return new Snufkin(TextureManager.snufkinTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 7)
                return new LittleMy(TextureManager.littlemyTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 8)
                return new Sniff(TextureManager.sniffTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 9)
                return new ThingumyAndBob(TextureManager.tofslanTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 10)
                return new Stinky(TextureManager.stinkyTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 11)
                return new Police(TextureManager.policeTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 12)
                return new Fillyjonk(TextureManager.fillyjonkTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 13)
                return new Ninny(TextureManager.ninnyTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 14)
                return new Tooticky(TextureManager.tootickyTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 15)
                return new Postman(TextureManager.thepostmanTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 16)
                return new Muskrat(TextureManager.themuskratTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 17)
                return new HobGoblin(TextureManager.thehobgoblinTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 18)
                return new Alicia(TextureManager.aliciaTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 19)
                return new Witch(TextureManager.thewitchTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else if (playerIndex == 20)
                return new TheGroke(TextureManager.thegrokeTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
            else
                return new Moomin(TextureManager.moominTexture, playerOneStartPosition, PlayerIndex.One, SpriteEffects.None, false);
        }

        private Character GetPlayerTwoCharacters(int playerIndex)
        {
            if (playerIndex == 0)
                return new Moomin(TextureManager.moominTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 1)
                return new Snorkmaiden(TextureManager.snorkmaidenTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 2)
                return new Pappa(TextureManager.pappaTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 3)
                return new Mamma(TextureManager.mammaTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 4)
                return new Snork(TextureManager.snorkTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 5)
                return new Hemulen(TextureManager.hemulenTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 6)
                return new Snufkin(TextureManager.snufkinTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 7)
                return new LittleMy(TextureManager.littlemyTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 8)
                return new Sniff(TextureManager.sniffTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 9)
                return new ThingumyAndBob(TextureManager.tofslanTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 10)
                return new Stinky(TextureManager.stinkyTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 11)
                return new Police(TextureManager.policeTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 12)
                return new Fillyjonk(TextureManager.fillyjonkTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 13)
                return new Ninny(TextureManager.ninnyTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 14)
                return new Tooticky(TextureManager.tootickyTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 15)
                return new Postman(TextureManager.thepostmanTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 16)
                return new Muskrat(TextureManager.themuskratTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 17)
                return new HobGoblin(TextureManager.thehobgoblinTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 18)
                return new Alicia(TextureManager.aliciaTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 19)
                return new Witch(TextureManager.thewitchTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else if (playerIndex == 20)
                return new TheGroke(TextureManager.thegrokeTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
            else
                return new Moomin(TextureManager.moominTexture, playerTwoStartPosition, PlayerIndex.Two, SpriteEffects.FlipHorizontally, false);
        }

        private void ManageHattifnatters()
        {
            if (hattifnatters.Count == 0) //Spawns them first time
                SpawnHattifnatters();

            topHattifnattersHitbox = new Rectangle(0, 260 + 40, 1920, (int)hattifnatters[0].position.Y - (int)topHattifnatterSpawnPos.Y);
            bottomHattifnattersHitbox = new Rectangle(0, (int)hattifnatters[1].position.Y + 50, 1920, 1080 - (int)hattifnatters[1].position.Y);

            if ((hattifnatters[hattifnattersCount].position.Y - 20) >= topHattifnatterSpawnPos.Y)
            {
                SpawnHattifnatters();
                hattifnattersCount += 2;
            }
        }

        private void SpawnHattifnatters()
        {
            if (!isNextHattifnatterSpawnRightOffset)
            {
                hattifnatters.Add(new Hattifatteners(TextureManager.hattifnatters, topHattifnatterSpawnPos, 0.2f));
                hattifnatters.Add(new Hattifatteners(TextureManager.hattifnatters, bottomHattifnatterSpawnPos, -0.2f));
                isNextHattifnatterSpawnRightOffset = true;
            }
            else
            {
                hattifnatters.Add(new Hattifatteners(TextureManager.hattifnatters, new Vector2(topHattifnatterSpawnPos.X + 25, topHattifnatterSpawnPos.Y), 0.2f));
                hattifnatters.Add(new Hattifatteners(TextureManager.hattifnatters, new Vector2(bottomHattifnatterSpawnPos.X + 25, bottomHattifnatterSpawnPos.Y), -0.2f));
                isNextHattifnatterSpawnRightOffset = false;
            }
        }
    }
}
