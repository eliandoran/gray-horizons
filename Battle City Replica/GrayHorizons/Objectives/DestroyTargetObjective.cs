using System;
using GrayHorizons.Logic;

namespace GrayHorizons.Objectives
{
    public class DestroyTargetObjective: Objective
    {
        readonly ObjectBase targetToDestroy;

        public DestroyTargetObjective(ObjectBase targetToDestroy)
        {
            this.targetToDestroy = targetToDestroy;
        }

        public override void CheckCompletion()
        {
            IsCompleted = !GameData.Map.GetObjects().Contains(targetToDestroy);
            base.CheckCompletion();
        }
    }
}

