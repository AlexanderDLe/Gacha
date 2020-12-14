using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class E_Damage : I_Effect
    {
        BaseManager baseManager;
        float damage;

        public E_Damage(BaseManager baseManager, float damage)
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