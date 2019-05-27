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
    class Cage
    {
        private Texture2D textureTop, textureBottom, textureRight, textureLeft;
        private Vector2 position, origin;

        private double activeTime;
        private double animationTimer, animationTimerReset;
        private int animationFrame;
        private Rectangle animationBox;
        private int width;
        public float topBounds, leftBounds, rightBounds, botBounds;

        public bool canRemove;

        public Cage(Vector2 position)
        {
            textureTop = TextureManager.cageTopTexture;
            textureBottom = TextureManager.cageBottomTexture;
            textureRight = TextureManager.cageRightTexture;
            textureLeft = TextureManager.cageLeftTexture;

            width = textureTop.Width / 4;
            origin = new Vector2(width / 2, textureTop.Height / 2);
            animationBox = new Rectangle(0, 0, width, textureTop.Height);
            activeTime = 4000;
            animationTimer = animationTimerReset = 50;

            this.position = position + origin / 2;

            topBounds = position.Y - 45;
            botBounds = position.Y + origin.Y + 10;
            leftBounds = position.X;
            rightBounds = position.X + origin.X;
        }

        public void Update(GameTime gameTime)
        {
            animationTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            activeTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (animationTimer <= 0)
            {
                animationTimer = animationTimerReset;
                animationFrame++;
                animationBox.X = (animationFrame % 4) * width;
            }

            if (activeTime <= 0)
                canRemove = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureBottom, position, animationBox, Color.White, 0, origin, 1, SpriteEffects.None, FloatLayerCalculator((int)origin.Y));
            spriteBatch.Draw(textureTop, position, animationBox, Color.White, 0, origin, 1, SpriteEffects.None, FloatLayerCalculator(-(int)origin.Y));
            spriteBatch.Draw(textureRight, position, animationBox, Color.White, 0, origin, 1, SpriteEffects.None, FloatLayerCalculator((int)origin.Y - 1));
            spriteBatch.Draw(textureLeft, position, animationBox, Color.White, 0, origin, 1, SpriteEffects.None, FloatLayerCalculator((int)origin.Y - 1));

        }

        protected float FloatLayerCalculator(int offset)
        {
            return 0 + (position.Y + offset)* 0.0005f;
        }
    }
}
