namespace GrayHorizons.Objectives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;

    public class ObjectiveList: List<Objective>
    {
        public string Summary { get; set; }

        GameData gameData;

        public GameData GameData
        {
            get
            {
                return gameData;
            }
            set
            {
                gameData = value;
                UpdateGameData();
            }
        }

        public Objective GetFirstUncompletedObjective()
        {
            return this.FirstOrDefault((Objective arg) => !arg.IsCompleted);
        }

        public void Update(TimeSpan gameTime)
        {
            var objective = GetFirstUncompletedObjective();
            if (objective.IsNotNull())
            {
                objective.Update(gameTime);
                objective.CheckCompletion();

                if (objective.IsCompleted)
                {
                    //objective.End(true);

                    var nextObjective = GetFirstUncompletedObjective();
                    if (nextObjective.IsNotNull())
                    {
                        nextObjective.Startup();
                    }
                }
            }
        }

        public void SkipTo(int objectiveIndex)
        {
            for (var index = 0; index < Count; index++)
            {
                var objective = this[index];
                objective.IsCompleted = (index < objectiveIndex);
            }

            GetFirstUncompletedObjective();
        }

        void UpdateGameData()
        {
            ForEach(objective => objective.GameData = GameData);
        }
    }
}

