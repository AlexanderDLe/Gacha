using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters
{
    public abstract class SkillEventHandler : MonoBehaviour
    {
        public float probability = .5f;
        public abstract void LinkReferences(AudioManager audioManager, ObjectPooler objectPooler, RaycastMousePosition raycaster, Animator animator);

        public abstract void Initialize(BaseStats baseStats, PlayableCharacter_SO charScript);
        public abstract void InitializeSkillManager(SkillManager movementSkill, SkillManager primarySkill, SkillManager ultimateSkill);

        public abstract void InitializeMovementSkill();
        public abstract void InitializePrimarySkill();
        public abstract void InitializeUltimateSkill();

        public abstract void TriggerMovementSkill();
        public abstract void TriggerPrimarySkill();
        public abstract void TriggerUltimateSkill();
    }
}