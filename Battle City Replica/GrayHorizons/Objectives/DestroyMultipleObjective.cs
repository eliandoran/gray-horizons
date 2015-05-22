using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.StaticObjects;

namespace GrayHorizons.Objectives
{
    public class DestroyMultipleObjective: TimedObjective
    {
        readonly IEnumerable<ObjectBase> toDestroy;
        readonly string format;
        int leftToDestroy;

        public DestroyMultipleObjective(IEnumerable<ObjectBase> toDestroy, string format)
            : this(toDestroy, format, new TimeSpan(0, 1, 0))
        {
        }

        public DestroyMultipleObjective(IEnumerable<ObjectBase> toDestroy, string format, TimeSpan time)
        {
            this.toDestroy = toDestroy;
            this.format = format;
            TimeLeft = time;
        }

        public override void Update(TimeSpan gameTime)
        {
            var dummiesList = toDestroy.ToList();
            leftToDestroy = dummiesList.Count(GameData.Map.GetObjects().Contains);

            if (leftToDestroy > 0)
            {
                if (TimeLeft > TimeSpan.Zero)
                {
                    Text = format.FormatWith(
                        leftToDestroy,
                        toDestroy.Count(),
                        "{0}:{1}:{2}".FormatWith(
                            TimeLeft.Hours.ToString("D2"),
                            TimeLeft.Minutes.ToString("D2"),
                            TimeLeft.Seconds.ToString("D2")));
                }
            }
            else
                End(true);
            
            base.Update(gameTime);
        }

        public override void End(bool won)
        {
            toDestroy.ToList().ForEach(GameData.Map.Remove);
            base.End(won);
        }

        public override void TimeIsUp()
        {
            End(leftToDestroy <= 0);
        }
    }
}

