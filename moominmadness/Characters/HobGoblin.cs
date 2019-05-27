using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class HobGoblin : Character
    {
        private Texture2D capeTexture;
        private Rectangle capeAnimationBox;
        private double capeAnimationTimer, capeAnimationTimerReset;
        private int capeAnimationFrame, capeWidth;

        public HobGoblin(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 5;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "The Hobgoblin";
            this.specialAbilityInfo = "Reflection Cape";

            capeTexture = TextureManager.thehobgoblincapeTexture;
            capeWidth = capeTexture.Width / 5;
            capeAnimationBox = new Rectangle(0, 0, capeTexture.Height, capeWidth);
            capeAnimationTimer = capeAnimationTimerReset = 40;


            shadowDrawSize = 1.5f;
            shadowDrawPos = new Vector2(30, (height / 2) + 14);
            shadowDrawBloatedPos = new Vector2(260, 250);
            animationWidth = 135;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 5;
            idleAnimationPos = 0; shootAnimationPos = 135; runAnimationPos = 270;

            specialAbilityTimer = specialAbilityTimerReset = 6;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
            bulletOrigin.X += 20;
            bulletOrigin.Y += 20;
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive || capeAnimationFrame > 0 && capeAnimationFrame < 7)
            {
                capeAnimationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (capeAnimationTimer <= 0)
            {
                capeAnimationTimer = capeAnimationTimerReset;
                capeAnimationFrame++;
                if (capeAnimationFrame < 4) //Add this to make the cape hang for a while longer
                    capeAnimationBox.X = (capeAnimationFrame % 5) * capeWidth;
            }

            if (capeAnimationFrame == 4)
            {
                specialAbilityActive = false;
            }

            if (capeAnimationFrame == 7)
            {
                capeAnimationFrame = 0;
                capeAnimationBox.X = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(!isDead)
                spriteBatch.Draw(capeTexture, new Vector2((int)position.X, (int)position.Y), capeAnimationBox, playerColor, 0, origin, drawSize, spriteEffect, FloatLayerCalculator());
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(20, 0, iconWidth - 50, iconHeight - 68), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;

                if (AudioManager.sound)
                    AudioManager.capeSwosh.Play();
            }

            base.SpecialAbility(gameTime);
        }

        public void SpawnCounterGrenade(Grenade grenade)
        {
            if (grenade is FlowerGrenade)
                grenades.Add(new FlowerGrenade(grenade.texture, bulletStartPos, grenade.directionX * -1, ((FlowerGrenade)grenade).color));
            else
                grenades.Add(new Grenade(grenade.texture, bulletStartPos, grenade.directionX * -1, grenade.isAliciaSpecial));
        }
        public void SpawnCounterBullet(Bullet bullet)
        {
            bullets.Add(new Bullet(bullet.texture, bulletStartPos, bulletDirection));
        }
    }
}
