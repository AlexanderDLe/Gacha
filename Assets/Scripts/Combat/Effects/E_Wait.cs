﻿using UnityEngine;

namespace RPG.Combat
{
    public class E_Wait : I_Effect
    {
        public E_Wait(out bool shouldWait, out float currentWaitDuration, float waitDuration)
        {
            shouldWait = true;
            currentWaitDuration = waitDuration;
        }

        public void ApplyEffect() { }
    }
}