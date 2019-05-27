using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MoominMadness
{
    class Comet
    {
        private Texture2D texture;
        private Vector2 position, origin;
        public Rectangle hitbox;
        private Rectangle animationBox;

        private Vector2 direction;
        private int width, height, animationFrame;
        private float rotation;

        private double animationTimer, animationTimerReset;

        public bool canRemove;
        private bool soundPlayed;

        public Comet(Vector2 position, Vector2 targetPosition)
        {
            this.texture = TextureManager.cometTexture;
            this.width = texture.Width / 2;
            this.height = texture.Height;
            this.position = position;
            this.direction = targetPosition - position;
            direction.Normalize();
            this.origin = new Vector2(680, 200);

            animationBox = new Rectangle(0, 0, width, height);
            animationTimer = animationTimerReset = 15;
        }

        public void Update(GameTime gameTime)
        {
            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animationTimer <= 0)
            {
                animationTimer = animationTimerReset;
                animationFrame++;
                animationBox.X = (animationFrame % 2) * width;
            }

            hitbox = new Rectangle((int)position.X - 200, (int)position.Y - 200, 300, 300);
            position += direction * 10;

            rotation = (float)Math.Atan2(direction.Y, direction.X);

            if (position.Y >= 1550)
                canRemove = true;

            if (hitbox.Y + hitbox.Height <= -10 && !soundPlayed && AudioManager.sound)
            {
                soundPlayed = true;
                AudioManager.cometSound.Play();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, animationBox, Color.White, rotation, origin, 1, SpriteEffects.None, 0.9f);
            spriteBatch.Draw(TextureManager.healthBar, hitbox, null, Color.Yellow, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public float FloatLayerCalculator()
        {
            return 0 + (position.Y + origin.Y) * 0.0005f;
        }
    }
}
