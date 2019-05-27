using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MoominMadness
{
    public static class TextureManager
    {
        public static SpriteFont statsFont { get; private set; }
        public static SpriteFont specialAbilityFont { get; private set; }

        #region Player graphics
        public static Texture2D moominTexture { get; private set; }
        public static Texture2D snorkmaidenTexture { get; private set; }
        public static Texture2D pappaTexture { get; private set; }
        public static Texture2D mammaTexture { get; private set; }
        public static Texture2D fillyjonkTexture { get; private set; }
        public static Texture2D fillyjonkChildTexture { get; private set; }
        public static Texture2D stinkyTexture { get; private set; }
        public static Texture2D ninnyTexture { get; private set; }
        public static Texture2D tootickyTexture { get; private set; }
        public static Texture2D aliciaTexture { get; private set; }
        public static Texture2D hemulenTexture { get; private set; }
        public static Texture2D littlemyTexture { get; private set; }
        public static Texture2D policeTexture { get; private set; }
        public static Texture2D sniffTexture { get; private set; }
        public static Texture2D snorkTexture { get; private set; }
        public static Texture2D snufkinTexture { get; private set; }
        public static Texture2D thehobgoblinTexture { get; private set; }
        public static Texture2D thewitchTexture { get; private set; }
        public static Texture2D themuskratTexture { get; private set; }
        public static Texture2D thepostmanTexture { get; private set; }
        public static Texture2D tofslanTexture { get; private set; }
        public static Texture2D vifslanTexture { get; private set; }
        public static Texture2D thegrokeTexture { get; private set; }

        public static Texture2D hattifnatters { get; private set; }
        public static Texture2D playerShadow { get; private set; }
        public static Texture2D bubbleShield { get; private set; }
        public static Texture2D thehobgoblincapeTexture { get; private set; }
        public static Texture2D kettleFrontTexture { get; private set; }
        public static Texture2D kettleBackTexture { get; private set; }
        public static Texture2D zeppelinTexture { get; private set; }

        #endregion

        #region Maps
        public static Texture2D defaultMap { get; private set; }

        #endregion

        #region Bullets
        public static Texture2D standardBullet { get; private set; }
        public static Texture2D explosions { get; private set; }
        public static Texture2D hemulenFlower { get; private set; }
        public static Texture2D bubblePatch { get; private set; }

        #endregion

        #region UI
        public static Texture2D mainMenu { get; private set; }
        public static Texture2D characterSelectMenu { get; private set; }
        public static Texture2D characterSelectFrame { get; private set; }
        public static Texture2D ingameMenu { get; private set; }
        public static Texture2D winGameMenu { get; private set; }
        public static Texture2D statsBar { get; private set; }
        public static Texture2D healthBar { get; private set; }
        public static Texture2D button { get; private set; }

        #endregion

        #region Misc
        public static Texture2D blood { get; private set; }
        public static Texture2D cageLeftTexture { get; private set; }
        public static Texture2D cageRightTexture { get; private set; }
        public static Texture2D cageTopTexture { get; private set; }
        public static Texture2D cageBottomTexture { get; private set; }
        public static Texture2D pixel { get; private set; }
        public static Texture2D fishingRod { get; private set; }
        public static Texture2D planeShadow { get; private set; }
        public static Texture2D cometTexture { get; private set; }
        public static Texture2D noShootingIcon { get; private set; }
        public static Texture2D invertedMovementIcon { get; private set; }
        #endregion

        public static void LoadContent(ContentManager Content)
        {
            statsFont = Content.Load<SpriteFont>(@"FuturaHandwritten");
            specialAbilityFont = Content.Load<SpriteFont>(@"SpecialAbilityInfoFont");

            #region Player graphics
            moominTexture = Content.Load<Texture2D>(@"Characters\MoomiSprite");
            snorkmaidenTexture = Content.Load<Texture2D>(@"Characters\SnorkmaidenSprite");
            pappaTexture = Content.Load<Texture2D>(@"Characters\PappaSprite");
            mammaTexture = Content.Load<Texture2D>(@"Characters\MammaSprite");
            fillyjonkTexture = Content.Load<Texture2D>(@"Characters\FillyfjonkaSprite");
            fillyjonkChildTexture = Content.Load<Texture2D>(@"Characters\FilifjonkaChildSprite");
            stinkyTexture = Content.Load<Texture2D>(@"Characters\StinkySprite");
            ninnyTexture = Content.Load<Texture2D>(@"Characters\NinnySprite");
            tootickyTexture = Content.Load<Texture2D>(@"Characters\TootikkiSprite");
            aliciaTexture = Content.Load<Texture2D>(@"Characters\AliciaSprite");
            hemulenTexture = Content.Load<Texture2D>(@"Characters\HemulSprite");
            littlemyTexture = Content.Load<Texture2D>(@"Characters\littlemySprite");
            policeTexture = Content.Load<Texture2D>(@"Characters\PoliceSprite");
            sniffTexture = Content.Load<Texture2D>(@"Characters\SniffSprite");
            snorkTexture = Content.Load<Texture2D>(@"Characters\SnorkSprite");
            snufkinTexture = Content.Load<Texture2D>(@"Characters\SnufkinSprite");
            thehobgoblinTexture = Content.Load<Texture2D>(@"Characters\thehobgoblinSprite");
            thewitchTexture = Content.Load<Texture2D>(@"Characters\thewitchSprite");
            themuskratTexture = Content.Load<Texture2D>(@"Characters\themuskratSprite");
            thepostmanTexture = Content.Load<Texture2D>(@"Characters\thepostmanSprite");
            tofslanTexture = Content.Load<Texture2D>(@"Characters\tofslanSprite");
            vifslanTexture = Content.Load<Texture2D>(@"Characters\vifslanSprite");
            thegrokeTexture = Content.Load<Texture2D>(@"Characters\thegrokeSprite");


            hattifnatters = Content.Load<Texture2D>(@"Characters\Hattifnatters");
            playerShadow = Content.Load<Texture2D>(@"Characters\shadow");
            bubbleShield = Content.Load<Texture2D>(@"Characters\Bubble");
            thehobgoblincapeTexture = Content.Load<Texture2D>(@"Characters\thehobgoblincapeSprite");
            kettleFrontTexture = Content.Load<Texture2D>(@"Characters\KettleFront");
            kettleBackTexture = Content.Load<Texture2D>(@"Characters\KettleBack");
            zeppelinTexture = Content.Load<Texture2D>(@"Characters\Zeppelin");

        #endregion

            #region Maps
            defaultMap = Content.Load<Texture2D>(@"Maps\moominBackground");
            #endregion

            #region Bullets
            standardBullet = Content.Load<Texture2D>(@"Bullets\Bullet");
            explosions = Content.Load<Texture2D>(@"Bullets\Explosion");
            hemulenFlower = Content.Load<Texture2D>(@"Bullets\HemulenFlower");
            bubblePatch = Content.Load<Texture2D>(@"Bullets\BubbleGrenade");
            #endregion

            #region UI
            mainMenu = Content.Load<Texture2D>(@"UI\MainMenu");
            characterSelectMenu = Content.Load<Texture2D>(@"UI\CharacterMenu");
            characterSelectFrame = Content.Load<Texture2D>(@"UI\PlayerSelectionFrame");
            ingameMenu = Content.Load<Texture2D>(@"UI\IngameMenu");
            winGameMenu = Content.Load<Texture2D>(@"UI\WinnerScreen");
            statsBar = Content.Load<Texture2D>(@"UI\StatsBar");
            healthBar = Content.Load<Texture2D>(@"UI\HealthBar");
            button = Content.Load<Texture2D>(@"UI\Button");
            #endregion

            #region Misc
            blood = Content.Load<Texture2D>(@"Misc\Blood");
            cageLeftTexture = Content.Load<Texture2D>(@"Misc\CageLeft");
            cageRightTexture = Content.Load<Texture2D>(@"Misc\CageRight");
            cageTopTexture = Content.Load<Texture2D>(@"Misc\CageTop");
            cageBottomTexture = Content.Load<Texture2D>(@"Misc\CageBottom");
            pixel = Content.Load<Texture2D>(@"Misc\pixel");
            fishingRod = Content.Load<Texture2D>(@"Misc\FishingRod");
            planeShadow = Content.Load<Texture2D>(@"Misc\PlaneShadow");
            cometTexture = Content.Load<Texture2D>(@"Misc\CometSprite");
            noShootingIcon = Content.Load<Texture2D>(@"Misc\NoShootingIcon");
            invertedMovementIcon = Content.Load<Texture2D>(@"Misc\InvertedMovementIcon");
            #endregion
        }
    }
}
