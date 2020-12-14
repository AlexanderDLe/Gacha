using RPG.Combat;
using UnityEngine;

public class AOE_Execute : AOE_Effect
{
    Collider[] hits;
    EffectPackage effectPackage;

    public AOE_Execute(Collider[] hits, EffectPackage effectPackage)
    {
        this.hits = hits;
        this.effectPackage = effectPackage;
    }

    public AOE_Execute(Vector3 hitPos, float radius, LayerMask layer, float damage, EffectPackage effectPackage)
    {
        this.hits = Physics.OverlapSphere(hitPos, radius, layer);
        this.effectPackage = effectPackage;
    }

    public void ApplyEffect()
    {
        foreach (Collider hit in hits)
        {
            EffectExecutor effectExecutor = hit.GetComponent<EffectExecutor>();
            effectExecutor.ExecuteEffects(effectPackage);
        }
    }


}
