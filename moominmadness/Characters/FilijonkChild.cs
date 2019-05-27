using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class FilijonkChild
    {
        private Texture2D texture;
        public Vector2 position;
        private Vector2 direction, origin, hitboxOrigin;
        public Rectangle hitbox, feetHitbox, animationBox;
        private Character enemy;

        private int width, height, feetHeight, speed;
        private float size;

        private int animationFrame, animationWidth;
        private double animationTimer;
        private double animationReset;

        public bool isExploded, hasSpawnedExplosion, canBeRemoved;

        public List<Explosion> explosions = new List<Explosion>();

        public FilijonkChild(Vector2 position, float size, ref Character enemy)
        {
            this.texture = TextureManager.fillyjonkChildTexture;
            this.position = position;
            this.size = size;
            this.enemy = enemy;

            this.speed = 2;
            this.width = (int)((texture.Width / 6) * size);
            this.height = (int)(texture.Height * size);
            this.animationWidth = 56;
            //this.feetHeight = 15;
            this.animationBox = new Rectangle(0, 0, texture.Width / 6, texture.Height);
            this.origin = new Vector2(width / 2, height / 2);
            this.hitboxOrigin = new Vector2(width / 2, height / 2);
        }

        public void Update(GameTime gameTime)
        {
            hitbox = new Rectangle((int)position.X - (int)hitboxOrigin.X, (int)position.Y - (int)hitboxOrigin.Y, width, height);
                feetHitbox = new Rectangle((int)position.X - ((int)hitboxOrigin.X), (int)position.Y + ((int)hitboxOrigin.Y / 2), width, feetHeight); //Place hitbox at feet

            direction = new Vector2(enemy.feetHitbox.X + enemy.feetHitbox.Width / 2, enemy.feetHitbox.Y) - position;
            direction.Normalize();

            position += direction * speed;

            Animation(15, 6, animationWidth, gameTime);


            if (Vector2.Distance(new Vector2(enemy.feetHitbox.X + enemy.feetHitbox.Width / 2, enemy.feetHitbox.Y), position) <= 100) //If close enough, blow up!
                speed = 6;

            if (Vector2.Distance(new Vector2(enemy.feetHitbox.X + enemy.feetHitbox.Width / 2, enemy.feetHitbox.Y), position) <= 40) //If close enough, blow up!
                isExploded = true;

            if (isExploded && !hasSpawnedExplosion) //Spawns the explosions
            {
                explosions.Add(new Explosion(TextureManager.explosions, position, 120));
                hasSpawnedExplosion = true;

                if (AudioManager.sound)
                    AudioManager.bigExplosionSound.Play();
            }

            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);

                if (explosions[i].canRemove)
                {
                    explosions.RemoveAt(i);
                    canBeRemoved = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isExploded)
            {
                //---shadow
                spriteBatch.Draw(TextureManager.playerShadow, new Vector2(position.X - width / 3, position.Y + height / 2f), null, new Color(0, 0, 0, 120), 0f, new Vector2(width / 2, height - height / 1.3f), 0.8f * size, SpriteEffects.None, 0.05f);
                //---player
                spriteBatch.Draw(texture, position, animationBox, Color.White, 0, origin, size, direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, FloatLayerCalculator());
            }

           //spriteBatch.Draw(texture, hitbox, null, Color.Green, 0, Vector2.Zero, SpriteEffects.None, 1);
           //spriteBatch.Draw(texture, feetHitbox, null, Color.Blue, 0, Vector2.Zero, SpriteEffects.None, 1);

            foreach (Explosion explosion in explosions)
                explosion.Draw(spriteBatch);
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
            return 0 + position.Y * 0.0005f;
        }
    }
}
