using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class LittleMy : Character
    {
        double dashTime, dashTimeReset;

        public LittleMy(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Little My";
            this.specialAbilityInfo = "Dash";
            shadowDrawSize = 0.5f;
            shadowDrawPos = new Vector2(-3, (height / 2.2f));
            shadowDrawBloatedPos = new Vector2(20, 75);
            animationWidth = 64;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 64; runAnimationPos = 128;

            specialAbilityTimer = specialAbilityTimerReset = 1;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 6;
            dashTime = dashTimeReset = 50;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
            bulletOrigin.X -= 20;
            bulletOrigin.Y -= 20;
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                dashTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
                if ((!isInvertedMovement ? position.X < rightMaxBounds : position.X > leftMaxBounds) && (!isInvertedMovement ? position.X > leftMaxBounds : position.X < rightMaxBounds) &&
                    (!isInvertedMovement ? position.Y > topMaxBounds : position.Y < bottomMaxBounds) && (!isInvertedMovement ? position.Y < bottomMaxBounds : position.Y > topMaxBounds))
                    position += speed * 8;

                if (dashTime < 0)
                {
                    dashTime = dashTimeReset;
                    specialAbilityActive = false;
                }
            }

            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(-3, 10, iconWidth - 14, iconHeight - 24), playerColor, 0, Vector2.Zero, 5, spriteEffect, FloatLayerCalculator());
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
