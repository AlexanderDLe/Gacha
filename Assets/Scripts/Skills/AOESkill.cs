﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [CreateAssetMenu(menuName = "Abilities/Create New AOE Skill", order = 1)]
    public class AOESkill : SkillScriptableObject
    {
        private UseAOESkill useAOESkill;

        public override void Initialize(GameObject obj, Animator animator, string skillType)
        {
            useAOESkill = obj.GetComponent<UseAOESkill>();
            useAOESkill.Initialize(animator, skillType);
        }

        public override void TriggerSkill()
        {
            useAOESkill.Activate();
        }
    }
}