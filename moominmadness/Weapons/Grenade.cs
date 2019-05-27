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
    class Grenade
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float groundPosition;
        private Vector2 velocity, direction;
        protected SpriteEffects spriteEffect;
        private int width, height, hitboxWidth, hitboxHeight;
        public Rectangle hitbox, shadowHitbox;

        public float directionX, rotation;
        private float floatLayerOffsetY;
        protected float shadowScale;
        public Vector2 shadowPosition;

        private int destroyCounter;
        public bool impact, destroy, isAliciaSpecial;

        public Grenade(Texture2D texture, Vector2 position, float directionX, bool aliciaSpecial)
        {
            this.texture = texture;
            this.position = position;
            this.groundPosition = position.Y + 7;
            this.velocity = new Vector2(0, -20);
            this.directionX = directionX;
            this.width = hitboxWidth = texture.Width;
            this.height = hitboxHeight = texture.Height;
            this.origin = new Vector2(width / 2, height / 2);
            this.floatLayerOffsetY = 30;
            this.shadowScale = 0.5f;
            this.shadowPosition = new Vector2(position.X + width / 2, groundPosition + 23);
            this.destroyCounter = 0;
            this.isAliciaSpecial = aliciaSpecial;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (position.Y >= groundPosition)
            {
                destroy = true;
            }

            direction = new Vector2(directionX, 1);
            direction.Normalize();

            if (!impact)
            {
                velocity.X = 15 * direction.X;
                velocity.Y += 1.5f * direction.Y;
                position += velocity;
            }

            hitbox = new Rectangle((int)position.X + (int)origin.X, (int)position.Y + (int)origin.Y, hitboxWidth, hitboxHeight);
            shadowPosition = new Vector2(position.X + width / 2, groundPosition + 23);
            shadowHitbox = new Rectangle((int)shadowPosition.X + (int)origin.X, (int)shadowPosition.Y, hitboxWidth, hitboxHeight);

            rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            shadowScale = 0.5f - (((groundPosition + 23) - position.Y) * -0.002f);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.playerShadow, shadowPosition, null, new Color(0, 0, 0, 120), 0f, origin, shadowScale, SpriteEffects.None, 0.1f);

            spriteBatch.Draw(texture, position, null, Color.White, rotation, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, FloatLayerCalculator());
            //spriteBatch.Draw(texture, hitbox, null, Color.Red, 0, origin, spriteEffect, FloatLayerCalculator());
            //spriteBatch.Draw(texture, shadowHitbox, null, Color.Blue, 0, origin, spriteEffect, FloatLayerCalculator());
        }

        public float FloatLayerCalculator()
        {
            return 0 + (position.Y + floatLayerOffsetY) * 0.0005f;
        }
    }
}
