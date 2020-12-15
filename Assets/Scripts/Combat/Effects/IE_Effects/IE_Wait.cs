using UnityEngine;

namespace RPG.Combat
{
    public class IE_Wait : IE_Effect
    {
        public IE_Wait(out bool shouldWait, out float currentWaitDuration, float waitDuration)
        {
            shouldWait = true;
            currentWaitDuration = waitDuration;
        }

        public void ApplyEffect() { }
    }
}