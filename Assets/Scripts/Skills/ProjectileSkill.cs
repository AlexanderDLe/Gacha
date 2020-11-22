using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Projectile Skill", order = 0)]
    public class ProjectileSkill : SkillScriptableObject
    {
        private UseProjectileSkill useProjectileSkill;

        public override void Initialize(GameObject obj, Animator animator, RaycastMousePosition raycaster, string skillType)
        {
            useProjectileSkill = obj.GetComponent<UseProjectileSkill>();
            useProjectileSkill.Initialize(animator, raycaster, skillType);
        }

        public override void TriggerSkill()
        {
            useProjectileSkill.Activate();
        }
    }
}