using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;

namespace MoominMadness
{
    class Muskrat : Character
    {
        public bool isSpawningComet;
        public List<Comet> comets = new List<Comet>();

        public Muskrat(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "The Muskrat";
            this.specialAbilityInfo = "Apocalypse";

            shadowDrawPos = new Vector2(4, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 135);
            animationWidth = 96;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 96; runAnimationPos = 192;

            specialAbilityTimer = specialAbilityTimerReset = 12f;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < comets.Count; i++)
            {
                comets[i].Update(gameTime);
                if (comets[i].canRemove)
                {
                    comets.RemoveAt(i);
                    specialAbilityActive = false;
                }
            }

            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 12, iconHeight - 29), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Comet comet in comets)
                comet.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                isSpawningComet = true;
            }

            base.SpecialAbility(gameTime);
        }

        public void SpawnComet(bool isPlayerOne, Vector2 enemyPosition)
        {
            Vector2 spawnPoint = new Vector2(400, -200);
            if(!isPlayerOne)
                spawnPoint = new Vector2(1500, -200);

            comets.Add(new Comet(spawnPoint, enemyPosition));
            isSpawningComet = false;
        }

    }
}
