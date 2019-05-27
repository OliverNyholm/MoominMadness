using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace MoominMadness
{
    class ThingumyAndBob : Character
    {
        private Texture2D bobTexture;
        public Vector2 bobPosition, bobOffset;
        public Vector2 bobBulletStartPos;
        public Rectangle bobHitbox, bobFeetHitbox;
        private float spreadDistance;
        private bool isReturningToFormation, isHeadingToFormation;

        public ThingumyAndBob(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Thingumy And Bob";
            this.specialAbilityInfo = "Split up";

            shadowDrawSize = 0.5f;
            shadowDrawPos = new Vector2(-3, (height / 2.2f));
            shadowDrawBloatedPos = new Vector2(20, 75);
            animationWidth = 64;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 64; runAnimationPos = 128;

            specialAbilityTimer = specialAbilityTimerReset = 6;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 15;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
            bulletOrigin.X -= 20;
            bulletOrigin.Y -= 20;

            bobTexture = TextureManager.vifslanTexture;
            bobOffset = new Vector2(10, 0);
            spreadDistance = 80;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (specialAbilityActive)
            {
                isReturningToFormation = true; //Set here ready for when special ability finished
                if (Vector2.Distance(position, bobPosition) < spreadDistance && isHeadingToFormation)
                {
                    if (bobFeetHitbox.Y - (feetHitbox.Height / 3) > topMaxBounds)
                        bobOffset.Y -= 2f;
                    else
                    {
                        bobOffset.Y -= 2f;
                        position.Y += 2f;
                    }
                }
                else if (isHeadingToFormation)
                {
                    isHeadingToFormation = false;
                }
            }
            else
            {
                if (bobOffset.Y <= 2 && isReturningToFormation)
                {
                    Vector2 formationDir = (position + new Vector2(20, 0)) - bobPosition;
                    formationDir.Normalize();
                    bobPosition += formationDir * 7;
                    bobOffset.Y += 2f;
                }
                else if (bobOffset.Y >= -1 && isReturningToFormation)
                {
                    isReturningToFormation = false;
                    bobOffset = new Vector2(10, 0);
                    topMaxBounds = 260;
                }
            }
            bobPosition = position + bobOffset;
            bobBulletStartPos = bulletStartPos + bobOffset;

            if (isDead) //Resets hitboxes so no attacks hit on the hidden body
            {
                bobHitbox = new Rectangle(0, 0, 0, 0);
                bobFeetHitbox = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                feetHitbox = new Rectangle((int)position.X - ((int)hitboxOrigin.X), (int)position.Y + ((int)hitboxOrigin.Y / 3), width, feetHeight); //Place hitbox at feet
                bobHitbox = new Rectangle((int)bobPosition.X - (int)hitboxOrigin.X, (int)bobPosition.Y - (int)hitboxOrigin.Y, width, height);
                bobFeetHitbox = new Rectangle((int)bobPosition.X - ((int)hitboxOrigin.X), (int)bobPosition.Y + ((int)hitboxOrigin.Y / 3), width, feetHeight); //Place hitbox at feet
            }
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(bobTexture, position, new Rectangle(4, 7, iconWidth - 14, iconHeight - 24), playerColor, 0, Vector2.Zero, 5, spriteEffect, FloatLayerCalculator() - 0.01f);
            spriteBatch.Draw(texture, position, new Rectangle(-3, -7, iconWidth - 14, iconHeight - 24), playerColor, 0, Vector2.Zero, 5, spriteEffect, FloatLayerCalculator());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                if (!isBloated)
                    spriteBatch.Draw(TextureManager.playerShadow, bobPosition + shadowDrawPos, null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), shadowDrawSize, SpriteEffects.None, 0.05f);
                else
                    spriteBatch.Draw(TextureManager.playerShadow, bobPosition + shadowDrawBloatedPos, null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), shadowDrawSize, SpriteEffects.None, 0.05f);

                spriteBatch.Draw(bobTexture, new Vector2((int)bobPosition.X, (int)bobPosition.Y), animationBox, playerColor, 0, origin, drawSize, spriteEffect, FloatLayerCalculator() - 0.05f);
            }

            //spriteBatch.Draw(texture, bobFeetHitbox, null, Color.Red, 0, Vector2.Zero, spriteEffect, 1);

            base.Draw(spriteBatch);
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                isHeadingToFormation = true;
            }

            base.SpecialAbility(gameTime);
        }
    }
}
