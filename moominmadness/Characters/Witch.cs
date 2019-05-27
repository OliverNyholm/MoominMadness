using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class Witch : Character
    {
        public bool canSpawnCage;
        List<Cage> cages = new List<Cage>();
        private Character enemy;
        private float enemyRightMaxBounds, enemyLeftMaxBounds, enemyTopMaxBounds, enemyBottomMaxBounds;

        public Witch(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect) : base(texture, position, playerIndex, spriteEffect, isCharacterSelect)
        {
            this.width = iconWidth = texture.Width / 6;
            this.height = iconHeight = texture.Height / 3;
            this.feetHeight = 30;
            this.animationBox = new Rectangle(0, 0, width, height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);

            this.characterName = "The Witch";
            this.specialAbilityInfo = "Cage of Death";

            shadowDrawPos = new Vector2(8, (height / 2));
            shadowDrawBloatedPos = new Vector2(80, 130);
            animationWidth = 96;
            idleAnimationSpeed = 80;
            shootAnimationSpeed = 60; runAnimationSpeed = 15;
            idleAnimationCount = 2; shootAnimationCount = 3; runAnimationCount = 6;
            idleAnimationPos = 0; shootAnimationPos = 96; runAnimationPos = 192;

            specialAbilityTimer = specialAbilityTimerReset = 4;
            specialAbilityCooldownTimer = specialAbilityCooldownTimerReset = 12;

            takeDamageSound = AudioManager.femaleGrunt;

            SetCharacterValues();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < cages.Count; i++)
            {
                cages[i].Update(gameTime);
                if (cages[i].canRemove)
                {
                    cages.RemoveAt(i);
                    specialAbilityActive = false;
                    ResetMaxBounds();
                }
            }

            base.Update(gameTime);
        }

        public override void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 12, iconHeight - 29), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cage cage in cages)
                cage.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        protected override void SpecialAbility(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Blue) && GamePlayManager.isGameStarted && specialAbilityAvailable && !isDead)
            {
                specialAbilityActive = true;
                specialAbilityAvailable = false;
                canSpawnCage = true;
            }

            base.SpecialAbility(gameTime);
        }

        public void SpawnKettle(Character enemy)
        {
            this.enemy = enemy;
            cages.Add(new Cage(enemy.position - enemy.origin));

            //----------- Store old values for reset ----------------------
            enemyRightMaxBounds = enemy.rightMaxBounds;
            enemyLeftMaxBounds = enemy.leftMaxBounds;
            enemyTopMaxBounds = enemy.topMaxBounds;
            enemyBottomMaxBounds = enemy.bottomMaxBounds;

            Cage cage = cages[0];

            enemy.rightMaxBounds = cage.rightBounds > enemy.rightMaxBounds ? enemy.rightMaxBounds : cage.rightBounds;
            enemy.leftMaxBounds = cage.leftBounds < enemy.leftMaxBounds ? enemy.leftMaxBounds : cage.leftBounds;
            enemy.topMaxBounds = cage.topBounds < enemy.topMaxBounds ? enemy.topMaxBounds : cage.topBounds;
            enemy.bottomMaxBounds = cage.botBounds > enemy.bottomMaxBounds ? enemy.bottomMaxBounds : cage.botBounds;

            canSpawnCage = false;
        }

        private void ResetMaxBounds()
        {
            enemy.rightMaxBounds = enemyRightMaxBounds;
            enemy.leftMaxBounds = enemyLeftMaxBounds;
            enemy.topMaxBounds = enemyTopMaxBounds;
            enemy.bottomMaxBounds = enemyBottomMaxBounds;
        }
    }
}
