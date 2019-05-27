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
    public static class AudioManager
    {
        public static bool sound = true;

        #region Music
        public static Song musicOne { get; private set; }
        public static Song musicTwo { get; private set; }
        public static Song musicThree { get; private set; }
        public static Song musicFour { get; private set; }
        public static Song musicFive { get; private set; }
        public static Song musicSix { get; private set; }
        public static Song musicGrokeOne { get; private set; }
        public static Song musicGrokeTwo { get; private set; }
        #endregion

        #region Weapon SoundEffects
        public static SoundEffect grenadeShootSound { get; private set; }
        public static SoundEffect explosionSound { get; private set; }
        public static SoundEffect bigExplosionSound { get; private set; }
        public static SoundEffect pappaRifleSound { get; private set; }
        public static SoundEffect cometSound { get; private set; }
        public static SoundEffect bombPlaneSound { get; private set; }
        public static SoundEffect bombDropSound { get; private set; }
        #endregion

        #region Misc
        public static SoundEffect femaleGrunt { get; private set; }
        public static SoundEffect maleGrunt { get; private set; }
        public static SoundEffect fillyjonkChildCall { get; private set; }
        public static SoundEffect capeSwosh { get; private set; }
        public static SoundEffect gameStartSound { get; private set; }
        public static SoundEffect winSound { get; private set; }
        #endregion

        public static void LoadContent(ContentManager Content)
        {
            musicOne = Content.Load<Song>(@"Audio\Music\moominMusic1");
            musicTwo = Content.Load<Song>(@"Audio\Music\moominMusic2");
            musicThree = Content.Load<Song>(@"Audio\Music\moominMusic3");
            musicFour = Content.Load<Song>(@"Audio\Music\moominMusic4");
            musicFive = Content.Load<Song>(@"Audio\Music\moominMusic5");
            musicSix = Content.Load<Song>(@"Audio\Music\moominMusic6");
            musicGrokeOne = Content.Load<Song>(@"Audio\Music\moominMusicGroke1");
            musicGrokeTwo = Content.Load<Song>(@"Audio\Music\moominMusicGroke2");


            grenadeShootSound = Content.Load<SoundEffect>(@"Audio\Weapons\grenadeShootSound");
            explosionSound = Content.Load<SoundEffect>(@"Audio\Weapons\explosionSound");
            bigExplosionSound = Content.Load<SoundEffect>(@"Audio\Weapons\bigExplosionSound");
            pappaRifleSound = Content.Load<SoundEffect>(@"Audio\Weapons\pappaRifle");
            cometSound = Content.Load<SoundEffect>(@"Audio\Weapons\cometSound");
            bombPlaneSound = Content.Load<SoundEffect>(@"Audio\Weapons\bombPlane");
            bombDropSound = Content.Load<SoundEffect>(@"Audio\Weapons\bombDrop");

            femaleGrunt = Content.Load<SoundEffect>(@"Audio\Misc\femaleGrunt");
            maleGrunt = Content.Load<SoundEffect>(@"Audio\Misc\maleGrunt");
            fillyjonkChildCall = Content.Load<SoundEffect>(@"Audio\Misc\fillyjonkChildCall");
            capeSwosh = Content.Load<SoundEffect>(@"Audio\Misc\capeSwosh");
            gameStartSound = Content.Load<SoundEffect>(@"Audio\Misc\gameStartSound");
            winSound = Content.Load<SoundEffect>(@"Audio\Misc\winSound");
        }
    }
}
