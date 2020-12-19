using System.Collections;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class EffectExecutor : MonoBehaviour
    {
        BaseManager baseManager;

        private void Awake()
        {
            this.baseManager = GetComponent<BaseManager>();
        }

        public void Execute(Stats originStats, EffectPackage effectPackage)
        {
            StartCoroutine(ExecuteChain(originStats, effectPackage));
        }

        IEnumerator ExecuteChain(Stats originStats, EffectPackage effectPackage)
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
                    case E_Enum.Wait:
                        Executor(new E_Wait(out shouldWait, out waitDuration, effect.duration));
                        break;
                    case E_Enum.Damage:
                        Executor(new E_Damage(originStats, baseManager, effect.damagePacket));
                        break;
                    default:
                        break;
                }
            }
        }

        public void Executor(E_Effect effect)
        {
            effect.ApplyEffect();
        }
    }
}