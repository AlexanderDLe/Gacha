using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class EffectExecutor : MonoBehaviour
    {
        BaseStats baseStats = null;
        BaseManager baseManager = null;

        public void Initialize(BaseStats baseStats)
        {
            this.baseStats = baseStats;
            this.baseManager = GetComponent<BaseManager>();
        }

        public void Execute(EffectPackage effectPackage)
        {
            StartCoroutine(ExecuteChain(effectPackage));
        }

        IEnumerator ExecuteChain(EffectPackage effectPackage)
        {
            bool shouldWait = false;
            float waitDuration = 0f;

            foreach (EffectItem effect in effectPackage.effectItems)
            {
                if (shouldWait)
                {
                    yield return new WaitForSeconds(waitDuration);
                    shouldWait = false;
                }
                switch (effect.effectEnum)
                {
                    case IE_Enum.Wait:
                        Executor(new IE_Wait(out shouldWait, out waitDuration, effect.duration));
                        break;
                    case IE_Enum.Damage:
                        Executor(new IE_Damage(baseManager, effect.value));
                        break;
                    default:
                        break;
                }
            }
        }

        public void Executor(IE_Effect effect)
        {
            effect.ApplyEffect();
        }
    }
}