using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class SkillEventHandler : MonoBehaviour
    {
        public float probability = .5f;
        public abstract void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator, MeleeAttacker meleeAttacker, ProjectileLauncher projectileLauncher, AOECaster aoeCaster);

        public abstract void Initialize(BaseStats baseStats, PlayableCharacter_SO charScript);
        public abstract void InitializeSkills();

        public abstract void TriggerMovementSkill();
        public abstract void TriggerPrimarySkill();
        public abstract void TriggerUltimateSkill();
    }
}