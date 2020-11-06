using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction oldAction;
        //monobehavior uses polymorphism to substitute cancel with monobehavior as parent
        public void StartAction(IAction newAction)
        {
            //We do not need to cancel an old action
            if (oldAction == newAction) return;

            //There are two actions happening at once. Always cancel the old action
            if (oldAction != null)
            {
                oldAction.Cancel();
            }

            //Now we have one action, so get rid of the old one.
            oldAction = newAction;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
