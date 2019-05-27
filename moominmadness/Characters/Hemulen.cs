using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Hemulen : Character
    {

        public Hemulen(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 5;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Hemulen";
            this.specialAbilityInfo = "Flower Power";

            shadowDrawPos = new Vector2(4, (height / 2) + 4);
            shadowDrawBloatedPos = new Vector2(80, 150);
            animationWidth = 93;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 5;
            idleAnimationPos = 0; shootAnimationPos = 102; runAnimationPos = 204;

            specialAbilityTimer = specialAbilityTimerReset = 0.1f;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            if (specialAbilityActive)
            {
                specialAbilityActive = false;
            }


            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(-10, 0, iconWidth - 9, iconHeight - 36), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                if (AudioManager.sound)
                    AudioManager.grenadeShootSound.Play();

                grenades.Add(new FlowerGrenade(TextureManager.hemulenFlower, bulletStartPos + bulletDirection * new Vector2(200, 0), bulletDirection * 1.5f, Color.Red));
                grenades.Add(new FlowerGrenade(TextureManager.hemulenFlower, bulletStartPos + bulletDirection * new Vector2(0, 100), bulletDirection * 1.5f, Color.Blue));
                grenades.Add(new FlowerGrenade(TextureManager.hemulenFlower, bulletStartPos - bulletDirection * new Vector2(0, 100), bulletDirection * 1.5f, Color.Yellow));
                specialAbilityActive = true;
                specialAbilityAvailable = false;
            }

            base.SpecialAbility(gameTime);
        }


    }
}
