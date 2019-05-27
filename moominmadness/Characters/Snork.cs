using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class Snork : Character
    {

        private List<Zeppelin> zeppelin = new List<Zeppelin>();

        public Snork(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "Snork";
            this.specialAbilityInfo = "Zeppelin of DOOM";

            shadowDrawPos = new Vector2(8, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 130);
            animationWidth = 93;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 93; runAnimationPos = 186;

            specialAbilityTimer = specialAbilityTimerReset = 3;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 10;

            takeDamageSound = AudioManager.maleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < zeppelin.Count; i++)
            {
                zeppelin[i].Update(gameTime);
                if (zeppelin[i].canDelete)
                {
                    zeppelin.RemoveAt(i);
                    specialAbilityActive = false;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Zeppelin z in zeppelin)
                z.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;

                Vector2 zeppelinPosition = new Vector2(120, 0);
                if (bulletDirection < 0)
                    zeppelinPosition = new Vector2(-40, 0);

                zeppelin.Add(new Zeppelin(position + zeppelinPosition, ref bullets, spriteEffect, bulletDirection));
            }

            base.SpecialAbility(gameTime);
        }
    }
}
