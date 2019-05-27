using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;

namespace MoominMadness
{
    class Sniff : Character
    {
        public bool hasCalledBombPlane;
        private List<BombPlane> bombPlanes = new List<BombPlane>();

        public Sniff(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Sniff";
            this.specialAbilityInfo = "Capitalism";

            shadowDrawPos = new Vector2(-2, (height / 2) + 6);
            shadowDrawBloatedPos = new Vector2(70, 175);
            animationWidth = 90;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 120; runAnimationPos = 240;

            specialAbilityTimer = specialAbilityTimerReset = 12f;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 15;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
            bulletOrigin.Y += 10;
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < bombPlanes.Count; i++)
            {
                bombPlanes[i].Update(gameTime);
                if (bombPlanes[i].canRemove)
                {
                    bombPlanes.RemoveAt(i);
                    specialAbilityActive = false;
                }
            }

            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(-10, 0, iconWidth - 6, iconHeight - 53), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            foreach (BombPlane bombPlane in bombPlanes)
                bombPlane.Draw(spriteBatch);

        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                hasCalledBombPlane = true;
            }

            base.SpecialAbility(gameTime);
        }

        public void SpawnBombPlane(bool isPlayerOne, ref List<Grenade> playerOneGrenadeList, ref List<Grenade> playerTwoGrenadeList)
        {
            bombPlanes.Add(new BombPlane(isPlayerOne, ref playerOneGrenadeList, ref playerTwoGrenadeList));
            hasCalledBombPlane = false;

            if (AudioManager.sound)
                AudioManager.bombPlaneSound.Play();
        }
    }
}
