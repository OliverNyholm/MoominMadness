using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Moomin : Character
    {
        private int explosionsSizeDefault, explosionSizeSpecialAbility;

        public Moomin(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 7;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Moomin";
            this.specialAbilityInfo = "Bigger explosions";

            shadowDrawPos = new Vector2(8, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 130);
            animationWidth = 93;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 7;
            idleAnimationPos = 0; shootAnimationPos = 93; runAnimationPos = 186;
            explosionsSizeDefault = explosionSize = 80; explosionSizeSpecialAbility = 120;

            specialAbilityTimer = specialAbilityTimerReset = 3;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                playerColor = Color.Red;
                explosionSize = explosionSizeSpecialAbility;
            }
            else
            {
                playerColor = Color.White;
                explosionSize = explosionsSizeDefault;
            }

            base.Update(gameTime);
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
