using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    interface IAudio
    {
        void PunchSound();
        void DeathSound();
        void StartAggroSound();
    }

}