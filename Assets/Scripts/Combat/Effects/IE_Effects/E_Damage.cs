using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class E_Damage : E_Effect
    {
        Stats originStats;
        Stats targetStats;
        BaseManager baseManager;
        DamagePacket damagePacket;
        float totalDamage;

        public E_Damage(Stats originStats, BaseManager baseManager, DamagePacket damagePacket)
        {
            this.originStats = originStats;
            this.baseManager = baseManager;
            this.targetStats = baseManager.stats;
            this.damagePacket = damagePacket;
        }

        public void ApplyEffect()
        {
            ProcessPhysDamage();
            ProcessOriginStats();
            ProcessTargetStats();
            ProcessDamageVariation();

            baseManager.TakeDamage((int)totalDamage);
        }

        private void ProcessPhysDamage()
        {
            totalDamage = damagePacket.physDmgFraction;
        }

        private void ProcessOriginStats()
        {
            totalDamage *= originStats.damage;
        }

        private void ProcessTargetStats()
        {
            totalDamage = totalDamage * (1 - targetStats.defense / 100);
        }

        private void ProcessDamageVariation()
        {
            float variationAmount = Random.Range(-10f, 10f);
            totalDamage *= 1 + variationAmount / 100;
        }
    }
}