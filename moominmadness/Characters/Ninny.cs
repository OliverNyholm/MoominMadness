using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Ninny : Character
    {

        public bool isInvisible;

        public Ninny(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Ninny";
            this.specialAbilityInfo = "Invisibility";

            isInvisible = false;

            shadowDrawPos = new Vector2(0, (height / 2) + 2);
            shadowDrawBloatedPos = new Vector2(80, 145);
            animationWidth = 87;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 108; runAnimationPos = 216;

            specialAbilityTimer = specialAbilityTimerReset = 5;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 13;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                isInvisible = true;
            }
            else
                isInvisible = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isInvisible)
                base.Draw(spriteBatch);
            else
            {
                foreach (Bullet bullet in bullets)
                    bullet.Draw(spriteBatch);

                foreach (Grenade grenade in grenades)
                    grenade.Draw(spriteBatch);

                foreach (Explosion explosion in explosions)
                    explosion.Draw(spriteBatch);
            }
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 6, iconHeight - 41), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
            }

            base.SpecialAbility(gameTime);
        }


    }
}
