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
    class Bullet
    {
        public Texture2D texture;
        public Vector2 position;
        private Vector2 velocity, direction;
        private SpriteEffects spriteEffect;
        private int width, height;
        public Rectangle hitbox;

        private float directionX;
        private float floatLayerOffsetY;
        public double bulletTimer;

        public Bullet(Texture2D texture, Vector2 position, float directionX)
        {
            this.texture = texture;
            this.position = position;
            this.directionX = directionX;
            this.width = texture.Width;
            this.height = texture.Height;
            floatLayerOffsetY = 0;
        }

        public void Update(GameTime gameTime)
        {
            bulletTimer += gameTime.ElapsedGameTime.TotalSeconds;
            hitbox = new Rectangle((int)position.X + width / 2, (int)position.Y + 25, width, height);

            direction = new Vector2(directionX, 0);
            direction.Normalize();

            velocity = 25 * direction;
            position += velocity;

            if (direction.X < 0)
                spriteEffect = SpriteEffects.None;
            else
                spriteEffect = SpriteEffects.FlipHorizontally;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, new Vector2(position.X + width / 2, position.Y + 30), null, new Color(0, 0, 0, 120), 0f, new Vector2(63 / 2, 21 / 2), 0.5f, SpriteEffects.None, 0.1f);

            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1, spriteEffect, FloatLayerCalculator());
            //spriteBatch.Draw(texture, hitbox, Color.Red);
        }

        public float FloatLayerCalculator()
        {
            return 0 + (position.Y + floatLayerOffsetY) * 0.0005f;
        }
    }
}
