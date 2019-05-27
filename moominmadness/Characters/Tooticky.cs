using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Tooticky : Character
    {

        public Tooticky(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Too-ticky";
            this.specialAbilityInfo = "Heal";

            shadowDrawPos = new Vector2(8, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 130);
            animationWidth = 96;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 96; runAnimationPos = 192;

            specialAbilityTimer = specialAbilityTimerReset = 0.1f;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                health += 1;
                if (health >= maxHealth)
                    health = maxHealth;
                specialAbilityActive = false;
            }


            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 12, iconHeight - 30), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
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
