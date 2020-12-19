using RPG.Characters;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.PlayerStates
{
    public class P_Skill : IState
    {
        PlayerManager playerManager;
        Animator animator = null;
        SkillManager skill;
        SkillEventHandler skillEventHandler;
        string skillType;

        public P_Skill(GameObject playerObject, Animator animator, PlayerManager playerManager, SkillManager skill, SkillEventHandler skillEventHandler, RaycastMousePosition raycaster)
        {
            this.animator = animator;
            this.playerManager = playerManager;
            this.skill = skill;
            this.skillType = skill.skillType;
            this.skillEventHandler = skillEventHandler;

            raycaster.RotateObjectTowardsMousePosition(playerObject);
            animator.SetTrigger(skillType);
        }

        public void Enter()
        {
            skill.SetIsUsingSkill(true);
            playerManager.DeactivateSkillAim(skill);
            skillEventHandler.enterSkillDict[skillType]();
        }

        public void Execute()
        {
            skillEventHandler.executeSkillDict[skillType]();
        }
        public void Exit()
        {
            skill.SetIsUsingSkill(false);
            skillEventHandler.exitSkillDict[skillType]();
            animator.ResetTrigger(skillType);
        }
    }
}