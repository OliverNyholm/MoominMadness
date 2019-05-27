using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class TheGroke : Character
    {

        public bool isSlowingEnemy;

        public TheGroke(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 3;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2f, height / 2f);

            this.characterName = "The Groke";
            this.specialAbilityInfo = "Permafrost";

            isSlowingEnemy = false;

            shadowDrawPos = new Vector2(0, (height / 2));
            shadowDrawBloatedPos = new Vector2(100, 100);
            animationWidth = 132;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 8;
            idleAnimationCount = 3; shootAnimationCount = 3; runAnimationCount = 3;
            idleAnimationPos = 0; shootAnimationPos = 128; runAnimationPos = 256;

            specialAbilityTimer = specialAbilityTimerReset = 3;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
            bulletOrigin.X += 40;
            bulletOrigin.Y += 40;
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                isSlowingEnemy = true;
            }
            else
                isSlowingEnemy = false;

            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(40, 10, iconWidth - 47, iconHeight - 62), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
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
