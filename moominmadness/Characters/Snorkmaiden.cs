using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Snorkmaiden : Character
    {
        private bool isShielded;
        private float damageAbsorbed;

        public Snorkmaiden(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 7;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Snorkmaiden";
            this.specialAbilityInfo = "Shield";

            shadowDrawPos = new Vector2(8, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 130);
            animationWidth = 93;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 7;
            idleAnimationPos = 0; shootAnimationPos = 93; runAnimationPos = 186;
            isShielded = false;
            damageAbsorbed = 0;

            specialAbilityTimer = specialAbilityTimerReset = 3;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
                isShielded = true;
            else
                isShielded = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isShielded && !isBloated)
                spriteBatch.Draw(TextureManager.bubbleShield, position - origin - new Vector2(7, 7), null, Color.White * 0.8f, 0, Vector2.Zero, 1, SpriteEffects.None, FloatLayerCalculator() + 0.1f);
            if (isShielded && isBloated)
                spriteBatch.Draw(TextureManager.bubbleShield, position - origin - new Vector2(7, 7), null, Color.White * 0.8f, 0, origin / 2, 2, SpriteEffects.None, FloatLayerCalculator() + 0.1f);

            base.Draw(spriteBatch);
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                damageAbsorbed = 0;
            }

            base.SpecialAbility(gameTime);
        }

        public override bool TakeDamage(float damage)
        {
            if(isShielded)
            {
                damageAbsorbed += damage;
                if (damageAbsorbed >= 2)
                    specialAbilityActive = false;
                return false;
            }
            return base.TakeDamage(damage);
        }
    }
}
