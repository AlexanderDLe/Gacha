using RPG.Core;

namespace RPG.Combat
{
    public class IE_Damage : IE_Effect
    {
        BaseManager baseManager;
        float damage;

        public IE_Damage(BaseManager baseManager, float damage)
        {
            this.baseManager = baseManager;
            this.damage = damage;
        }

        public void ApplyEffect()
        {
            baseManager.TakeDamage((int)damage);
        }
    }
}