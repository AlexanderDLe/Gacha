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

        // bool shouldWait = false;
        // float waitDuration = 0f;

        public void Initialize(BaseStats baseStats)
        {
            this.baseStats = baseStats;
            this.baseManager = GetComponent<BaseManager>();
        }

        public void ExecuteEffects(EffectPackage effectPackage)
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
                    case EffectEnum.Wait:
                        Executor(new E_Wait(out shouldWait, out waitDuration, effect.duration));
                        break;
                    case EffectEnum.Damage:
                        Executor(new E_Damage(baseManager, effect.value));
                        break;
                    default:
                        break;
                }
            }
        }

        public void Executor(I_Effect effect)
        {
            effect.ApplyEffect();
        }
    }
}