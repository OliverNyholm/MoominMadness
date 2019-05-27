using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace MoominMadness
{
    class Fillyjonk : Character
    {
        public bool isChildrenCalled;

        public Fillyjonk(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Mrs. Fillyjonk";
            this.specialAbilityInfo = "Suicide Squad";

            isChildrenCalled = false;

            shadowDrawPos = new Vector2(0, 84);
            shadowDrawBloatedPos = new Vector2(70, 230);
            animationWidth = 90;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 150; runAnimationPos = 300;

            specialAbilityTimer = specialAbilityTimerReset = 500000;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 500000;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
            bulletOrigin.Y += 55;
        }


        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 6, iconHeight - 83), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }


        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                isChildrenCalled = true;

                if (AudioManager.sound)
                    AudioManager.fillyjonkChildCall.Play();
            }

            base.SpecialAbility(gameTime);
        }
    }
}
