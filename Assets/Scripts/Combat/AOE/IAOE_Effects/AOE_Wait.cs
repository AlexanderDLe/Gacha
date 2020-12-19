namespace RPG.Combat
{
    public class AOE_Wait : AOE_Effect
    {
        public AOE_Wait(out bool shouldWait, out float currentWaitDuration, float waitDuration)
        {
            shouldWait = true;
            currentWaitDuration = waitDuration;
        }

        public void ApplyEffect() { }
    }
}