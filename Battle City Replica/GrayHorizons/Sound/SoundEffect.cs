using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using System.Collections.Generic;
using GrayHorizons.Extensions;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Sound
{
    public class SoundEffect
    {
        readonly List<Microsoft.Xna.Framework.Audio.SoundEffect> sounds = new List<Microsoft.Xna.Framework.Audio.SoundEffect> ();
        readonly Random random = new Random ();

        public List<Microsoft.Xna.Framework.Audio.SoundEffect> Sounds
        {
            get
            {
                return sounds;
            }
        }

        public void Play (
            Microsoft.Xna.Framework.Audio.SoundEffect sound = null)
        {
            if (sound == null)
                sound = GetRandomSound ();
            
            GetInstance (sound).Play ();
        }

        public SoundEffectInstance GetInstance (
            Microsoft.Xna.Framework.Audio.SoundEffect sound = null)
        {
            if (sound == null)
                sound = GetRandomSound ();
            
            return sound.CreateInstance ();
        }

        public Microsoft.Xna.Framework.Audio.SoundEffect GetRandomSound ()
        {
            var count = Sounds.Count - 1;
            var randomSound = random.Next (0, count);

            #if DEBUG
            Debug.WriteLine (
                "Played sound {0} out of {1}.".FormatWith (randomSound + 1, count),
                "SOUND");
            #endif

            return Sounds [randomSound];
        }

        public SoundEffectInstance PlayLooped ()
        {
            var instance = GetInstance ();
            instance.IsLooped = true;
            instance.Play ();
            return instance;
        }

        public void Play3D (Rectangle listenerPosition,
                            Rectangle emitterPosition,
                            Microsoft.Xna.Framework.Audio.SoundEffect sound = null)
        {
            var instance = GetInstance (sound);
            instance.Apply3D (
                new AudioListener {
                    Position = new Vector3 (listenerPosition.Location.ToVector2 (), 0)
                },
                
                new AudioEmitter {
                    Position = new Vector3 (emitterPosition.Location.ToVector2 (), 0)
                }
            );
            instance.Play ();
        }
    }
}

