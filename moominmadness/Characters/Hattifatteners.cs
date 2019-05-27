using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoominMadness
{
    class Hattifatteners
    {
        private Texture2D texture;
        public Vector2 position;
        public Rectangle hitbox;
        private Rectangle animationBox;

        private float direction;
        private int width, height;
        private int animationFrame;
        private double animationTimer, animationTimerReset;

        public Hattifatteners(Texture2D texture, Vector2 position, float direction)
        {
            this.texture = texture;
            this.position = position;
            this.direction = direction;
            this.width = texture.Width;
            this.height = texture.Height / 4;

            animationBox = new Rectangle(0, 0, width, height);
            this.animationTimer = animationTimerReset = 160;
        }

        public void Update(GameTime gameTime)
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, width, height);

            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animationTimer <= 0)
            {
                animationTimer = animationTimerReset;
                animationFrame++;
                animationBox.Y = (animationFrame % 4) * height;
            }

            position.Y += direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y), animationBox, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, FloatLayerCalculator());
        }

        private float FloatLayerCalculator()
        {
            return 0 + (position.Y + height) * 0.0005f;
        }
    }
}
