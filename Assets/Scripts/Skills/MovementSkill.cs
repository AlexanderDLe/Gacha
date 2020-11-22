using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Movement Skill", order = 2)]
    public class MovementSkill : SkillScriptableObject
    {
        private UseMovementSkill useMovementSkill;

        public override void Initialize(GameObject obj, Animator animator, RaycastMousePosition raycaster, string skillType)
        {
            useMovementSkill = obj.GetComponent<UseMovementSkill>();
            useMovementSkill.Initialize(animator, raycaster, skillType);
        }

        public override void TriggerSkill()
        {
            useMovementSkill.Activate();
        }
    }
}