using UnityEngine;

namespace RPG.Combat
{
    public class IAOE_Wait : IAOE_Effect
    {
        public IAOE_Wait(out bool shouldWait, out float currentWaitDuration, float waitDuration)
        {
            shouldWait = true;
            currentWaitDuration = waitDuration;
        }

        public void ApplyEffect() { }
    }
}