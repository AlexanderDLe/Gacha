using RPG.Combat;
using UnityEngine;

namespace RPG.Core
{
    public abstract class BaseManager : MonoBehaviour
    {
        public abstract void TakeDamage(int damage);
        public abstract void ExecuteEffectPackage(EffectPackage effectPackage);
    }
}