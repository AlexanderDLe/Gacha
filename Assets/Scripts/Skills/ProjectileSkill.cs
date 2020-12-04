﻿using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New Projectile Skill", order = 0)]
    public class ProjectileSkill : SkillScriptableObject
    {
        RaycastMousePosition raycaster = null;
        Animator animator = null;
        GameObject player = null;

        public override void Initialize(GameObject player_GO, string skillType)
        {
            this.raycaster = player_GO.GetComponent<RaycastMousePosition>();
            this.animator = player_GO.GetComponent<Animator>();
            this.player = player_GO;
        }

        public override void TriggerSkill(string skillType)
        {
            RaycastHit hit = raycaster.GetRaycastMousePoint();
            player.transform.LookAt(hit.point);
            animator.SetTrigger(skillType);
        }
    }
}