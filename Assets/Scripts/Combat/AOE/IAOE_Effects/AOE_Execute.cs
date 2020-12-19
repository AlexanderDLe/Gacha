using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

public class AOE_Execute : AOE_Effect
{
    Stats originStats;
    Collider[] hits;
    EffectPackage effectPackage;

    public AOE_Execute(Stats originStats, Collider[] hits, EffectPackage effectPackage)
    {
        this.originStats = originStats;
        this.hits = hits;
        this.effectPackage = effectPackage;
    }

    public AOE_Execute(Stats originStats, Vector3 hitPos, float radius, LayerMask layer, EffectPackage effectPackage)
    {
        this.originStats = originStats;
        this.hits = Physics.OverlapSphere(hitPos, radius, layer);
        this.effectPackage = effectPackage;
    }

    public void ApplyEffect()
    {
        foreach (Collider hit in hits)
        {
            EffectExecutor effectExecutor = hit.GetComponent<EffectExecutor>();
            effectExecutor.Execute(originStats, effectPackage);
        }
    }


}
