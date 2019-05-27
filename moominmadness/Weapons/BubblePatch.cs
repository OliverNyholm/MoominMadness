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
    class BubblePatch
    {
        private Texture2D texture;
        public Vector2 position;
        private Vector2 origin;
        public Rectangle hitbox;
        private Rectangle animationBox;
        private int explosionSize, width, height;

        private int animationFrame;
        private double explosionAnimationTimer, explosionAnimationTimerReset;

        public bool canRemove;

        public BubblePatch(Texture2D texture, Vector2 position, int explosionSize)
        {
            this.texture = texture;
            this.position = new Vector2(position.X, position.Y - 20);
            this.width = texture.Width / 4;
            this.height = texture.Height;
            this.origin = new Vector2(width / 2, height / 2);

            this.animationBox = new Rectangle(0, 0, width, height);
            this.explosionAnimationTimer = explosionAnimationTimerReset = 15;
            canRemove = false;
        }

        public void Update(GameTime gameTime)
        {
            this.hitbox = new Rectangle((int)position.X - (int)origin.X, (int)position.Y - (int)origin.Y, width, height);

            explosionAnimationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (explosionAnimationTimer <= 0)
            {
                explosionAnimationTimer = explosionAnimationTimerReset;
                animationFrame++;
                animationBox.X = (animationFrame % 4) * width;
            }

            if (animationFrame >= 400) //400 frames = 6 seconds
                canRemove = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            float drawSize = explosionSize == 80 ? 1 : 1.5f;
            spriteBatch.Draw(texture, new Vector2((int)position.X, (int)position.Y), animationBox, Color.White, 0, origin, drawSize, SpriteEffects.None, FloatLayerCalculator());
            //spriteBatch.Draw(texture, hitbox, null, Color.Black, 0, Vector2.Zero, SpriteEffects.None, FloatLayerCalculator());
        }

        private float FloatLayerCalculator()
        {
            return 0 + (position.Y - 60) * 0.0005f;
        }
    }
}
