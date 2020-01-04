using System;
using System.Collections.ObjectModel;

namespace GrayHorizons.Objectives
{
    public class ObjectiveList: Collection<Objective>
    {
        public string Summary;

        public Objective GetFirstUncompletedObjective()
        {
            foreach (Objective objective in this)
                if (!objective.IsCompleted)
                    return objective;
            return null;
        }
    }
}

