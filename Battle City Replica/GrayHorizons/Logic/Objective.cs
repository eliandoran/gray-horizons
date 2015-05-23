namespace GrayHorizons.Logic
{
    using System;
    using GrayHorizons.Screens;
    using System.Diagnostics;
    using GrayHorizons.Extensions;

    public abstract class Objective
    {
        public event EventHandler Starting;

        public event EventHandler Ending;

        public event EventHandler Won;

        public event EventHandler Lost;

        public bool IsCompleted { get; set; }

        public bool ShouldStart { get; set; }

        public string Text { get; set; }

        public string InitialMessage { get; set; }

        public TimeSpan InitialMessageDuration { get; set; }

        public string EndingMessage { get; set; }

        public GameData GameData { get; set; }

        public TimeSpan StartDelay { get; set; }

        public TimeSpan CurrentStartDelay { get; set; }

        public virtual void Startup()
        {
            if (CurrentStartDelay > TimeSpan.Zero)
            {
                ShouldStart = true;
                return;
            }

            OnStarting(EventArgs.Empty);

            if (!String.IsNullOrEmpty(InitialMessage))
            {
                GameData.ScreenManager.AddScreen(
                    new MessageScreen(
                        GameData,
                        InitialMessage)
                    {
                        TimeLeft = InitialMessageDuration != TimeSpan.Zero ? InitialMessageDuration : TimeSpan.FromSeconds(3)
                    },
                    null);
            }
        }

        public virtual void End(bool won)
        {
            OnEnding(EventArgs.Empty);
            Debug.WriteLine(won);

            if (won)
                OnWon(EventArgs.Empty);
            else
                OnLost(EventArgs.Empty);

            if (!String.IsNullOrEmpty(EndingMessage))
            {
                GameData.ScreenManager.AddScreen(
                    new MessageScreen(
                        GameData,
                        EndingMessage),
                    null);
            }

            IsCompleted = true;
        }

        public virtual void CheckCompletion()
        {

        }

        public virtual void Update(TimeSpan gameTime)
        {
            if (ShouldStart)
            {
                if (CurrentStartDelay > gameTime)
                    CurrentStartDelay -= gameTime;
                else
                {
                    CurrentStartDelay = TimeSpan.Zero;
                    Startup();
                }
            }
        }

        protected virtual void OnStarting(EventArgs e)
        {
            if (Starting.IsNotNull())
                Starting(this, e);
        }

        protected virtual void OnEnding(EventArgs e)
        {
            if (Ending.IsNotNull())
                Ending(this, e);
        }

        protected virtual void OnWon(EventArgs e)
        {
            if (Won.IsNotNull())
                Won(this, e);
        }

        protected virtual void OnLost(EventArgs e)
        {
            if (Lost.IsNotNull())
                Lost(this, e);
        }
    }
}

