namespace GrayHorizons.Objectives
{
    using System;
    using GrayHorizons.Logic;

    public class TimedObjective: Objective
    {
        public TimeSpan TimeLeft { get; set; }

        public override void Update(TimeSpan gameTime)
        {
            if (TimeLeft > gameTime)
                TimeLeft -= gameTime;
            else
                TimeIsUp();
        }

        public virtual void TimeIsUp()
        {

        }
    }
}

