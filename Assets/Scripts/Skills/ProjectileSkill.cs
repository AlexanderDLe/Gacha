using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Projectile Skill", order = 0)]
    public class ProjectileSkill : SkillScriptableObject
    {
        private UseProjectileSkill useProjectileSkill;

        public override void Initialize(GameObject player_GO, string skillType)
        {
            useProjectileSkill = player_GO.GetComponent<UseProjectileSkill>();
            useProjectileSkill.Initialize(player_GO, skillType);
        }

        public override void TriggerSkill()
        {
            useProjectileSkill.Activate();
        }
    }
}