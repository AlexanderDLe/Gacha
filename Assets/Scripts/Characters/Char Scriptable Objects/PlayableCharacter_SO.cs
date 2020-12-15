using RPG.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Create New Playable Character", order = 0)]
    public class PlayableCharacter_SO : BaseCharacter_SO
    {
        [FoldoutGroup("Metadata")]
        public PlayableCharEnum characterEnum;
        [FoldoutGroup("Metadata")]
        public Weapon weapon;

        #region Character FX
        [FoldoutGroup("Character FX")]
        public AudioClip[] dashAudio;
        #endregion

        #region Auto Attack
        [FoldoutGroup("Auto Attack")]
        public AttackTypeEnum attackType;
        [FoldoutGroup("Auto Attack"), HideIf("attackType", AttackTypeEnum.None)]
        public AutoAttack_SO autoAttack_SO;
        #endregion

        #region Skills
        [FoldoutGroup("Skills")]
        public Skill_SO movementSkill;
        [FoldoutGroup("Skills")]
        public Skill_SO primarySkill;
        [FoldoutGroup("Skills")]
        public Skill_SO ultimateSkill;
        #endregion
    }
}