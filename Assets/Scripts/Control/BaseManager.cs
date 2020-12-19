using RPG.Attributes;
using RPG.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Core
{
    public abstract class BaseManager : MonoBehaviour
    {
        [FoldoutGroup("Management Systems")]
        public Animator animator = null;
        [FoldoutGroup("Management Systems")]
        public Stats stats = null;
        [FoldoutGroup("Management Systems")]
        public ObjectPooler debugPooler = null;
        [FoldoutGroup("Management Systems")]
        public EffectExecutor effectExecuter = null;

        public ProjectileLauncher projectileLauncher = null;

        public abstract void TakeDamage(int damage);
    }
}