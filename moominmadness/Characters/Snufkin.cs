using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Snufkin : Character
    {
        private Character enemy;
        private bool isPlayerOneEnemy;
        public bool canGetEnemy;

        public Snufkin(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Snufkin";
            this.specialAbilityInfo = "Fishing";

            shadowDrawPos = new Vector2(0, (height / 2) + 4);
            shadowDrawBloatedPos = new Vector2(65, 150);
            animationWidth = 87;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 108; runAnimationPos = 216;

            specialAbilityTimer = specialAbilityTimerReset = 4f;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                Vector2 dragDirection = position - enemy.position;
                dragDirection.Normalize();

                if (isPlayerOneEnemy && enemy.position.X < enemy.rightMaxBounds)
                    enemy.position += dragDirection * 3;
                if (!isPlayerOneEnemy && enemy.position.X > enemy.leftMaxBounds)
                    enemy.position += dragDirection * 3;

                if (enemy.isDead)
                    specialAbilityActive = false;
            }


            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (specialAbilityActive)
            {
                if (spriteEffect == SpriteEffects.None)
                {
                    Vector2 rodPosition = position - new Vector2(-origin.X * 1.3f, origin.Y);
                    //direction is destination - source vectors
                    Vector2 direction = enemy.position - rodPosition;
                    //get the angle from 2 specified numbers (our point)
                    float angle = (float)Math.Atan2(direction.Y, direction.X);
                    //calculate the distance between our two vectors
                    float distance = Vector2.Distance(rodPosition, enemy.position);

                    spriteBatch.Draw(TextureManager.fishingRod, rodPosition, null, playerColor, 0.4f, Vector2.Zero, 1.5f, spriteEffect, FloatLayerCalculator() - 0.001f);
                    spriteBatch.Draw(TextureManager.pixel, rodPosition + new Vector2(18, 12), new Rectangle((int)rodPosition.X, (int)rodPosition.Y - 30, (int)distance, 1), Color.Brown, angle, Vector2.Zero, 1.0f, SpriteEffects.None, FloatLayerCalculator());

                    if(enemy is ThingumyAndBob)
                    {
                        //direction is destination - source vectors
                        Vector2 direction2 = ((ThingumyAndBob)enemy).bobPosition - rodPosition;
                        //get the angle from 2 specified numbers (our point)
                        float angle2 = (float)Math.Atan2(direction2.Y, direction2.X);
                        //calculate the distance between our two vectors
                        float distance2 = Vector2.Distance(rodPosition, ((ThingumyAndBob)enemy).bobPosition);

                        spriteBatch.Draw(TextureManager.pixel, rodPosition + new Vector2(18, 12), new Rectangle((int)rodPosition.X, (int)rodPosition.Y - 30, (int)distance2, 1), Color.Brown, angle2, Vector2.Zero, 1.0f, SpriteEffects.None, FloatLayerCalculator());
                    }
                }
                if (spriteEffect == SpriteEffects.FlipHorizontally)
                {
                    Vector2 rodPosition = position - new Vector2(origin.X + 30, origin.Y);
                    //direction is destination - source vectors
                    Vector2 direction = enemy.position - rodPosition;
                    //get the angle from 2 specified numbers (our point)
                    float angle = (float)Math.Atan2(direction.Y, direction.X);
                    //calculate the distance between our two vectors
                    float distance = Vector2.Distance(rodPosition, enemy.position);

                    spriteBatch.Draw(TextureManager.fishingRod, rodPosition, null, playerColor, -0.4f, Vector2.Zero, 1.5f, spriteEffect, FloatLayerCalculator() - 0.001f);
                    spriteBatch.Draw(TextureManager.pixel, rodPosition + new Vector2(5, 0), new Rectangle((int)rodPosition.X, (int)rodPosition.Y - 30, (int)distance, 1), Color.Brown, angle, Vector2.Zero, 1.0f, SpriteEffects.None, FloatLayerCalculator());

                    if (enemy is ThingumyAndBob)
                    {
                        //direction is destination - source vectors
                        Vector2 direction2 = ((ThingumyAndBob)enemy).bobPosition - rodPosition;
                        //get the angle from 2 specified numbers (our point)
                        float angle2 = (float)Math.Atan2(direction2.Y, direction2.X);
                        //calculate the distance between our two vectors
                        float distance2 = Vector2.Distance(rodPosition, ((ThingumyAndBob)enemy).bobPosition);

                        spriteBatch.Draw(TextureManager.pixel, rodPosition + new Vector2(5, 0), new Rectangle((int)rodPosition.X, (int)rodPosition.Y - 30, (int)distance2, 1), Color.Brown, angle2, Vector2.Zero, 1.0f, SpriteEffects.None, FloatLayerCalculator());
                    }
                }
            }

            base.Draw(spriteBatch);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(-10, 0, iconWidth - 3, iconHeight - 42), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                canGetEnemy = true;
            }

            base.SpecialAbility(gameTime);
        }

        public void SetEnemy(Character enemy, bool isPlayerOne)
        {
            this.enemy = enemy;
            isPlayerOneEnemy = isPlayerOne;
            canGetEnemy = false;
        }
    }
}
