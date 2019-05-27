using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MoominMadness
{
    class Blood
    {
        private Texture2D texture;
        private Vector2 position, groundPosition;
        private Vector2 velocity, direction;
        private Rectangle animationBox;
        private float rotation, drawSize;
        private int width, height;
        private bool isOnGround;

        private int animationFrame;
        private double animationTime, animationTimeReset = 20;


        public Blood(Vector2 position, Vector2 groundPosition, Vector2 direction, float drawSize)
        {
            this.texture = TextureManager.blood;
            this.position = position;
            this.groundPosition = new Vector2(groundPosition.X, groundPosition.Y - 10);
            this.width = texture.Width / 4;
            this.height = texture.Height;
            this.drawSize = drawSize;
            this.animationBox = new Rectangle(0, 0, width, height);

            this.direction = direction;
            this.velocity = new Vector2(0, -5);
        }

        public void Update(GameTime gameTime)
        {
            if (animationFrame == 3)
                return;

            if (!isOnGround)
            {
                direction.Normalize();
                velocity.X = 5 * direction.X;
                velocity.Y += 20 * direction.Y;
                position += velocity;
                rotation = (float)Math.Atan2(velocity.Y, velocity.X);
            }
           else
            {
                rotation = 0;
                animationTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (position.Y >= groundPosition.Y)
                isOnGround = true;



            if (animationTime <= 0)
            {
                animationTime = animationTimeReset;
                animationFrame++;
                animationBox.X = (animationFrame % 4) * width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y), animationBox, Color.White, rotation, Vector2.Zero, drawSize, SpriteEffects.None, FloatLayerCalculator());
        }

        private float FloatLayerCalculator()
        {
            return 0 + (position.Y - 45) * 0.0005f;
        }
    }
}
