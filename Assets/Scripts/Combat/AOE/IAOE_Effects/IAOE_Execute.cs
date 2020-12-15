using RPG.Combat;
using UnityEngine;

public class IAOE_Execute : IAOE_Effect
{
    Collider[] hits;
    EffectPackage effectPackage;

    public IAOE_Execute(Collider[] hits, EffectPackage effectPackage)
    {
        this.hits = hits;
        this.effectPackage = effectPackage;
    }

    public IAOE_Execute(Vector3 hitPos, float radius, LayerMask layer, EffectPackage effectPackage)
    {
        this.hits = Physics.OverlapSphere(hitPos, radius, layer);
        this.effectPackage = effectPackage;
    }

    public void ApplyEffect()
    {
        foreach (Collider hit in hits)
        {
            EffectExecutor effectExecutor = hit.GetComponent<EffectExecutor>();
            effectExecutor.Execute(effectPackage);
        }
    }


}
