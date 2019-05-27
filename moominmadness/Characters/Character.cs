using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace MoominMadness
{
    class Character
    {
        protected Texture2D texture;
        public Vector2 position;
        protected PlayerIndex playerIndex;
        public float health, percentHealth, maxHealth, percentageCooldown;

        protected Vector2 speed;
        public float rightMaxBounds, leftMaxBounds, topMaxBounds, bottomMaxBounds;

        public bool isMoving, isDead, isBloated, isInvertedMovement, isFreezing, isAbleToShoot, specialAbilityActive;
        protected bool shooting, isShootButtonPressed, specialAbilityAvailable;
        public double specialAbilityTimer, specialAbilityTimerReset, specialAbilityCooldownTimer, specialAbilityCooldownTimerUI, specialAbilityCooldownTimerReset;
        public string characterName, specialAbilityInfo;
        public bool inCharacterSelect;
        protected float bulletDirection, bulletDirectionReset;
        public List<Bullet> bullets = new List<Bullet>();
        public List<Grenade> grenades = new List<Grenade>();
        public List<Explosion> explosions = new List<Explosion>();
        public List<BubblePatch> bubblePatch = new List<BubblePatch>();
        protected Vector2 bulletStartPos, bulletOrigin;

        public Vector2 origin, hitboxOrigin;
        public Rectangle hitbox, feetHitbox;
        protected Rectangle animationBox;
        protected int width, height, feetHeight, iconWidth, iconHeight, explosionSize;
        public SpriteEffects spriteEffect;
        protected Vector2 shadowDrawPos, shadowDrawBloatedPos;
        protected float drawSize, shadowDrawSize;
        protected Color playerColor;

        public SoundEffect takeDamageSound;
        private bool hurtTalk;
        private double hurtTalkTimer;

        #region Animations
        private int animationFrame;
        private double animationTimer;
        private double animationReset;
        protected int animationWidth, animationCount, animationSpeed;
        protected int idleAnimationSpeed, shootAnimationSpeed, runAnimationSpeed;
        protected int idleAnimationCount, shootAnimationCount, runAnimationCount;
        protected int idleAnimationPos, shootAnimationPos, runAnimationPos;
        #endregion

        public Character(Texture2D texture, Vector2 position, PlayerIndex playerIndex, SpriteEffects spriteEffect, bool isCharacterSelect)
        {
            this.texture = texture;
            this.position = position;
            this.playerIndex = playerIndex;
            this.health = this.maxHealth = 10;
            this.speed = new Vector2(0, 0);
            this.spriteEffect = spriteEffect;
            this.inCharacterSelect = isCharacterSelect;
            this.animationFrame = 0;
            this.animationCount = idleAnimationCount;
            this.animationSpeed = idleAnimationSpeed;
            this.explosionSize = 80;
            this.drawSize = 1;
            this.shadowDrawSize = 0.8f;
            this.specialAbilityAvailable = true;
            this.playerColor = Color.White;
            this.isAbleToShoot = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (inCharacterSelect)
            {
                //moving = true;
                SetCurrentAnimation();
                Animation(animationSpeed, animationCount, animationWidth, gameTime);
                drawSize = 3;
                return;
            }

            if (isDead) //Resets hitboxes so no attacks hit on the hidden body
            {
                hitbox = new Rectangle(0, 0, 0, 0);
                feetHitbox = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                hitbox = new Rectangle((int)position.X - (int)hitboxOrigin.X, (int)position.Y - (int)hitboxOrigin.Y, width, height);
                feetHitbox = new Rectangle((int)position.X - ((int)hitboxOrigin.X), (int)position.Y + ((int)hitboxOrigin.Y / 2), width, feetHeight); //Place hitbox at feet
            }


            bulletStartPos = new Vector2(position.X + bulletOrigin.X, position.Y + bulletOrigin.Y);

            percentHealth = health / maxHealth;
            percentageCooldown = (float)(specialAbilityCooldownTimerUI / specialAbilityCooldownTimerReset);

            speed = new Vector2(0, 0);
            if (isAbleToShoot)
                Shooting(gameTime);
            Moving(gameTime);

            SpecialAbility(gameTime);
            SetCurrentAnimation();
            Animation(animationSpeed, animationCount, animationWidth, gameTime);

            if (isInvertedMovement)
                speed *= -1;
            if (isFreezing)
            {
                playerColor = Color.CornflowerBlue;
                speed *= 0.5f;
            }
            else
                playerColor = Color.White;

            if (this is Postman && ((Postman)this).isFast)
                speed *= 2;

            position += speed;


            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(gameTime);

                if (bullets[i].bulletTimer >= 3)
                    bullets.RemoveAt(i);
            }

            for (int i = 0; i < grenades.Count; i++)
            {
                grenades[i].Update(gameTime);

                if (grenades[i].destroy)
                {
                    if (grenades[i].isAliciaSpecial)
                        SpawnBubblePatch(i);
                    else
                        SpawnExplosion(i);
                    grenades.RemoveAt(i);
                }
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);

                if (explosions[i].canRemove)
                    explosions.RemoveAt(i);
            }

            for (int i = 0; i < bubblePatch.Count; i++)
            {
                bubblePatch[i].Update(gameTime);

                if (bubblePatch[i].canRemove)
                    bubblePatch.RemoveAt(i);
            }


            if (hurtTalk)
            {
                if (hurtTalkTimer > 0)
                    hurtTalkTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
                else
                {
                    hurtTalk = false;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                if (!isBloated)
                    spriteBatch.Draw(TextureManager.playerShadow, position + shadowDrawPos, null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), shadowDrawSize, SpriteEffects.None, 0.05f);
                else
                    spriteBatch.Draw(TextureManager.playerShadow, position + shadowDrawBloatedPos, null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), shadowDrawSize, SpriteEffects.None, 0.05f);

                spriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y), animationBox, playerColor, 0, origin, drawSize, spriteEffect, FloatLayerCalculator());
            }

            //spriteBatch.Draw(texture, hitbox, null, Color.Yellow, 0, Vector2.Zero, spriteEffect, 1);
            //spriteBatch.Draw(texture, feetHitbox, null, Color.Red, 0, Vector2.Zero, spriteEffect, 1);

            if(isInvertedMovement)
                spriteBatch.Draw(TextureManager.invertedMovementIcon, new Vector2((int)position.X + origin.X - TextureManager.invertedMovementIcon.Width / 2, (int)position.Y - origin.Y - TextureManager.invertedMovementIcon.Height / 2), null, Color.White, 0, origin, drawSize, spriteEffect, FloatLayerCalculator());
            if (!isAbleToShoot)
                spriteBatch.Draw(TextureManager.noShootingIcon, new Vector2((int)position.X + origin.X - TextureManager.noShootingIcon.Width / 2, (int)position.Y - origin.Y - TextureManager.noShootingIcon.Height / 2), null, Color.White, 0, origin, drawSize, spriteEffect, FloatLayerCalculator());



            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);

            foreach (Grenade grenade in grenades)
                grenade.Draw(spriteBatch);

            foreach (Explosion explosion in explosions)
                explosion.Draw(spriteBatch);

            foreach (BubblePatch bubble in bubblePatch)
                bubble.Draw(spriteBatch);
        }

        public virtual void DrawStatsCharacter(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, iconWidth - 9, iconHeight - 26), playerColor, 0, Vector2.Zero, 3, spriteEffect, FloatLayerCalculator());
        }

        private void Moving(GameTime gameTime)
        {
            #region Walk Right
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Right) && !shooting && (!isInvertedMovement ? feetHitbox.X + (feetHitbox.Width / 2) < rightMaxBounds : feetHitbox.X + (feetHitbox.Width / 2) > leftMaxBounds))
            {

                speed.X = 4.5f;

                isMoving = true;
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Down))
                    speed.X = 3f;
            }
            #endregion
            #region Walk Left
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) && !shooting && (!isInvertedMovement ? feetHitbox.X + (feetHitbox.Width / 2) > leftMaxBounds : feetHitbox.X + (feetHitbox.Width / 2) < rightMaxBounds))
            {
                speed.X = -4.5f;
                isMoving = true;
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Down))
                    speed.X = -3f;
            }
            #endregion
            #region Walk Up
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Up) && !shooting && (!isInvertedMovement ? feetHitbox.Y - feetHitbox.Height > topMaxBounds : feetHitbox.Y - feetHitbox.Height < bottomMaxBounds))
            {
                if (this is ThingumyAndBob && specialAbilityActive && !isInvertedMovement && ((ThingumyAndBob)this).bobFeetHitbox.Y - (feetHitbox.Height / 2) <= topMaxBounds)
                    return;

                speed.Y = -4.5f;
                isMoving = true;
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                    speed.Y = -3f;
            }
            #endregion
            #region Walk Down
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Down) && !shooting && (!isInvertedMovement ? feetHitbox.Y - feetHitbox.Height < bottomMaxBounds : feetHitbox.Y - feetHitbox.Height > topMaxBounds))
            {
                if (this is ThingumyAndBob && specialAbilityActive && isInvertedMovement && ((ThingumyAndBob)this).bobFeetHitbox.Y - (feetHitbox.Height / 2) <= topMaxBounds)
                    return;

                speed.Y = 4.5f;
                isMoving = true;
                if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Left) || InputHandler.IsButtonDown(playerIndex, PlayerInput.Right))
                    speed.Y = 3f;
            }
            #endregion
            #region Moving Bool
            if (!(InputHandler.IsButtonDown(playerIndex, PlayerInput.Left)) && !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Right)) &&
                !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Up)) && !(InputHandler.IsButtonDown(playerIndex, PlayerInput.Down)) || shooting)
            {
                isMoving = false;
            }
            #endregion
        }

        #region Legacy Shooting bullets
        //private void Shooting(GameTime gameTime)
        //{
        //    if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Red) && gameStarted)
        //    {
        //        if (!shooting)
        //        {
        //            animationBox.X = 0;
        //            animationFrame = 0;
        //        }
        //        shooting = true;
        //        if (animationFrame == 2)
        //        {
        //            //bullets.Add(new Bullet(TextureManager.standardBullet, bulletStartPos, bulletDirection));
        //            grenades.Add(new Grenade(TextureManager.standardBullet, bulletStartPos, bulletDirection));
        //            animationFrame = 0;
        //        }
        //    }
        //    else
        //    {
        //        if (shooting)
        //        {
        //            animationBox.X = 0;
        //            animationFrame = 0;
        //        }
        //        shooting = false;

        //    }
        //}
        #endregion

        private void Shooting(GameTime gameTime)
        {
            if (InputHandler.IsButtonDown(playerIndex, PlayerInput.Red) && GamePlayManager.isGameStarted && !shooting && !isDead)
            {
                if (Math.Abs(bulletDirection) < 2.4f)
                    bulletDirection += bulletDirection / 60;
                isShootButtonPressed = true;
            }
            if (InputHandler.IsButtonUp(playerIndex, PlayerInput.Red) && GamePlayManager.isGameStarted)
            {
                if (isShootButtonPressed)
                {
                    shooting = true;
                    animationBox.X = 0;
                    animationFrame = 0;
                    isShootButtonPressed = false;
                }
            }

            if (animationFrame == 2 && shooting)
            {
                if (this is Alicia && specialAbilityActive)
                    grenades.Add(new Grenade(TextureManager.standardBullet, bulletStartPos, bulletDirection, true));
                else
                {
                    grenades.Add(new Grenade(TextureManager.standardBullet, bulletStartPos, bulletDirection, false));
                    if (this is ThingumyAndBob && specialAbilityActive)
                        grenades.Add(new Grenade(TextureManager.standardBullet, ((ThingumyAndBob)this).bobBulletStartPos, bulletDirection, false));
                }

                if (AudioManager.sound)
                    AudioManager.grenadeShootSound.Play();

                bulletDirection = bulletDirectionReset;
                animationBox.X = 0;
                animationFrame = 0;
                shooting = false;
            }
        }

        public void SpawnExplosion(int i)
        {
            explosions.Add(new Explosion(TextureManager.explosions, grenades[i].position, explosionSize));

            if (AudioManager.sound)
                AudioManager.explosionSound.Play();
        }

        public void SpawnBubblePatch(int i)
        {
            bubblePatch.Add(new BubblePatch(TextureManager.bubblePatch, new Vector2(grenades[i].position.X, grenades[i].groundPosition), explosionSize));
        }

        protected virtual void SpecialAbility(GameTime gameTime)
        {
            if (specialAbilityActive) //Activates timer for how long special ability available
            {
                specialAbilityTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                specialAbilityCooldownTimerUI = specialAbilityCooldownTimerReset;
            }
            if (specialAbilityTimer <= 0 && specialAbilityActive) //When timer reaches end, reset timer and set special ability as false
            {
                specialAbilityTimer = specialAbilityTimerReset;
                specialAbilityActive = false;
            }
            if (specialAbilityCooldownTimer >= 0 && !specialAbilityAvailable && !specialAbilityActive) //When special ability is over, start cooldown timer
            {
                specialAbilityCooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                specialAbilityCooldownTimerUI -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (specialAbilityCooldownTimer <= 0 && !specialAbilityAvailable && !specialAbilityActive) //When cooldown reaches 0, set specialAbility as avaiable
            {
                specialAbilityCooldownTimer = specialAbilityCooldownTimerReset;
                specialAbilityAvailable = true;
            }
        }

        public virtual bool TakeDamage(float damage)
        {
            if (isDead)
                return false;

            this.health -= damage;

            if (!hurtTalk && AudioManager.sound)
            {
                takeDamageSound.Play();
                hurtTalkTimer = takeDamageSound.Duration.TotalMilliseconds;
                hurtTalk = true;
            }

            return true;
        }

        private void SetCurrentAnimation()
        {
            if (isMoving && !shooting)
            {
                animationBox.Y = runAnimationPos;
                animationSpeed = runAnimationSpeed;
                animationCount = runAnimationCount;
            }
            else if (shooting && !isMoving)
            {
                animationBox.Y = shootAnimationPos;
                animationSpeed = shootAnimationSpeed;
                animationCount = shootAnimationCount;
            }
            else if (!isMoving && !shooting)
            {
                animationBox.Y = idleAnimationPos;
                animationSpeed = idleAnimationSpeed;
                animationCount = idleAnimationCount;
            }
        }

        private void Animation(int animationSpeed, int animationLength, int animationWidth, GameTime gameTime)
        {
            animationReset = animationSpeed;

            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animationTimer <= 0)
            {
                animationTimer = animationReset;
                animationFrame++;
                animationBox.X = (animationFrame % animationLength) * animationWidth;
            }

            if (animationFrame > 2000000)
                animationFrame = 0;
        }

        protected float FloatLayerCalculator()
        {
            float positionY = position.Y + origin.Y;
            if (isBloated)
                positionY = position.Y + origin.Y * drawSize;

            if (inCharacterSelect)
                return 0.95f;
            else
                return 0 + (positionY) * 0.0005f;
        }

        protected void SetCharacterValues()
        {
            topMaxBounds = 260; //200+ for UI, 60+ for grass above
            bottomMaxBounds = 1080 - origin.Y;
            if (playerIndex == PlayerIndex.One)
            {
                leftMaxBounds = 0 + origin.X;
                rightMaxBounds = 900 - origin.X;
                bulletDirection = bulletDirectionReset = 1;
                bulletOrigin = new Vector2(-origin.X + 78, -origin.Y + 55);
            }
            else
            {
                leftMaxBounds = 1015 + origin.X;
                rightMaxBounds = 1920 - origin.X;
                bulletDirection = bulletDirectionReset = -1;
                bulletOrigin = new Vector2(-origin.X, -origin.Y + 55);
            }
        }

        public void IncreasePlayerSizeBy(float size)
        {
            float tempHeight = height * size; //Fullösning för att kunna köra * 2 och * 0.5 med floats.
            float tempWidth = width * size;
            float tempFeetHeight = feetHeight * size;

            height = (int)tempHeight;
            width = (int)tempWidth;
            feetHeight = (int)tempFeetHeight;
            drawSize *= size;
            shadowDrawSize *= size;
            hitboxOrigin = new Vector2(width / 2, height / 2);
        }

    }
}
